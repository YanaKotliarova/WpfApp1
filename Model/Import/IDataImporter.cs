using WpfApp1.Model.MainModel;

namespace WpfApp1.Model.Import
{
    internal interface IDataImporter
    {
        Task ReadFromFileAsync(string fileName, List<User> ListOfUsersFromFile);
    }
}