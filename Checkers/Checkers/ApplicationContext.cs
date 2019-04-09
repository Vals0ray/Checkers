using Microsoft.EntityFrameworkCore;

namespace Checkers
{
    public class ApplicationContext : DbContext
    {
        private string _databasePath;

        public DbSet<DBModel> DBModels { get; set; }

        public ApplicationContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }

    public interface IPath
    {
        string GetDatabasePath(string filename);
    }
}