namespace WpfApp1.Model.Database.Interfaces
{
    internal interface IConnectionStringValidation
    {
        bool ValidateConnectionString(string connectionString);
    }
}