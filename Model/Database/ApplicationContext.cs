using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WpfApp1.Model.MainModel;

namespace WpfApp1.Model.Database
{
    internal class ApplicationContext : DbContext
    {
        private const string DefaultConnection = "DefaultConnection";
        internal DbSet<User> Users { get; set; } = null!;

        string connectionString = ConfigurationManager.ConnectionStrings[DefaultConnection].ConnectionString;

        internal string ReturnConnectionString()
        {
            return connectionString;
        }
        /// <summary>
        /// The method of connecting to the DB.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

    }
}
