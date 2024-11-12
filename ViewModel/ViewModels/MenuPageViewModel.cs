using Microsoft.IdentityModel.Tokens;
using System.Windows.Navigation;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.Model.Interfaces;
using WpfApp1.MVVM;
using WpfApp1.View.UI.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class MenuPageViewModel : ViewModelBase
    {      
        private readonly IRepository<User> _repository;
        private readonly IMetroDialog _metroDialog;
        private readonly IUsers _users;

        public MenuPageViewModel(IRepository<User> repository, IMetroDialog metroDialog, IUsers users)
        {
            _repository = repository;
            _metroDialog = metroDialog;
            _users = users;
        }

        private int _amountOfUsersInDB = 0;

        private RelayCommand _openPageCommand;
        /// <summary>
        /// The command associated with created navigation buttons to open program pages.
        /// </summary>
        public RelayCommand OpenPageCommand
        {
            get
            {
                return _openPageCommand ??
                    (_openPageCommand = new RelayCommand(async obj =>
                    {
                        try
                        {
                            NavigationButton ClickedButton = obj as NavigationButton;

                            NavigationService navigationService = NavigationService.GetNavigationService(ClickedButton);

                            navigationService.Navigate(ClickedButton!.NavigationUri);

                        }
                        catch (Exception ex)
                        {
                            await _metroDialog.MetroDialogMessage(this, "Ошибка при переходе на страницу", ex.Message);
                        }
                    }
                    ));
            }
        }

        private RelayCommand _pageIsLoadedCommand;
        /// <summary>
        /// The command which is called when page is loaded and change value of IsExportAvailable property.
        /// </summary>
        public RelayCommand PageIsLoadedCommand
        {
            get
            {
                return _pageIsLoadedCommand ??
                    (_pageIsLoadedCommand = new RelayCommand(obj =>
                    {
                        IsExportAvailable = !_repository.ReturnIsDBEmpty();
                    }
                    ));
            }
        }

        /// <summary>
        /// A property associated with an IsEnable property of View button.
        /// </summary>
        public bool IsViewingAvailable
        {
            get { return !_users.ReturnListOfUsersForView().IsNullOrEmpty(); }
        }

        private bool _isExportAvailable;
        /// <summary>
        /// A property associated with an IsEnable property of Export button.
        /// </summary>
        public bool IsExportAvailable
        {
            get { return _isExportAvailable; }
            set
            {
                _isExportAvailable = value;
                OnPropertyChanged(nameof(IsExportAvailable));
            }
        }
    }
}
