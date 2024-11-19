using System.Windows.Controls;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.MVVM;
using WpfApp1.View.UI.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class MainWindowViewModel
    {
        private const string EnterConnectionStringPageUri = "/View/Pages/EnterConnectionStringPage.xaml";
        private const string MenuPageUri = "/View/Pages/MenuPage.xaml";

        private readonly IRepository<User> _repository;
        private readonly IMetroDialog _metroDialog;

        public MainWindowViewModel(IRepository<User> repository, IMetroDialog metroDialog)
        {
            _repository = repository;
            _metroDialog = metroDialog;
        }

        private RelayCommand _initializeDBCommand;
        /// <summary>
        /// A command to validate the connection string and the availability 
        /// of DB and initialize it, if necessary.
        /// Called when the main window is loaded.
        /// </summary>
        public RelayCommand InitializeDBCommand
        {
            get
            {
                return _initializeDBCommand ??
                    (_initializeDBCommand = new RelayCommand(async obj =>
                    {
                        Uri uri = null;

                        await Task.Run(async () =>
                        {
                            try
                            {
                                await _repository.InitializeDBAsync(_repository.ReturnConnectionStringValue());
                                uri = new Uri(MenuPageUri, UriKind.Relative);
                            }
                            catch (Exception ex)
                            {
                                await _metroDialog.ShowMessage(this, "При подключении возникла ошибка", ex.Message);
                                uri = new Uri(EnterConnectionStringPageUri, UriKind.Relative);
                            }
                        });

                        Frame frame = obj as Frame;
                        frame.Navigate(uri);
                    }));
            }
        }
    }
}