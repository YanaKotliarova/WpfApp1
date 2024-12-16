using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.MVVM;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.Events;
using WpfApp1.ViewModel.Factories;

namespace WpfApp1.ViewModel.ViewModels
{
    class ImportPageViewModel : ViewModelBase
    {
        private const string CsvImporterName = "CsvImporter";

        private readonly IImporterFactory _importer;
        private readonly IRepository<User> _repository;
        private readonly IFileDialog _fileDialog;
        private readonly IMetroDialog _metroDialog;
        private readonly IEventAggregator _eventAggregator;

        public ImportPageViewModel(IImporterFactory importer, IRepository<User> repository, IFileDialog fileDialog,
            IMetroDialog metroDialog, IEventAggregator eventAggregator)
        {
            _importer = importer;
            _repository = repository;
            _fileDialog = fileDialog;
            _metroDialog = metroDialog;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<ImportExportAvailabilityEvent>().Subscribe((state) =>
            {
                IsProgressBarVisible = !state;
            });
        }

        private RelayCommand _pageIsLoadedCommand;
        /// <summary>
        /// The command which is called when page is loaded and change text of main text box on export page.
        /// </summary>
        public RelayCommand PageIsLoadedCommand
        {
            get
            {
                return _pageIsLoadedCommand ??
                    (_pageIsLoadedCommand = new RelayCommand(obj =>
                    {
                        if (_repository.IsDBAvailable)
                            ImportText = Properties.Resources.DefaultImportTextBoxMessage;
                    }));
            }
        }

        private RelayCommand _readCsvFileAndAddToBDCommand;
        /// <summary>
        ///The command associated with the button to open the csv file. 
        ///When file is open, data from it is read and written to DB.
        /// </summary>
        public RelayCommand ReadCsvFileAndAddToBDCommand
        {
            get
            {
                return _readCsvFileAndAddToBDCommand ??
                    (_readCsvFileAndAddToBDCommand = new RelayCommand(async obj =>
                    {
                        try
                        {
                            if (_fileDialog.OpenFileDialog(out string fileName, Properties.Resources.OpenCsvFileExtensionFilter))
                            {
                                _repository.IsDBAvailable = false;

                                ImportText = Properties.Resources.AwaitImport;

                                var fileImporter = _importer.GetImporter(CsvImporterName);

                                await foreach (List<User> listOfUsers in fileImporter.ReadFromFileAsync(fileName))
                                {
                                    await _repository.AddToDBAsync(listOfUsers);
                                }

                                _repository.IsDBAvailable = true;

                                await _metroDialog.ShowMessage(Properties.Resources.HeaderImportCompleted,
                                    Properties.Resources.DataImported);

                                ImportText = Properties.Resources.DataImported;
                            }
                        }
                        catch (Exception ex)
                        {
                            await _metroDialog.ShowMessage(Properties.Resources.HeaderImportEx, ex.Message);
                            _repository.IsDBAvailable = true;
                        }
                    }, x => _repository.IsDBAvailable));
            }
        }

        private string _importTextBoxText;
        /// <summary>
        /// A property associated with the text field used to display information.
        /// </summary>
        public string ImportText
        {
            get { return _importTextBoxText; }
            set
            {
                _importTextBoxText = value;
                OnPropertyChanged(nameof(ImportText));
            }
        }

        private bool _isProgressBarVisible;
        /// <summary>
        /// A property associated with an IsVisible property of progress bar.
        /// </summary>
        public bool IsProgressBarVisible
        {
            get { return _isProgressBarVisible; }
            set
            {
                _isProgressBarVisible = value;
                OnPropertyChanged(nameof(IsProgressBarVisible));
            }
        }
    }
}
