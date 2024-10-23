using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.Model.Interfaces;
using WpfApp1.Services.Import;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.ViewModel.DependencyInjection
{
    internal struct DependencyStruct
    {
        public IExporterFactory Exporter { get; set; }
        public IAbstractFactory<IRepository<User>> Repository { get; set; }
        public IAbstractFactory<IDataImporter> Importer { get; set; }
        public IAbstractFactory<IDataFormatter> DataFormatter { get; set; }
        public IAbstractFactory<IConnectionStringValidation> ConnectionStringValidation { get; set; }
        public IAbstractFactory<IMessage> Message { get; set; }
        public IAbstractFactory<IFileDialog> FileDialog { get; set; }
        public IAbstractFactory<IUsers> Users { get; set; }

        public DependencyStruct(IAbstractFactory<IRepository<User>> repository, IAbstractFactory<IMessage> message)
        {
            Repository = repository;
            Message = message;
        }
        public DependencyStruct(IExporterFactory exporter, IAbstractFactory<IRepository<User>> repository, IAbstractFactory<IDataFormatter> dataFormatter, IAbstractFactory<IMessage> message, IAbstractFactory<IUsers> users)
        {
            Exporter = exporter;
            Repository = repository;
            DataFormatter = dataFormatter;
            Message = message;
            Users = users;
        }

        public DependencyStruct(IAbstractFactory<IDataImporter> importer, IAbstractFactory<IRepository<User>> repository, IAbstractFactory<IFileDialog> fileDialog, IAbstractFactory<IMessage> message, IAbstractFactory<IUsers> users)
        {
            Importer = importer;
            Repository = repository;
            Message = message;
            FileDialog = fileDialog;
            Users = users;
        }

        public DependencyStruct(IAbstractFactory<IUsers> users)
        {
            Users = users;
        }
    }
}
