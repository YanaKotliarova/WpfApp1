using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1
{
    public partial class App : Application
    {
        [STAThread]
        static void Main()
        {
            App app = new App();
            MainWindow window = new MainWindow();
            window.Title = "WPF Exporter";

            using (MainWindowViewModel.serviceProvider = MainWindowViewModel.services.BuildServiceProvider())
            {
                app.Run(window);
            }                                  

        }
    }
}
