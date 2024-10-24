﻿using Microsoft.IdentityModel.Tokens;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.Model.Interfaces;
using WpfApp1.MVVM;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.DependencyInjection;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class ExportPageViewModel : ViewModelBase
    {
        private const string XmlExtension = ".xml";
        private const string ExcelExtension = ".xlsx";

        private const string ExcelExporterName = "ExcelExporter";
        private const string XmlExporterName = "XmlExporter";

        private const string Space = " ";
        private const string DefaultFileName = "DefaultFileName";

        private string _fileExtension = "";

        private readonly IExporterFactory _exporterFactory;
        private readonly IAbstractFactory<IRepository<User>> _repositoryFactory;
        private readonly IAbstractFactory<IDataFormatter> _dataFormatterFactory;
        private readonly IAbstractFactory<IMessage> _messageFactory;
        private readonly IAbstractFactory<IUsers> _usersFactory;

        public ExportPageViewModel(DependencyStruct dependencyStruct)
        {
            _exporterFactory = dependencyStruct.Exporter;
            _repositoryFactory = dependencyStruct.Repository;
            _dataFormatterFactory = dependencyStruct.DataFormatter;
            _messageFactory = dependencyStruct.Message;
            _usersFactory = dependencyStruct.Users;
        }

        private string _exportTextBoxText = "Введите данные для выборки, выберите тип файла, а затем нажмите кнопку экспорта.";
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

        private RelayCommand _exportIntoFileCommand;
        /// <summary>
        /// The command associated with the data export button, 
        /// which collects the entered data from text fields, 
        /// creates structures with data about users to be searched in DB, 
        /// then creates a file of chosen format and 
        /// launches a gradual selection from DB and 
        /// writes users to this file. After creating the file, 
        /// the created selection is displayed in the main text field.
        /// </summary>
        public RelayCommand ExportIntoFileCommand
        {
            get
            {
                return _exportIntoFileCommand ??
                    (_exportIntoFileCommand = new RelayCommand(async obj =>
                    {
                        var db = _repositoryFactory.Create();
                        var formatter = _dataFormatterFactory.Create();
                        var message = _messageFactory.Create();
                        var users = _usersFactory.Create();

                        string newFileName = DefaultFileName;
                        if (!FileNameTextBox.IsNullOrEmpty())
                            newFileName = FileNameTextBox;

                        string date = formatter.FormateDate(DatePicker);
                        string firstName = formatter.FormateStringData(FirstNameTextBox);
                        string lastName = formatter.FormateStringData(LastNameTextBox);
                        string patronymic = formatter.FormateStringData(PatronymicTextBox);
                        string city = formatter.FormateStringData(CityTextBox);
                        string country = formatter.FormateStringData(CountryTextBox);

                        PersonStruct person = new PersonStruct(firstName, lastName, patronymic);
                        EntranceInfoStruct entranceInfo = new EntranceInfoStruct(date, city, country);

                        int amountOfUsersInDB = 0;
                        try
                        {
                            amountOfUsersInDB = await db.ReturnAmountOfUsersInDBAsync();

                            if (amountOfUsersInDB > 0)
                            {
                                await CreateFileAsync(newFileName);

                                while (db.ReturnAmountOfViewedUsers() <= amountOfUsersInDB)
                                {
                                    users.SetListOfUsersFromDB(await db.GetFromDBAsync(person, entranceInfo, users.ReturnListOfUsersFromDB()));

                                    users.ReturnListOfUsersFromFile().AddRange(users.ReturnListOfUsersFromDB());

                                    if (users.ReturnListOfUsersFromDB().Count > 0)
                                        await AddToFileAsync(newFileName);
                                }
                                message.ShowMessage("Файл " + newFileName + _fileExtension + " создан!");
                                db.SetAmountOfViewedUsers(0);
                                ExportText = $"Созданную выборку можно увидеть в созданном файле: {newFileName + _fileExtension} " +
                                $"либо на вкладке \"Просмотр\"";
                            }
                            else throw new Exception("Невозможно создать выборку: БД пуста!");
                        }
                        catch (Exception ex)
                        {
                            message.ShowMessage(ex.Message);
                        }


                    }
                    ));
            }
        }

        private RelayCommand _selectFileFormatCommand;
        /// <summary>
        /// A command associated with two RadioButton buttons to select the format of the saved file.
        /// </summary>
        public RelayCommand SelectFileFormatCommand
        {
            get
            {
                return _selectFileFormatCommand ??
                    (_selectFileFormatCommand = new RelayCommand(obj =>
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
        /// Asynchronous method for creating a file of the chosen format.
        /// </summary>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        private async Task CreateFileAsync(string newFileName)
        {
            if (_fileExtension.Equals(ExcelExtension))
            {
                var fileExporter = _exporterFactory.GetExporter(ExcelExporterName);
                newFileName += ExcelExtension;
                await fileExporter.CreateFileAsync(newFileName);
            }
            else if (_fileExtension.Equals(XmlExtension))
            {
                var fileExporter = _exporterFactory.GetExporter(XmlExporterName);
                newFileName += XmlExtension;
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
        /// <returns></returns>
        private async Task AddToFileAsync(string newFileName)
        {
            var users = _usersFactory.Create();

            if (_fileExtension.Equals(ExcelExtension))
            {
                var fileExporter = _exporterFactory.GetExporter(ExcelExporterName);
                newFileName += ExcelExtension;
                await fileExporter.AddToFileAsync(newFileName, users.ReturnListOfUsersFromDB());
            }
            else if (_fileExtension.Equals(XmlExtension))
            {
                var fileExporter = _exporterFactory.GetExporter(XmlExporterName);
                newFileName += XmlExtension;
                await fileExporter.AddToFileAsync(newFileName, users.ReturnListOfUsersFromDB());
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
                    var message = _messageFactory.Create();
                    message.ShowMessage(ex.Message);
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
                    var message = _messageFactory.Create();
                    message.ShowMessage(ex.Message);
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
                    var message = _messageFactory.Create();
                    message.ShowMessage(ex.Message);
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
                    var message = _messageFactory.Create();
                    message.ShowMessage(ex.Message);
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
                    var message = _messageFactory.Create();
                    message.ShowMessage(ex.Message);
                }
            }
        }
    }
}
