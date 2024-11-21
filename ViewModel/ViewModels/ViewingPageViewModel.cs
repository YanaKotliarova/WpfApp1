using System.Windows.Controls;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.MVVM;

namespace WpfApp1.ViewModel.ViewModels
{
    class ViewingPageViewModel : ViewModelBase
    {
        private readonly IRepository<User> _repository;
        public ViewingPageViewModel(IRepository<User> repository)
        {
            _repository = repository;
        }

        private string _viewTextBoxText;
        /// <summary>
        /// A property associated with the text field used to display information.
        /// </summary>
        public string ViewText
        {
            get { return _viewTextBoxText; }
            set
            {
                _viewTextBoxText = value;
                OnPropertyChanged(nameof(ViewText));
            }
        }

        private RelayCommand _showSelectionCommand;
        /// <summary>
        /// The command associated with the button to show a selection from DB.
        /// </summary>
        public RelayCommand ShowSelectionCommand
        {
            get
            {
                return _showSelectionCommand ??
                    (_showSelectionCommand = new RelayCommand(async obj =>
                    {
                        List<User> listForViewing = new List<User>();
                        await Task.Run(async () =>
                        {
                            await foreach (List<User> listOfUsers in _repository.GetSelectionFromDBAsync(_repository.PersonInfo!.Value, _repository.EntranceInfo!.Value))
                            {
                                listForViewing.AddRange(listOfUsers);
                            }
                        });
                        ListView listView = obj as ListView;
                        listView.ItemsSource = listForViewing;
                    }
                    ));
            }
        }
    }
}
