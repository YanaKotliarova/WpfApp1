using System.Windows.Navigation;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.MVVM;
using WpfApp1.View.UI.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class MenuPageViewModel : ViewModelBase
    {      
        private readonly IRepository<User> _repository;
        private readonly IMetroDialog _metroDialog;

        public MenuPageViewModel(IRepository<User> repository, IMetroDialog metroDialog)
        {
            _repository = repository;
            _metroDialog = metroDialog;
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
                            await _metroDialog.ShowMessage(this, "Ошибка при переходе на страницу", ex.Message);
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
                        IsExportAvailable = !_repository.IsDBEmpty;
                        if (_repository.PersonInfo != null && _repository.EntranceInfo != null && _repository.IsDBAvailable)
                            IsViewingAvailable = true;
                    }
                    ));
            }
        }

        private bool _isViewingAvailable = false;
        /// <summary>
        /// A property associated with an IsEnable property of View button.
        /// </summary>
        public bool IsViewingAvailable
        {
            get { return _isViewingAvailable; }
            set
            {
                _isViewingAvailable = value;
                OnPropertyChanged(nameof(IsViewingAvailable));
            }
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
