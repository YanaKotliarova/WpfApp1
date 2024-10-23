using System.Windows.Controls;
using System.Windows.Navigation;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.MVVM;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.DependencyInjection;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class MenuPageViewModel : ViewModelBase
    {
        private readonly IAbstractFactory<IRepository<User>> _repositoryFactory;
        private readonly IAbstractFactory<IMessage> _messageFactory;

        public MenuPageViewModel(DependencyStruct dependencyStruct)
        {
            _repositoryFactory = dependencyStruct.Repository;
            _messageFactory = dependencyStruct.Message;
        }

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
                        try
                        {
                            NavigationButton ClickedButton = obj as NavigationButton;

                            NavigationService navigationService = NavigationService.GetNavigationService(ClickedButton);

                            navigationService.Navigate(ClickedButton!.NavigationUri);

                        }
                        catch (Exception ex)
                        {
                            var message = _messageFactory.Create();
                            message.ShowMessage(ex.Message);
                        }
                    }
                    ));
            }
        }

        private RelayCommand _initializeDBCommand;
        /// <summary>
        /// A command to validate the connection string and the availability 
        /// of DB and initialize it, if necessary.
        /// Called when the main window is loaded.
        /// </summary>
        public RelayCommand InitializeDBCommand
        {
            get
            {
                return _initializeDBCommand ??
                    (_initializeDBCommand = new RelayCommand(async obj =>
                    {
                        try
                        {
                            var db = _repositoryFactory.Create();
                            await db.InitializeDBAsync(db.ReturnConnectionStringValue());
                        }
                        catch(Exception ex)
                        {
                            var message = _messageFactory.Create();
                            message.ShowMessage(ex.Message);

                            Uri uri = new Uri("/View/Pages/EnterConnectionStringPage.xaml", UriKind.Relative);
                            Page page = obj as Page;

                            NavigationService navigationService = NavigationService.GetNavigationService(page);
                            navigationService.Navigate(uri);
                        }
                    }
                    ));
            }
        }


    }
}
