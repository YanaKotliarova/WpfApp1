using WpfApp1.Services.Export;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.ViewModel.Factories
{
    internal class ExporterFactory : IExporterFactory
    {
        private readonly Func<IEnumerable<IDataExporter>> _factory;
        public ExporterFactory(Func<IEnumerable<IDataExporter>> factory)
        {
            _factory = factory;
        }
        public IDataExporter GetExporter(string exporterName)
        {
            var set = _factory();
            IDataExporter output = set.Where(x => x.ExporterName == exporterName).First();
            return output;
        }
    }
}
