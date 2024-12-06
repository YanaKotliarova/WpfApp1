using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1.View.Pages
{
    public partial class ViewSelectionPage : Page
    {
        public ViewSelectionPage()
        {
            InitializeComponent();
            DataContext = App.serviceProvider.GetService<ViewSelectionPageViewModel>();
        }
    }
}
