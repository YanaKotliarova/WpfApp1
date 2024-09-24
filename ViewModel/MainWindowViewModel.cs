using Microsoft.IdentityModel.Tokens;
using System;
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
                            user.ListOfUsersFromDB = await dataBase.GetFromDBAsync(person, entranceInfo, user.ListOfUsersFromDB);

                            OutputUsersToTextbox(user.ListOfUsersFromDB);

                            await AddToFileAsync(newFileName);
                        }
                        uiWorking.ShowMessage("Файл " + newFileName + _fileExtension + " создан!");
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
            else
            {
                uiWorking.ShowMessage(_fileExtension);
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

        internal void OutputUsersToTextbox(List<User> ListOfUsersFromDB)
        {
            MainText = "Созданная выборка:\r\n";
            foreach (User u in ListOfUsersFromDB)
                MainText += u.Date.ToString() + Space + u.FirstName + Space + u.LastName + Space
                        + u.Patronymic + Space + u.City + Space + u.Country + "\r\n";
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
                _firstNameTextBox = value;
                OnPropertyChanged(nameof(FirstNameTextBox));
            }
        }

        private string _lastNameTextBox;
        public string LastNameTextBox
        {
            get { return _lastNameTextBox; }
            set
            {
                _lastNameTextBox = value;
                OnPropertyChanged(nameof(LastNameTextBox));
            }
        }

        private string _patronymicTextBox;
        public string PatronymicTextBox
        {
            get { return _patronymicTextBox; }
            set
            {
                _patronymicTextBox = value;
                OnPropertyChanged(nameof(PatronymicTextBox));
            }
        }

        private string _cityTextBox;
        public string CityTextBox
        {
            get { return _cityTextBox; }
            set
            {
                _cityTextBox = value;
                OnPropertyChanged(nameof(CityTextBox));
            }
        }

        private string _countryTextBox;
        public string CountryTextBox
        {
            get { return _countryTextBox; }
            set
            {
                _countryTextBox = value;
                OnPropertyChanged(nameof(CountryTextBox));
            }
        }
    }
}
