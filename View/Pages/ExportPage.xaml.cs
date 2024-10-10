using System.Windows.Controls;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1.View.Pages
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class Export : Page
    {
        public Export()
        {
            InitializeComponent();
            DataContext = new ExportPageViewModel();
        }
    }
}
