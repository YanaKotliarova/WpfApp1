using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using WpfApp1.View;

namespace WpfApp1.Model
{
    internal class ApplicationContext : DbContext
    {
        internal DbSet<User> Users { get; set; } = null!;
        public string connectionString = @"Server=localhost;Database=Users;Trusted_Connection=True;TrustServerCertificate=true;";
        //public string connectionString = @"blablabla";

        protected bool ValidateConnectionString()
        {
            try
            {
                using (var con = new SqlConnection(connectionString))
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
        public ApplicationContext()
        {
            if (!ValidateConnectionString()) throw new Exception("Не валидатная строка подключения к БД.");
            //Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
