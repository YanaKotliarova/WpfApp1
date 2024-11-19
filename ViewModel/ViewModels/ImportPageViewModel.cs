using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.MVVM;
using WpfApp1.Services.Import;
using WpfApp1.View.UI.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class ImportPageViewModel : ViewModelBase
    {
        private readonly IDataImporter _importer;
        private readonly IRepository<User> _repository;
        private readonly IFileDialog _fileDialog;
        private readonly IMetroDialog _metroDialog;

        public ImportPageViewModel(IDataImporter importer, IRepository<User> repository, IFileDialog fileDialog,
            IMetroDialog metroDialog)
        {
            _importer = importer;
            _repository = repository;
            _fileDialog = fileDialog;
            _metroDialog = metroDialog;
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
                        DisplayProgressBar = !_repository.IsDBAvailable;
                        IsImportAvailable = _repository.IsDBAvailable;
                    }));
            }
        }

        private string _importTextBoxText = "Выберите, пожалуйста, файл для импорта.";
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
                            if (_fileDialog.OpenFileDialog(out string fileName))
                            {
                                IsImportAvailable = false;
                                DisplayProgressBar = true;
                                _repository.IsDBAvailable = false;

                                ImportText = "Открытие файла и перенос данных могут занять некоторое время...";

                                await Task.Run(async () =>
                                {
                                    await foreach (List<User> listOfUsers in _importer.ReadFromFileAsync(fileName))
                                    {
                                        await _repository.AddToDBAsync(listOfUsers);
                                        listOfUsers.Clear();
                                    }
                                });

                                DisplayProgressBar = false;
                                IsImportAvailable = true;
                                _repository.IsDBAvailable = true;

                                ImportText = "Данные загружены в базу даных и готовы к экспорту.";
                            }
                        }
                        catch (Exception ex)
                        {
                            await _metroDialog.ShowMessage(this, "Ошибка", ex.Message);
                            DisplayProgressBar = false;
                            IsImportAvailable = true;
                            _repository.IsDBAvailable = true;
                        }
                    }
                    ));
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

        private bool _isImportAvailable;
        /// <summary>
        /// A property associated with an IsEnable property of import button.
        /// </summary>
        public bool IsImportAvailable
        {
            get { return _isImportAvailable; }
            set
            {
                _isImportAvailable = value;
                OnPropertyChanged(nameof(IsImportAvailable));
            }
        }
    }
}
