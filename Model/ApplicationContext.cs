using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.Model
{
    internal class ApplicationContext : DbContext
    {
        internal DbSet<User> Users { get; set; } = null!;
        public string connectionString = @"Server=localhost;Database=Users;Trusted_Connection=True;TrustServerCertificate=true;";
        //public string connectionString = @"blablabla";

        public ApplicationContext()
        {
            //if (!ValidateConnectionString()) throw new Exception("Не валидатная строка подключения к БД.");
            //Database.Migrate();
        }

        /// <summary>
        /// Метод проверки валидности строки подключения к БД.
        /// </summary>
        /// <returns></returns>
        internal bool ValidateConnectionString()
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

        /// <summary>
        /// Метод подключения к БД.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
