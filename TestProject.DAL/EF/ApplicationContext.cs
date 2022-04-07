using Microsoft.EntityFrameworkCore;
using TestProject.DAL.Entities;

namespace TestProject.DAL.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
    }
}
