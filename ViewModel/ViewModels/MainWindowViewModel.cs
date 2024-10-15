using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Model.Database.Interfaces;
using WpfApp1.Model.Database;
using WpfApp1.Model.Import;
using WpfApp1.Model.MainModel.Interfaces;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.DependencyInjection;
using WpfApp1.Model.MainModel;
using WpfApp1.View.UI;
using WpfApp1.MVVM;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class MainWindowViewModel
    {
        public static ServiceCollection services;    
        public static ServiceProvider serviceProvider;

        public MainWindowViewModel()
        {
            services = new ServiceCollection();

            services.AddExporterFactory();
            services.AddAbstractFactory<IDataImporter, CsvImporter>();
            services.AddAbstractFactory<IRepository<User>, DataBase>();
            services.AddAbstractFactory<IDataFormatter, DataFormatter>();
            services.AddAbstractFactory<IConnectionStringValidation, ConnectionStringValidation>();
            services.AddAbstractFactory<IMessage, Message>();
            services.AddAbstractFactory<IFileDialog, FileDialog>();
            services.AddAbstractFactory<IUser, User>();
                       
        }

        private RelayCommand _initializeDBCommand;
        /// <summary>
        /// A command to validate the connection string and the availability 
        /// of DB and initialize it, if necessary.
        /// Called when the main window is loaded.
        /// </summary>
        public RelayCommand InitializeDBCommand
        {
            get
            {
                return _initializeDBCommand ??
                    (_initializeDBCommand = new RelayCommand(async obj =>
                    {
                        var db = serviceProvider.GetService<IAbstractFactory<IRepository<User>>>()!.Create();
                        await db.InitializeDBAsync();
                    }
                    ));
            }
        }
    }

}