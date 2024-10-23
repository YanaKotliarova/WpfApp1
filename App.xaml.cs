using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfApp1.Data.Database;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.Model.Interfaces;
using WpfApp1.Services.Import;
using WpfApp1.View.UI;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.DependencyInjection;

namespace WpfApp1
{
    public partial class App : Application
    {
        public static ServiceCollection services;
        public static ServiceProvider serviceProvider;

        [STAThread]
        public static void Main()
        {
            services = new ServiceCollection();

            services.AddExporterFactory();
            services.AddAbstractFactory<IDataImporter, CsvImporter>();
            services.AddAbstractFactory<IRepository<User>, DataBase>();
            services.AddAbstractFactory<IDataFormatter, DataFormatter>();
            services.AddAbstractFactory<IConnectionStringValidation, ConnectionStringValidation>();
            services.AddAbstractFactory<IMessage, Message>();
            services.AddAbstractFactory<IFileDialog, FileDialog>();
            services.AddAbstractFactory<IUsers, Users>();
                        

            App app = new App();                        
            MainWindow window = new MainWindow();
            window.Title = "WPF Exporter";


            using (serviceProvider = services.BuildServiceProvider())
            {
                app.Run(window);
            }
        }
    }
}
