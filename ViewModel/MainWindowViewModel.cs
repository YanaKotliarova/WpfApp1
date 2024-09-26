using Microsoft.IdentityModel.Tokens;
using WpfApp1.Model;
using WpfApp1.MVVM;
using WpfApp1.View;

namespace WpfApp1.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private const string XmlExtension = ".xml";
        private const string ExcelExtension = ".xlsx";
        private const string Space = " ";
        private const string DefaultFileName = "DefaultFileName";

        User user = new User();
        CsvFile csvFile = new CsvFile();
        DataBase dataBase = new DataBase();
        ExcelFile excelFile = new ExcelFile();
        XmlFile xmlFile = new XmlFile();
        UIWorking uiWorking = new UIWorking();

        private string _fileExtension = "Выберите формат файла!";

        private string _mainTextBoxText = "Выберите, пожалуйста, файл!\t\t\t\t\t\t\t--------->\r\n(в формате csv)";
        public string MainText
        {
            get { return _mainTextBoxText; }
            set
            {
                _mainTextBoxText = value;
                OnPropertyChanged(nameof(MainText));
            }
        }

        private RelayCommand _readCsvFileCommandAndAddToBD;
        public RelayCommand ReadCsvFileCommandAndAddToBD
        {
            get
            {
                return _readCsvFileCommandAndAddToBD ??
                    (_readCsvFileCommandAndAddToBD = new RelayCommand(async obj =>
                    {
                        if (uiWorking.OpenFileDialog(out string fileName))
                        {
                            MainText = "Открытие файла и перенос данных могут занять некоторое время...";
                            await csvFile.ReadFromCsvFileAsync(fileName, user.ListOfUsersFromFile);
                            await dataBase.AddToDBAsync(user.ListOfUsersFromFile);
                            MainText = "Данные загружены в базу даных и готовы к экспорту.";
                        }
                    }
                    ));
            }
        }

        private RelayCommand _exportIntoFile;
        public RelayCommand ExportIntoFile
        {
            get
            {
                return _exportIntoFile ??
                    (_exportIntoFile = new RelayCommand(async obj =>
                    {
                        string newFileName = DefaultFileName;
                        if (!FileNameTextBox.IsNullOrEmpty())
                            newFileName = FileNameTextBox;

                        string date = dataBase.FormateDate(DatePicker);
                        string firstName = dataBase.FormateStringData(FirstNameTextBox);
                        string lastName = dataBase.FormateStringData(LastNameTextBox);
                        string patronymic = dataBase.FormateStringData(PatronymicTextBox);
                        string city = dataBase.FormateStringData(CityTextBox);
                        string country = dataBase.FormateStringData(CountryTextBox);

                        PersonStruct person = new PersonStruct(firstName, lastName, patronymic);
                        EntranceInfoStruct entranceInfo = new EntranceInfoStruct(date, city, country);


                        await CreateFileAsync(newFileName);

                        while (dataBase.amountOfViewedUsers <= dataBase.amoutOfUsersInDB)
                        {
                            if (dataBase.amountOfViewedUsers < 0) break;

                            user.ListOfUsersFromDB = await dataBase.GetFromDBAsync(person, entranceInfo, user.ListOfUsersFromDB);
                            user.ListOfUsersFromFile.AddRange(user.ListOfUsersFromDB);
                            await AddToFileAsync(newFileName);
                        }

                        OutputUsersToTextbox(user.ListOfUsersFromFile);

                        if (!(dataBase.amountOfViewedUsers < 0)) uiWorking.ShowMessage("Файл " + newFileName + _fileExtension + " создан!");
                        dataBase.amountOfViewedUsers = 0;
                    }
                    ));
            }
        }

        private RelayCommand _selectFileFormat;
        public RelayCommand SelectFileFormat
        {
            get
            {
                return _selectFileFormat ??
                    (_selectFileFormat = new RelayCommand(obj =>
                    {
                        if (obj.Equals(ExcelExtension))
                            _fileExtension = ExcelExtension;
                        else if (obj.Equals(XmlExtension))
                            _fileExtension = XmlExtension;
                    }
                    ));
            }
        }

        private async Task CreateFileAsync(string newFileName)
        {
            if (_fileExtension.Equals(ExcelExtension))
            {
                newFileName += ExcelExtension;
                await excelFile.CreateExcelFileAsync(newFileName);
            }
            else if (_fileExtension.Equals(XmlExtension))
            {
                newFileName += XmlExtension;
                await xmlFile.CreateXmlFileAsync(newFileName);
            }
            else
            {
                uiWorking.ShowMessage(_fileExtension);
                dataBase.amountOfViewedUsers = -1;
            }
        }

        private async Task AddToFileAsync(string newFileName)
        {
            if (_fileExtension.Equals(ExcelExtension))
            {
                newFileName += ExcelExtension;
                await excelFile.AddToExcelFileAsync(newFileName, user.ListOfUsersFromDB);
            }
            else if (_fileExtension.Equals(XmlExtension))
            {
                newFileName += XmlExtension;
                await xmlFile.AddToXmlFileAsync(newFileName, user.ListOfUsersFromDB);
            }
        }

        internal void OutputUsersToTextbox(List<User> ListOfUsersFromFile)
        {
            MainText = "Созданная выборка:\r\n";
            foreach (User u in ListOfUsersFromFile)
                MainText += u.Date.ToString() + Space + u.FirstName + Space + u.LastName + Space
                        + u.Patronymic + Space + u.City + Space + u.Country + "\r\n";
            ListOfUsersFromFile.Clear();
        }

        private void TextBoxDataValidating(string? textBoxData)
        {
            if ((!string.IsNullOrEmpty(textBoxData)) && (!textBoxData.All(Char.IsLetter)))
                throw new Exception("Возможен только буквенный ввод!");
        }

        private DateTime? _datePicker;
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
                    uiWorking.ShowMessage(ex.Message);
                }
            }
        }

        private string _lastNameTextBox;
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
                    uiWorking.ShowMessage(ex.Message);
                }
            }
        }

        private string _patronymicTextBox;
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
                    uiWorking.ShowMessage(ex.Message);
                }
            }
        }

        private string _cityTextBox;
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
                    uiWorking.ShowMessage(ex.Message);
                }
            }
        }

        private string _countryTextBox;
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
                    uiWorking.ShowMessage(ex.Message);
                }
            }
        }
    }
}
