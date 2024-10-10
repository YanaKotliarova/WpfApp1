using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using WpfApp1.Model.MainModel;

namespace WpfApp1.Model.Database
{
    internal class ApplicationContext : DbContext
    {
        private const string DefaultConnectionString = "DefaultConnection";
        private const string ConfigurationFile = "connectionConfiguration.json";
        internal DbSet<User> Users { get; set; } = null!;

        IConfigurationRoot configuration = new ConfigurationBuilder()
                                .AddJsonFile(ConfigurationFile)
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .Build();

        /// <summary>
        /// A method for verifying the validity of the DB connection string.
        /// </summary>
        /// <returns></returns>
        internal bool ValidateConnectionString()
        {
            try
            {
                using (var con = new SqlConnection(configuration.GetConnectionString(DefaultConnectionString)))
                {
                    con.Open();
                }
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (SqlException)
            {
                return true;
            }
        }

        /// <summary>
        /// The method of connecting to the DB.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(DefaultConnectionString));
        }

    }
}
