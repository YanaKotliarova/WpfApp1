using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.MVVM;
using WpfApp1.Properties;
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

            _eventAggregator.GetEvent<ExportingAvailabilityEvent>().Subscribe(state => { IsExportAvailable = !state; });

            _eventAggregator.GetEvent<ViewingAvailabilityEvent>().Subscribe(state => { IsViewingAvailable = state; });

            GetLanguages();
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
                            await _metroDialog.ShowMessage(Properties.Resources.HeaderOpenPageEx, ex.Message);
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
                            await _metroDialog.ShowMessage(Properties.Resources.HeaderUnexpectedEx, ex.Message);
                        }
                    }
                    ));
            }
        }

        /// <summary>
        /// The method for adding available languages into created collection.
        /// </summary>
        private void GetLanguages()
        {
            ResourceManager rm = new ResourceManager(typeof(Resources));
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            foreach (CultureInfo culture in cultures)
            {
                ResourceSet rs = rm.GetResourceSet(culture, true, false);

                if (rs != null)
                {
                    if (culture.Equals(CultureInfo.InvariantCulture))
                    {
                        Languages.Add(new CultureInfo("ru"));
                    }
                    else
                    {
                        Languages.Add(culture);
                    }
                }
            }           

            Language = SetLanguageChanger();
        }

        /// <summary>
        /// The method for setting default value of ComboBox with available languages.
        /// </summary>
        /// <returns></returns>
        private CultureInfo SetLanguageChanger()
        {
            CultureInfo currentCultureInfo;
            CultureInfo cultureInfo = new CultureInfo(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);

            if (Languages.Contains(cultureInfo))
                currentCultureInfo = new CultureInfo(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
            else currentCultureInfo = new CultureInfo("ru");

            return currentCultureInfo;
        }


        private ObservableCollection<CultureInfo> _languages = new ObservableCollection<CultureInfo>();
        /// <summary>
        /// A property for creating collection of languages that the application has been translated into.
        /// </summary>
        public ObservableCollection<CultureInfo> Languages
        {
            get { return _languages; }
            set
            {
                _languages = value;
                OnPropertyChanged(nameof(Languages));
            }
        }

        private CultureInfo _language;
        /// <summary>
        /// A property associated with SelectedItem property of ComboBox for choosing language.
        /// Changes the language of the application according to the selected item of ComboBox.
        /// </summary>
        public CultureInfo Language
        {
            get { return _language; }
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));

                Thread.CurrentThread.CurrentUICulture = value;

                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                Frame frame = window.FindName("MainFrame") as Frame;
                frame.Refresh();
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
