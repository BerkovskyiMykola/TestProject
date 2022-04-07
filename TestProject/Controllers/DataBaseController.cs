using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.BLL.Exceptions;
using TestProject.BLL.Services.BackupAndRestore;

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DatabaseController : ControllerBase
    {
        private readonly IBackupAndRestoreService _backupAndRestoreService;

        public DatabaseController(IBackupAndRestoreService backupAndRestoreService)
        {
            _backupAndRestoreService = backupAndRestoreService;
        }

        [HttpGet("backups")]
        public IActionResult GetBackups()
        {
            try
            {
                var response = _backupAndRestoreService.GetBackups();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("createBackup")]
        public async Task<IActionResult> PostBackup()
        {
            try
            {
                var response = await _backupAndRestoreService.CreateBackupAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deleteBackup/{backupName}")]
        public IActionResult DeleteBackup(string backupName)
        {
            try
            {
                _backupAndRestoreService.DeleteBackup(backupName);

                return Ok();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("restore/{backupName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Restore(string backupName)
        {
            try
            {
                await _backupAndRestoreService.RestoreAsync(backupName);

                return NoContent();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
