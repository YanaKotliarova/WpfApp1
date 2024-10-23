using WpfApp1.Model;

namespace WpfApp1.Services.Import
{
    internal interface IDataImporter
    {
        Task ReadFromFileAsync(string fileName, List<User> ListOfUsersFromFile);
    }
}