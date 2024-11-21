using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1.View.Pages
{
    public partial class ImportPage : Page
    {        
        public ImportPage()
        {            
            InitializeComponent();
            DataContext = App.serviceProvider.GetService<ImportPageViewModel>();
        }
    }
}
