using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WpfApp1.Model;

namespace WpfApp1.Data.Database
{
    internal class ApplicationContext : DbContext
    {
        private const string DefaultConnection = "DefaultConnection";
        internal DbSet<User> Users { get; set; } = null!;

        private string _connectionString = ConfigurationManager.ConnectionStrings[DefaultConnection].ConnectionString;

        internal string ReturnConnectionString()
        {
            return _connectionString;
        }
        internal void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// The method of connecting to the DB.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

    }
}
