using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.Model.Interfaces;
using WpfApp1.MVVM;
using WpfApp1.Services.Import;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.DependencyInjection;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class ImportPageViewModel : ViewModelBase
    {
        private readonly IAbstractFactory<IDataImporter> _importerFactory;
        private readonly IAbstractFactory<IRepository<User>> _repositoryFactory;
        private readonly IAbstractFactory<IFileDialog> _fileDialogFactory;
        private readonly IAbstractFactory<IMessage> _messageFactory;
        private readonly IAbstractFactory<IUsers> _usersFactory;

        public ImportPageViewModel(DependencyStruct dependencyStruct)
        {
            _importerFactory = dependencyStruct.Importer;
            _repositoryFactory = dependencyStruct.Repository;
            _fileDialogFactory = dependencyStruct.FileDialog;
            _messageFactory = dependencyStruct.Message;
            _usersFactory = dependencyStruct.Users;
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
                            var fileDialog = _fileDialogFactory.Create();
                            var users = _usersFactory.Create();

                            if (fileDialog.OpenFileDialog(out string fileName))
                            {
                                var db = _repositoryFactory.Create();

                                var fileImporter = _importerFactory.Create();
                                ImportText = "Открытие файла и перенос данных могут занять некоторое время...";
                                await fileImporter.ReadFromFileAsync(fileName, users.ReturnListOfUsersFromFile());

                                await db.AddToDBAsync(users.ReturnListOfUsersFromFile());

                                ImportText = "Данные загружены в базу даных и готовы к экспорту.";
                            }
                        }
                        catch (Exception ex)
                        {
                            var message = _messageFactory.Create();
                            message.ShowMessage(ex.Message);
                        }

                    }
                    ));
            }
        }
    }
}
