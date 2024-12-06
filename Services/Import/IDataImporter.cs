using WpfApp1.Model;

namespace WpfApp1.Services.Import
{
    internal interface IDataImporter
    {
        string ImporterName { get; set; }
        IAsyncEnumerable<List<User>> ReadFromFileAsync(string fileName);
    }
}