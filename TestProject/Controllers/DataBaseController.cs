using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DataBaseController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly string _directoryPath;

        public DataBaseController(ApplicationContext context)
        {
            _context = context;
            _directoryPath = $"C:\\{_context.Database.GetDbConnection().Database}_Backups";

            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
        }

        [HttpGet("Backups")]
        public IActionResult GetBackups()
        {
            DirectoryInfo directory = new DirectoryInfo(_directoryPath);

            FileInfo[] files = directory.GetFiles("*.bak");

            return Ok(files.Select(x => new { BackupName = x.Name[..(x.Name.Length-4)] }));
        }

        [HttpPost("createBackup")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostBackup()
        {
            string dbname = _context.Database.GetDbConnection().Database;
            var backupName = $"{dbname}_{DateTime.UtcNow:yyyy-MM-dd_hh-mm-ss}";
            string sqlCommand = $"BACKUP DATABASE {dbname} TO DISK = '{_directoryPath}\\{backupName}.bak'";
            await _context.Database.ExecuteSqlRawAsync(sqlCommand);
            return Ok(new { BackupName = backupName });
        }

        [HttpDelete("deleteBackup/{backupName}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteBackup(string backupName)
        {
            var path = Path.Combine(_directoryPath, backupName + ".bak");
            if (!System.IO.File.Exists(path))
            {
                return BadRequest();
            }

            System.IO.File.Delete(path);

            return NoContent();
        }

        [HttpPut("restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Restore(string backupName)
        {
            var path = Path.Combine(_directoryPath, backupName + ".bak");

            if (!System.IO.File.Exists(path))
            {
                return BadRequest();
            }

            string dbname = _context.Database.GetDbConnection().Database;
            string sqlCommand1 = $"USE master RESTORE DATABASE {dbname} FROM DISK = '{path}'";
            await _context.Database.ExecuteSqlRawAsync(sqlCommand1);
            return Ok();
        }
    }
}
