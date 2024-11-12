using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        { 
            InitializeComponent();
            DataContext = App.serviceProvider.GetService<MainWindowViewModel>();
        }
    }
}