using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfApp1.Data.Database;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.View.UI;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.DependencyInjection;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1
{
    public partial class App : Application
    {
        internal static ServiceProvider serviceProvider;

        public ResourceDictionary MetroResources
        {
            get
            {
                ResourceDictionary controls = new ResourceDictionary();
                controls.Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml", UriKind.RelativeOrAbsolute);
                ResourceDictionary fonts = new ResourceDictionary();
                fonts.Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml", UriKind.RelativeOrAbsolute);
                ResourceDictionary themes = new ResourceDictionary();
                themes.Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.Purple.xaml", UriKind.RelativeOrAbsolute);

                ResourceDictionary generalResource = new ResourceDictionary();

                generalResource.MergedDictionaries.Add(controls);
                generalResource.MergedDictionaries.Add(fonts);
                generalResource.MergedDictionaries.Add(themes);

                return generalResource;
            }
        }

        [STAThread]
        public static void Main()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<ExportPageViewModel>();
            services.AddSingleton<ImportPageViewModel>();
            services.AddSingleton<ViewSelectionPageViewModel>();
            services.AddSingleton<ViewFilePageViewModel>();
            services.AddSingleton<MenuPageViewModel>();
            services.AddSingleton<EnterConnectionStringPageViewModel>();

            services.AddSingleton<IEventAggregator, EventAggregator>();

            services.AddExporterFactory();
            services.AddImporterFactory();
            services.AddScoped<IRepository<User>, DataBase>();
            services.AddTransient<IDataFormatter, DataFormatter>();
            services.AddTransient<IConnectionStringValidation, ConnectionStringValidation>();
            services.AddTransient<IFileDialog, FileDialog>();
            services.AddTransient<IDialogCoordinator, DialogCoordinator>();
            services.AddTransient<IMetroDialog, MetroDialog>();

            using (serviceProvider = services.BuildServiceProvider())
            {
                App app = new App();
                app.ShutdownMode = ShutdownMode.OnLastWindowClose;
                MainWindow window = new MainWindow();
                window.Title = "CSV EXPORTER";
                Current.Resources = app.MetroResources;
                app.Run(window);
            }
        }
    }
}
