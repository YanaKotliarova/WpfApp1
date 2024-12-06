using WpfApp1.Services.Import;

namespace WpfApp1.ViewModel.Factories
{
    class ImporterFactory : IImporterFactory
    {
        private readonly Func<IEnumerable<IDataImporter>> _factory;
        public ImporterFactory(Func<IEnumerable<IDataImporter>> factory)
        {
            _factory = factory;
        }
        public IDataImporter GetImporter(string importerName)
        {
            var set = _factory();
            IDataImporter output = set.Where(x => x.ImporterName == importerName).First();
            return output;
        }
    }
}
