using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WpfApp1.Model.Interfaces;
using WpfApp1.ViewModel.DependencyInjection;
using WpfApp1.ViewModel.Factories.Interfaces;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1.View.Pages
{
    public partial class ViewingPage : Page
    {
        private IAbstractFactory<IUsers> _usersFactory =
            App.serviceProvider.GetService<IAbstractFactory<IUsers>>()!;

        public ViewingPage()
        {
            DependencyStruct dependencyStruct = new DependencyStruct(_usersFactory);

            InitializeComponent();
            DataContext = new ViewingPageViewModel(dependencyStruct);
        }
    }
}
