using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1.View.Pages
{
    public partial class ViewFilePage : Page
    {
        public ViewFilePage()
        {
            InitializeComponent();
            DataContext = App.serviceProvider.GetService<ViewFilePageViewModel>();
        }
    }
}
