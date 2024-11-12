using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.Model.Interfaces;
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
        private readonly IUsers _users;

        public ImportPageViewModel(IDataImporter importer, IRepository<User> repository, IFileDialog fileDialog,
            IMetroDialog metroDialog, IUsers users)
        {
            _importer = importer;
            _repository = repository;
            _fileDialog = fileDialog;
            _metroDialog = metroDialog;
            _users = users;
        }

        private string _importTextBoxText = "Выберите, пожалуйста, файл!\t\t\t\t\t\t\t--------->\r\n(в формате csv)";
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

                                ImportText = "Открытие файла и перенос данных могут занять некоторое время...";

                                await Task.Run(async () =>
                                {
                                    await foreach (List<User> list in _importer.ReadFromFileAsync(fileName))
                                    {
                                        _users.SetListOfUsersFromFile(list);
                                        await _repository.AddToDBAsync(_users.ReturnListOfUsersFromFile());
                                        _users.ReturnListOfUsersFromFile().Clear();
                                    }
                                });

                                await _metroDialog.MetroDialogMessage(this, "Данные успешно перенесены", "Можете перейти на страницу экспорта");

                                DisplayProgressBar = false;
                                IsImportAvailable = true;

                                ImportText = "Данные загружены в базу даных и готовы к экспорту.";
                            }
                        }
                        catch (Exception ex)
                        {
                            await _metroDialog.MetroDialogMessage(this, "Ошибка", ex.Message);
                            DisplayProgressBar = false;
                            IsImportAvailable = true;
                        }

                    }
                    ));
            }
        }

        private bool _displayProgressBar = false;
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

        private bool _isImportAvailable = true;
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
