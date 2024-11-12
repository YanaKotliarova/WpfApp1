using WpfApp1.Model;

namespace WpfApp1.Services.Import
{
    internal interface IDataImporter
    {
        IAsyncEnumerable<List<User>> ReadFromFileAsync(string fileName);
    }
}