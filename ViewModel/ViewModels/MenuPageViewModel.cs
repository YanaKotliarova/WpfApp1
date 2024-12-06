using System.Windows.Navigation;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.MVVM;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.Events;

namespace WpfApp1.ViewModel.ViewModels
{
    class MenuPageViewModel : ViewModelBase
    {      
        private readonly IRepository<User> _repository;
        private readonly IMetroDialog _metroDialog;
        private readonly IEventAggregator _eventAggregator;

        public MenuPageViewModel(IRepository<User> repository, IMetroDialog metroDialog, IEventAggregator eventAggregator)
        {
            _repository = repository;
            _metroDialog = metroDialog;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<ExportingAvailability>().Subscribe(state => { IsExportAvailable = !state; });

            _eventAggregator.GetEvent<ViewingAvailability>().Subscribe(state => { IsViewingAvailable = state; });
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
                            var viewModel = _metroDialog.ReturnViewModel();
                            await _metroDialog.ShowMessage(viewModel, "Ошибка при переходе на страницу", ex.Message);
                        }
                    }));
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
                    (_pageIsLoadedCommand = new RelayCommand(async obj =>
                    {
                        try
                        {
                            IsExportAvailable = !_repository.IsDBEmpty;
                        }
                        catch (Exception ex) 
                        {
                            var viewModel = _metroDialog.ReturnViewModel();
                            await _metroDialog.ShowMessage(viewModel, "Возникла непредвиденная ошибка", ex.Message);
                        }
                    }
                    ));
            }
        }

        private bool _isViewingAvailable;
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
