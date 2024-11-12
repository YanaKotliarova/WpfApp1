using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1.View.Pages
{
    /// <summary>
    /// Interaction logic for EnterConnectionStringPage.xaml
    /// </summary>
    public partial class EnterConnectionStringPage: Page
    {
        public EnterConnectionStringPage()
        {
            InitializeComponent();
            DataContext = App.serviceProvider.GetService<EnterConnectionStringPageViewModel>();
        }
    }
}
