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
        /// <summary>
        /// Свойство, связанное с главным текстовым полем, используемым для вывода информации.
        /// </summary>
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
        /// <summary>
        /// Команда, связанная с кнопкой открытия csv файла. 
        /// При открытии файла данные из него считываются и записываются в БД.
        /// </summary>
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
        /// <summary>
        /// Команда, связанная с кнопкой экспорта данных, которая собирает введённые данные из текстовых полей,
        /// создаёт структуры с данными о пользователях, которых нужно искать в БД, после чего создаёт файл нужного формата и
        /// запускает постепенные выборку из БД и запись пользователей в этот файл. После создания файла созданная выборка 
        /// выводится в главное текстовое поле.
        /// </summary>
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
        /// <summary>
        /// Команда, связанная с двумя кнопками типа RadioButton, для выбора формата сохраняемого файла.
        /// </summary>
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

        /// <summary>
        /// Асинхронный метод создания файла нужного формата.
        /// </summary>
        /// <param name="newFileName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Асинхронный метод добавления данных выборки в файл.
        /// </summary>
        /// <param name="newFileName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Метод вывода созданной выборки в главное текстовое поле.
        /// </summary>
        /// <param name="ListOfUsersFromFile"></param>
        internal void OutputUsersToTextbox(List<User> ListOfUsersFromFile)
        {
            MainText = "Созданная выборка:\r\n";
            foreach (User u in ListOfUsersFromFile)
                MainText += u.Date.ToString() + Space + u.FirstName + Space + u.LastName + Space
                        + u.Patronymic + Space + u.City + Space + u.Country + "\r\n";
            ListOfUsersFromFile.Clear();
        }

        /// <summary>
        /// Метод валидации текстовых полей, допускающий ввод только букв.
        /// </summary>
        /// <param name="textBoxData"></param>
        /// <exception cref="Exception"></exception>
        private void TextBoxDataValidating(string? textBoxData)
        {
            if ((!string.IsNullOrEmpty(textBoxData)) && (!textBoxData.All(Char.IsLetter)))
                throw new Exception("Возможен только буквенный ввод!");
        }

        private DateTime? _datePicker;
        /// <summary>
        /// Свойство, связанное с объектом DatePicker для ввода даты для создания выборки из БД.
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
        /// Свойство, связанное с текстовым полем для ввода имени файла для сохранения выборки из БД.
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
        /// Свойство, связанное с текстовым полем для ввода имени для создания выборки из БД.
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
                    uiWorking.ShowMessage(ex.Message);
                }
            }
        }

        private string _lastNameTextBox;
        /// <summary>
        /// Свойство, связанное с текстовым полем для ввода фамилии для создания выборки из БД.
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
                    uiWorking.ShowMessage(ex.Message);
                }
            }
        }

        private string _patronymicTextBox;
        /// <summary>
        /// Свойство, связанное с текстовым полем для ввода отчества для создания выборки из БД.
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
                    uiWorking.ShowMessage(ex.Message);
                }
            }
        }

        private string _cityTextBox;
        /// <summary>
        /// Свойство, связанное с текстовым полем для ввода города для создания выборки из БД.
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
                    uiWorking.ShowMessage(ex.Message);
                }
            }
        }

        private string _countryTextBox;
        /// <summary>
        /// Свойство, связанное с текстовым полем для ввода страны для создания выборки из БД.
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
                    uiWorking.ShowMessage(ex.Message);
                }
            }
        }
    }
}
