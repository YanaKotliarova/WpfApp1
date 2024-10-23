using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.Model.Interfaces;
using WpfApp1.Services.Import;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.DependencyInjection;
using WpfApp1.ViewModel.Factories.Interfaces;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1.View.Pages
{
    public partial class Import : Page
    {
        private IAbstractFactory<IDataImporter> _importerFactory =
            App.serviceProvider.GetService<IAbstractFactory<IDataImporter>>()!;

        private IAbstractFactory<IRepository<User>> _repositoryFactory =
            App.serviceProvider.GetService<IAbstractFactory<IRepository<User>>>()!;

        private IAbstractFactory<IFileDialog> _fileDialogFactory =
            App.serviceProvider.GetService<IAbstractFactory<IFileDialog>>()!;

        private IAbstractFactory<IMessage> _messageFactory =
            App.serviceProvider.GetService<IAbstractFactory<IMessage>>()!;

        private IAbstractFactory<IUsers> _usersFactory =
            App.serviceProvider.GetService<IAbstractFactory<IUsers>>()!;

        public Import()
        {
            DependencyStruct dependencyStruct = 
                new DependencyStruct(_importerFactory, _repositoryFactory, _fileDialogFactory, _messageFactory, _usersFactory);

            InitializeComponent();
            DataContext = new ImportPageViewModel(dependencyStruct);
        }
    }
}
