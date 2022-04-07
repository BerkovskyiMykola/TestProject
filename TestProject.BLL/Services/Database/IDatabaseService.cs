using TestProject.BLL.DTO;

namespace TestProject.BLL.Services.Database
{
    public interface IDatabaseService
    {
        IEnumerable<BackupDTO> GetBackups();
        Task<BackupDTO> CreateBackupAsync();
        void DeleteBackup(string backupName);
        Task RestoreAsync(string backupName);
    }
}
