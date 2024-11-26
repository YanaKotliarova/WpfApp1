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
                        if (_repository.IsDBAvailable)
                        {
                            try
                            {
                                if (_fileDialog.OpenFileDialog(out string fileName))
                                {
                                    DisplayProgressBar = true;
                                    _repository.IsDBAvailable = false;

                                    IsButtonEnable = false;

                                    ImportText = "Открытие файла и перенос данных могут занять некоторое время...";

                                    await foreach (List<User> listOfUsers in _importer.ReadFromFileAsync(fileName))
                                    {
                                        await _repository.AddToDBAsync(listOfUsers);
                                        listOfUsers.Clear();
                                    }

                                    DisplayProgressBar = false;
                                    _repository.IsDBAvailable = true;

                                    IsButtonEnable = true;

                                    var viewModel = _metroDialog.ReturnViewModel();
                                    await _metroDialog.ShowMessage(viewModel, "Импорт завершен!",
                                        "Данные загружены в базу даных и готовы к экспорту.");

                                    ImportText = "Данные загружены в базу даных и готовы к экспорту.";
                                }
                            }
                            catch (Exception ex)
                            {
                                var viewModel = _metroDialog.ReturnViewModel();
                                await _metroDialog.ShowMessage(viewModel, "Ошибка", ex.Message);
                                DisplayProgressBar = false;
                                _repository.IsDBAvailable = true;

                                IsButtonEnable = true;
                            }
                        }
                        else
                        {
                            await _metroDialog.ShowMessage(this, "Пожалуйста, подождите!",
                            "Еще не закончилась предыдущая операция. Повторите попытку позже.");
                        }
                    }
                    ));
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

        private bool _isButtonEnable = true;
        /// <summary>
        /// A property associated with an IsEnable property of import button.
        /// </summary>
        public bool IsButtonEnable
        {
            get { return _isButtonEnable; }
            set
            {
                _isButtonEnable = value;
                OnPropertyChanged(nameof(IsButtonEnable));
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
    }
}
