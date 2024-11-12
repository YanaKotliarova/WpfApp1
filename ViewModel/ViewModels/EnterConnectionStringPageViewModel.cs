using System.Windows.Controls;
using System.Windows.Navigation;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.MVVM;
using WpfApp1.View.UI.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class EnterConnectionStringPageViewModel : ViewModelBase
    {
        private const string MenuPageUri = "/View/Pages/MenuPage.xaml";

        private readonly IRepository<User> _repository;
        private readonly IMetroDialog _metroDialog;

        public EnterConnectionStringPageViewModel(IRepository<User> repository, IMetroDialog metroDialog)
        {
            _repository = repository;
            _metroDialog = metroDialog;
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
                        try
                        {
                            DisplayProgressBar = true;
                            DisplayEnterButton = false;
                            IsTextBoxAvailable = false;

                            await Task.Run(() => _repository.InitializeDBAsync(NewConnectionStringTextBox));

                            await _metroDialog.MetroDialogMessage(this, "Подключение установлено", "Подключение к БД завершено успешно");

                            Uri uri = new Uri(MenuPageUri, UriKind.Relative);
                            Page page = obj as Page;

                            NavigationService navigationService = NavigationService.GetNavigationService(page);
                            navigationService.Navigate(uri);
                        }
                        catch (Exception ex)
                        {
                            await _metroDialog.MetroDialogMessage(this, "Попробуйте снова.", ex.Message);
                            DisplayProgressBar = false;
                            DisplayEnterButton = true;
                            IsTextBoxAvailable = true;
                        }
                    }
                    ));
            }
        }

        private bool _isTextBoxAvailable = true;
        /// <summary>
        /// A property associated with an IsEnable property of text box.
        /// </summary>
        public bool IsTextBoxAvailable
        {
            get { return _isTextBoxAvailable; }
            set
            {
                _isTextBoxAvailable = value;
                OnPropertyChanged(nameof(IsTextBoxAvailable));
            }
        }

        private bool _displayProgressBar;
        /// <summary>
        /// A property associated with an IsVisible property of progress bar.
        /// </summary>
        public bool DisplayProgressBar
        {
            get { return _displayProgressBar; }
            set
            {
                _displayProgressBar = value;
                OnPropertyChanged(nameof(DisplayProgressBar));
            }
        }

        private bool _displayEnterButton = true;
        /// <summary>
        /// A property associated with an IsVisible property of button.
        /// </summary>
        public bool DisplayEnterButton
        {
            get { return _displayEnterButton; }
            set
            {
                _displayEnterButton = value;
                OnPropertyChanged(nameof(DisplayEnterButton));
            }
        }

        private string _newConnectionString;
        /// <summary>
        /// A property associated with a text box for entering a new connection string.
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
