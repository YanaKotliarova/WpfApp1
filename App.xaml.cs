using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfApp1.Model.Database;
using WpfApp1.Model.Database.Interfaces;
using WpfApp1.Model.Import;
using WpfApp1.Model.MainModel;
using WpfApp1.Model.MainModel.Interfaces;
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
        static void Main()
        {
            App app = new App();
            MainWindow window = new MainWindow();
            window.Title = "WPF Exporter";

            services = new ServiceCollection();

            services.AddExporterFactory();
            services.AddAbstractFactory<IDataImporter, CsvImporter>();
            services.AddAbstractFactory<IRepository<User>, DataBase>();
            services.AddAbstractFactory<IDataFormatter,DataFormatter>();
            services.AddAbstractFactory<IMessage, Message>();
            services.AddAbstractFactory<IFileDialog, FileDialog>();
            services.AddAbstractFactory<IUser, User>();

            using (serviceProvider = services.BuildServiceProvider())
            {
                app.Run(window);
            }
                


        }
    }

}
