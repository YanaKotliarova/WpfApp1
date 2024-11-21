using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1.View.Pages
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class ExportPage : Page
    {
        public ExportPage()
        {            
            InitializeComponent();
            DataContext = App.serviceProvider.GetService<ExportPageViewModel>();
        }
    }
}
