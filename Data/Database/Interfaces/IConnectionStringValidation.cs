namespace WpfApp1.Data.Database.Interfaces
{
    internal interface IConnectionStringValidation
    {
        bool ValidateConnectionString(string connectionString);
    }
}