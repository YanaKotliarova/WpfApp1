using System.Windows.Controls;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1.View.Pages
{
    public partial class Import : Page
    {
        public Import()
        {
            InitializeComponent();
            DataContext = new ImportPageViewModel();
        }
    }
}
