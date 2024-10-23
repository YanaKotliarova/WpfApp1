using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.Model.Interfaces;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.DependencyInjection;
using WpfApp1.ViewModel.Factories.Interfaces;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1.View.Pages
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class Export : Page
    {
        private IExporterFactory _exporterFactory = App.serviceProvider.GetService<IExporterFactory>()!;

        private IAbstractFactory<IRepository<User>> _repositoryFactory = 
            App.serviceProvider.GetService<IAbstractFactory<IRepository<User>>>()!;

        private IAbstractFactory<IDataFormatter> _dataFormatterFactory = 
            App.serviceProvider.GetService<IAbstractFactory<IDataFormatter>>()!;

        private IAbstractFactory<IMessage> _messageFactory =
            App.serviceProvider.GetService<IAbstractFactory<IMessage>>()!;

        private IAbstractFactory<IUsers> _usersFactory =
            App.serviceProvider.GetService<IAbstractFactory<IUsers>>()!;


        public Export()
        {
            DependencyStruct dependencyStruct = 
                new DependencyStruct(_exporterFactory, _repositoryFactory, _dataFormatterFactory, _messageFactory, _usersFactory);

            InitializeComponent();
            DataContext = new ExportPageViewModel(dependencyStruct);
        }
    }
}
