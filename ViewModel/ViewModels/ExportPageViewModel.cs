using System.IO;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.MVVM;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class ExportPageViewModel : ViewModelBase
    {
        private const string XmlExtension = ".xml";
        private const string ExcelExtension = ".xlsx";

        private const string ExcelExporterName = "ExcelExporter";
        private const string XmlExporterName = "XmlExporter";

        private const string DefaultExportTextBoxMessage = "Введите данные для выборки, выберите тип файла, а затем нажмите кнопку экспорта.";

        private readonly IExporterFactory _exporter;
        private readonly IRepository<User> _repository;
        private readonly IDataFormatter _dataFormatter;
        private readonly IFileDialog _fileDialog;
        private readonly IMetroDialog _metroDialog;

        public ExportPageViewModel(IExporterFactory exporter, IRepository<User> repository, IDataFormatter dataFormatter,
            IFileDialog fileDialog, IMetroDialog metroDialog)
        {
            _exporter = exporter;
            _repository = repository;
            _dataFormatter = dataFormatter;
            _fileDialog = fileDialog;
            _metroDialog = metroDialog;
        }

        private string _exportTextBoxText;
        /// <summary>
        /// A property associated with the text field used to display information.
        /// </summary>
        public string ExportText
        {
            get { return _exportTextBoxText; }
            set
            {
                _exportTextBoxText = value;
                OnPropertyChanged(nameof(ExportText));
            }
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
                        ExportText = DefaultExportTextBoxMessage;
                    }));
            }
        }

        private RelayCommand _exportIntoFileCommand;
        /// <summary>
        /// The command associated with the data export button, 
        /// which collects the entered data from text fields, 
        /// creates structures with data about users to be searched in DB, 
        /// then creates a file of chosen format, 
        /// launches a gradual selection from DB and 
        /// writes users to this file.
        /// </summary>
        public RelayCommand ExportIntoFileCommand
        {
            get
            {
                return _exportIntoFileCommand ??
                    (_exportIntoFileCommand = new RelayCommand(async obj =>
                    {
                        if (_repository.IsDBAvailable)
                        {
                            try
                            {
                                if (_fileDialog.SaveFileDialog(out string newFileName))
                                {
                                    ExportText = "Экспорт может занять некоторое время, пожалуйста подождите...";

                                    DisplayProgressBar = true;
                                    _repository.IsDBAvailable = false;

                                    string date = _dataFormatter.FormateDate(DatePicker);
                                    DatePicker = null;

                                    string firstName = _dataFormatter.FormateStringData(FirstNameTextBox);
                                    FirstNameTextBox = "";

                                    string lastName = _dataFormatter.FormateStringData(LastNameTextBox);
                                    LastNameTextBox = "";

                                    string patronymic = _dataFormatter.FormateStringData(PatronymicTextBox);
                                    PatronymicTextBox = "";

                                    string city = _dataFormatter.FormateStringData(CityTextBox);
                                    CityTextBox = "";

                                    string country = _dataFormatter.FormateStringData(CountryTextBox);
                                    CountryTextBox = "";

                                    PersonInfoStruct person = new PersonInfoStruct(firstName, lastName, patronymic);
                                    EntranceInfoStruct entranceInfo = new EntranceInfoStruct(date, city, country);

                                    await CreateFileAsync(newFileName);
                                    await Task.Run(async () =>
                                    {
                                        await foreach (List<User> listOfUsers in _repository.GetSelectionFromDBAsync(person, entranceInfo))
                                        {
                                            if (listOfUsers.Count > 0)
                                                await AddToFileAsync(newFileName, listOfUsers);

                                            listOfUsers.Clear();
                                        }
                                    });

                                    DisplayProgressBar = false;
                                    _repository.IsDBAvailable = true;

                                    ExportText = $"Созданную выборку можно увидеть в созданном файле: \"{newFileName}\" " +
                                    $"либо на вкладке \"Просмотр\"";
                                }
                            }
                            catch (Exception ex)
                            {
                                await _metroDialog.ShowMessage(this, "При создании файла произошла ошибка", ex.Message);
                                DisplayProgressBar = false;
                                _repository.IsDBAvailable = true;
                            }
                        }
                        else await _metroDialog.ShowMessage(this, "Пожалуйста, подождите!",
                            "Еще не закончилась предыдущая операция. Повторите попытку позже.");
                    }));
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

        /// <summary>
        /// Asynchronous method for creating a file of the chosen format.
        /// </summary>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        private async Task CreateFileAsync(string newFileName)
        {
            if (Path.GetExtension(newFileName).Equals(ExcelExtension))
            {
                var fileExporter = _exporter.GetExporter(ExcelExporterName);
                await fileExporter.CreateFileAsync(newFileName);
            }
            else if (Path.GetExtension(newFileName).Equals(XmlExtension))
            {
                var fileExporter = _exporter.GetExporter(XmlExporterName);
                await fileExporter.CreateFileAsync(newFileName);
            }
            else
            {
                throw new Exception("Выберите формат файла!");
            }

        }

        /// <summary>
        /// An asynchronous method for adding selected data to a file.
        /// </summary>
        /// <param name="newFileName"></param>
        /// <param name="listOfUsersFromDB"></param>
        /// <returns></returns>
        private async Task AddToFileAsync(string newFileName, List<User> listOfUsersFromDB)
        {
            if (Path.GetExtension(newFileName).Equals(ExcelExtension))
            {
                var fileExporter = _exporter.GetExporter(ExcelExporterName);
                await fileExporter.AddToFileAsync(newFileName, listOfUsersFromDB);
            }
            else if (Path.GetExtension(newFileName).Equals(XmlExtension))
            {
                var fileExporter = _exporter.GetExporter(XmlExporterName);
                await fileExporter.AddToFileAsync(newFileName, listOfUsersFromDB);
            }

        }

        /// <summary>
        /// A method for validating text fields that allows you to enter only letters.
        /// </summary>
        /// <param name="textBoxData"></param>
        /// <exception cref="Exception"></exception>
        private void TextBoxDataValidating(string? textBoxData)
        {
            if (!string.IsNullOrEmpty(textBoxData) && !textBoxData.All(char.IsLetter))
                throw new Exception("Возможен только буквенный ввод!");
        }

        private DateTime? _datePicker;
        /// <summary>
        /// A property associated with the DatePicker object for entering a date 
        /// to create a selection from DB.
        /// </summary>
        public DateTime? DatePicker
        {
            get { return _datePicker; }
            set
            {
                _datePicker = value;
                OnPropertyChanged(nameof(DatePicker));
            }
        }

        private string _fileNameTextBox;
        /// <summary>
        /// A property associated with a text field for entering a file name 
        /// to save a selection from DB.
        /// </summary>
        public string FileNameTextBox
        {
            get { return _fileNameTextBox; }
            set
            {
                _fileNameTextBox = value;
                OnPropertyChanged(nameof(FileNameTextBox));
            }
        }

        private string _firstNameTextBox;
        /// <summary>
        /// A property associated with a text field for entering a first name 
        /// to save a selection from DB.
        /// </summary>
        public string FirstNameTextBox
        {
            get { return _firstNameTextBox; }
            set
            {
                try
                {
                    TextBoxDataValidating(value);

                    _firstNameTextBox = value;
                    OnPropertyChanged(nameof(FirstNameTextBox));
                }
                catch (Exception ex)
                {
                    _metroDialog.ShowMessage(this, "Неверный ввод", ex.Message);
                }

            }
        }

        private string _lastNameTextBox;
        /// <summary>
        /// A property associated with a text field for entering a last name 
        /// to save a selection from DB.
        /// </summary>
        public string LastNameTextBox
        {
            get { return _lastNameTextBox; }
            set
            {
                try
                {
                    TextBoxDataValidating(value);

                    _lastNameTextBox = value;
                    OnPropertyChanged(nameof(LastNameTextBox));
                }
                catch (Exception ex)
                {
                    _metroDialog.ShowMessage(this, "Неверный ввод", ex.Message);
                }
            }
        }

        private string _patronymicTextBox;
        /// <summary>
        /// A property associated with a text field for entering a patronymic 
        /// to save a selection from DB.
        /// </summary>
        public string PatronymicTextBox
        {
            get { return _patronymicTextBox; }
            set
            {
                try
                {
                    TextBoxDataValidating(value);

                    _patronymicTextBox = value;
                    OnPropertyChanged(nameof(PatronymicTextBox));
                }
                catch (Exception ex)
                {
                    _metroDialog.ShowMessage(this, "Неверный ввод", ex.Message);
                }
            }
        }

        private string _cityTextBox;
        /// <summary>
        /// A property associated with a text field for entering a city
        /// to save a selection from DB.
        /// </summary>
        public string CityTextBox
        {
            get { return _cityTextBox; }
            set
            {
                try
                {
                    TextBoxDataValidating(value);

                    _cityTextBox = value;
                    OnPropertyChanged(nameof(CityTextBox));
                }
                catch (Exception ex)
                {
                    _metroDialog.ShowMessage(this, "Неверный ввод", ex.Message);
                }
            }
        }

        private string _countryTextBox;
        /// <summary>
        /// A property associated with a text field for entering a country 
        /// to save a selection from DB.
        /// </summary>
        public string CountryTextBox
        {
            get { return _countryTextBox; }
            set
            {
                try
                {
                    TextBoxDataValidating(value);

                    _countryTextBox = value;
                    OnPropertyChanged(nameof(CountryTextBox));
                }
                catch (Exception ex)
                {
                    _metroDialog.ShowMessage(this, "Неверный ввод", ex.Message);
                }
            }
        }
    }
}
