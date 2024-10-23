using WpfApp1.Services.Export;

namespace WpfApp1.ViewModel.Factories.Interfaces
{
    internal interface IExporterFactory
    {
        IDataExporter GetExporter(string exportetName);
    }
}