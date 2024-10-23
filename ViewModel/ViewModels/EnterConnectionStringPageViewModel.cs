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
    class EnterConnectionStringPageViewModel : ViewModelBase
    {
        private readonly IAbstractFactory<IRepository<User>> _repositoryFactory;
        private readonly IAbstractFactory<IMessage> _messageFactory;

        public EnterConnectionStringPageViewModel(DependencyStruct dependencyStruct)
        {
            _repositoryFactory = dependencyStruct.Repository;
            _messageFactory = dependencyStruct.Message;
        }

        private RelayCommand _reinitializeDBCommand;
        /// <summary>
        /// A command to validate a new connection string and initialize DB.
        /// </summary>
        public RelayCommand ReinitializeDBCommand
        {
            get
            {
                return _reinitializeDBCommand ??
                    (_reinitializeDBCommand = new RelayCommand(async obj =>
                    {
                        var message = _messageFactory.Create();
                        try
                        {
                            string newConnectionString = NewConnectionStringTextBox;
                            var db = _repositoryFactory.Create();
                            await db.InitializeDBAsync(newConnectionString);

                            message.ShowMessage("Подключение установлено!");

                            Uri uri = new Uri("/View/Pages/MenuPage.xaml", UriKind.Relative);
                            Page page = obj as Page;

                            NavigationService navigationService = NavigationService.GetNavigationService(page);
                            navigationService.Navigate(uri);
                        }
                        catch (Exception ex)
                        {                            
                            message.ShowMessage(ex.Message + "\nПопробуйте снова.");
                        }
                    }
                    ));
            }
        }

        private string _newConnectionString;
        /// <summary>
        /// A property associated with a text field for entering a new connection string.
        /// </summary>
        public string NewConnectionStringTextBox
        {
            get { return _newConnectionString; }
            set
            {
                _newConnectionString = value;
                OnPropertyChanged(nameof(NewConnectionStringTextBox));
            }
        }
    }
}
