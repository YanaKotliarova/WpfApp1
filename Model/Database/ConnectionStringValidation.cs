using Microsoft.Data.SqlClient;
using WpfApp1.Model.Database.Interfaces;

namespace WpfApp1.Model.Database
{
    internal class ConnectionStringValidation : IConnectionStringValidation
    {
        /// <summary>
        /// A method for verifying the validity of the DB connection string.
        /// </summary>
        /// <returns></returns>
        public bool ValidateConnectionString(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                }
                return true;
            }
            catch (SqlException)
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
