using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Model.Database.Interfaces;
using WpfApp1.Model.Import;
using WpfApp1.Model.MainModel;
using WpfApp1.Model.MainModel.Interfaces;
using WpfApp1.MVVM;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class ImportPageViewModel : ViewModelBase
    {
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

                        var message = MainWindowViewModel.serviceProvider.GetService<IAbstractFactory<IMessage>>()!.Create();
                        try
                        {
                            var fileDialog = MainWindowViewModel.serviceProvider.GetService<IAbstractFactory<IFileDialog>>()!.Create();
                            var user = MainWindowViewModel.serviceProvider.GetService<IAbstractFactory<IUser>>()!.Create();

                            if (fileDialog.OpenFileDialog(out string fileName))
                            {
                                var db = MainWindowViewModel.serviceProvider.GetService<IAbstractFactory<IRepository<User>>>()!.Create();

                                var fileImporter = MainWindowViewModel.serviceProvider.GetService<IAbstractFactory<IDataImporter>>()!.Create();
                                ImportText = "Открытие файла и перенос данных могут занять некоторое время...";
                                await fileImporter.ReadFromFileAsync(fileName, user.ReturnListOfUsersFromFile());

                                await db.AddToDBAsync(user.ReturnListOfUsersFromFile());

                                ImportText = "Данные загружены в базу даных и готовы к экспорту.";
                            }
                        }
                        catch (Exception ex)
                        {
                            message.ShowMessage(ex.Message);
                        }

                    }
                    ));
            }
        }
    }
}
