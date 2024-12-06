using WpfApp1.Services.Import;

namespace WpfApp1.ViewModel.Factories
{
    internal interface IImporterFactory
    {
        IDataImporter GetImporter(string importerName);
    }
}