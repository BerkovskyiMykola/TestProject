using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DatabaseController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly string _directoryPath;

        public DatabaseController(ApplicationContext context)
        {
            _context = context;
            _directoryPath = $"C:\\{_context.Database.GetDbConnection().Database}_Backups";

            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
        }

        [HttpGet("backups")]
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
            var backupName = $"{DateTime.UtcNow:yyyy-MM-dd_hh-mm-ss}";

            var path = Path.Combine(_directoryPath, backupName + ".bak");
            if (System.IO.File.Exists(path))
            {
                return BadRequest();
            }

            string sqlCommand = $"BACKUP DATABASE {dbname} TO DISK = '{_directoryPath}\\{backupName}.bak'";
            var a = await _context.Database.ExecuteSqlRawAsync(sqlCommand);
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

            try
            {
                System.IO.File.Delete(path);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("restore/{backupName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Restore(string backupName)
        {
            var path = Path.Combine(_directoryPath, backupName + ".bak");

            if (!System.IO.File.Exists(path))
            {
                return BadRequest();
            }

            string dbname = _context.Database.GetDbConnection().Database;
            string sqlCommand = $"ALTER DATABASE {dbname} SET Single_User WITH Rollback Immediate; " +
                $"USE master RESTORE DATABASE {dbname} FROM DISK = '{path}';" +
                $"ALTER DATABASE {dbname} SET Multi_User";
            await _context.Database.ExecuteSqlRawAsync(sqlCommand);
            return Ok();
        }
    }
}
