using WpfApp1.Model.MainModel;

namespace WpfApp1.Model.Export
{
    internal interface IDataExporter
    {
        string ExporterName { get; set; }
        Task AddToFileAsync(string excelFileName, List<User> ListOfUsersFromDB);
        Task CreateFileAsync(string excelFileName);
    }
}