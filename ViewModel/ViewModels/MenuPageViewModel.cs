using Microsoft.Extensions.DependencyInjection;
using System.Windows.Navigation;
using WpfApp1.MVVM;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class MenuPageViewModel : ViewModelBase
    {
        private RelayCommand _openPageCommand;
        /// <summary>
        /// The command associated with created navigation buttons to open program pages.
        /// </summary>
        public RelayCommand OpenPageCommand
        {
            get
            {
                return _openPageCommand ??
                    (_openPageCommand = new RelayCommand(obj =>
                    {
                        var message = App.serviceProvider.GetService<IAbstractFactory<IMessage>>()!.Create();
                        try
                        {
                            NavigationButton ClickedButton = obj as NavigationButton;

                            NavigationService navigationService = NavigationService.GetNavigationService(ClickedButton);

                            navigationService.Navigate(ClickedButton.NavigationUri);

                        }
                        catch (Exception ex)
                        {
                            message.ShowMessage(ex.Message);
                        }
                    }
                    ));
            }
        }
    }
}
