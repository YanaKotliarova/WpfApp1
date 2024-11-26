using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.MVVM;

namespace WpfApp1.ViewModel.ViewModels
{
    class ViewingPageViewModel : ViewModelBase
    {
        private const int AmoutOfUsersPerPage = 10;

        private readonly IRepository<User> _repository;
        public ViewingPageViewModel(IRepository<User> repository)
        {
            _repository = repository;
        }
        public List<User> ListOfUsersForViewing { get; private set; } = new List<User>();

        private ICollectionView _usersCollection;
        public ICollectionView UsersCollection
        {
            get { return _usersCollection; }
            set
            {
                _usersCollection = value;
                OnPropertyChanged(nameof(UsersCollection));
            }
        }

        private ObservableCollection<User> _listOfUsers = new ObservableCollection<User>();
        public ObservableCollection<User> ListOfUsers
        {
            get { return _listOfUsers; }
            set
            {
                _listOfUsers = value;
                OnPropertyChanged(nameof(ListOfUsers));
            }
        }

        private RelayCommand _closeViewingPageCommand;
        /// <summary>
        /// The command associated with the button to show a selection from DB.
        /// </summary>
        public RelayCommand CloseViewingPageCommand
        {
            get
            {
                return _closeViewingPageCommand ??
                    (_closeViewingPageCommand = new RelayCommand(async obj =>
                    {
                        ListOfUsersForViewing.Clear();
                        ListOfUsers.Clear();
                    }
                    ));
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
                        UsersPerPage = AmoutOfUsersPerPage;
                        UsersCollection = CollectionViewSource.GetDefaultView(ListOfUsers);

                        await Task.Run(async () =>
                        {
                            await foreach (List<User> listOfUsers in _repository.GetSelectionFromDBAsync(_repository.PersonInfo!.Value, _repository.EntranceInfo!.Value))
                            {
                                ListOfUsersForViewing.AddRange(listOfUsers);
                            }
                        });

                        UpdateCollection(ListOfUsersForViewing.Take(UsersPerPage));
                        UpdateUsersCount();
                    }
                    ));
            }
        }

        private void UpdateCollection(IEnumerable<User> enumerable)
        {
            ListOfUsers.Clear();
            foreach (var item in enumerable)
            {
                ListOfUsers.Add(item);
            }

        }

        private RelayCommand _firstCommand;
        /// <summary>
        /// 
        /// </summary>
        public RelayCommand FirstCommand
        {
            get
            {
                return _firstCommand ??
                    (_firstCommand = new RelayCommand(obj =>
                    {
                        UpdateCollection(ListOfUsersForViewing.Take(UsersPerPage));
                        CurrentPage = 1;
                        UpdateEnableState();
                    }
                    ));
            }
        }

        private RelayCommand _previousCommand;
        /// <summary>
        /// 
        /// </summary>
        public RelayCommand PreviousCommand
        {
            get
            {
                return _previousCommand ??
                    (_previousCommand = new RelayCommand(obj =>
                    {
                        CurrentPage--;
                        int userToStartFrom = ListOfUsersForViewing.Count - UsersPerPage * (NumberOfPages - (CurrentPage - 1));
                        var usersToShow = ListOfUsersForViewing.Skip(userToStartFrom).Take(UsersPerPage);
                        UpdateCollection(usersToShow);
                        UpdateEnableState();
                    }
                    ));
            }
        }

        private RelayCommand _nextCommand;
        /// <summary>
        /// 
        /// </summary>
        public RelayCommand NextCommand
        {
            get
            {
                return _nextCommand ??
                    (_nextCommand = new RelayCommand(obj =>
                    {
                        int userToStartFrom = CurrentPage * UsersPerPage;
                        var usersToShow = ListOfUsersForViewing.Skip(userToStartFrom).Take(UsersPerPage);
                        UpdateCollection(usersToShow);
                        CurrentPage++;
                        UpdateEnableState();
                    }
                    ));
            }
        }

        private RelayCommand _lastCommand;
        /// <summary>
        /// 
        /// </summary>
        public RelayCommand LastCommand
        {
            get
            {
                return _lastCommand ??
                    (_lastCommand = new RelayCommand(obj =>
                    {
                        int usersToSkip = UsersPerPage * (NumberOfPages - 1);
                        UpdateCollection(ListOfUsersForViewing.Skip(usersToSkip));
                        CurrentPage = NumberOfPages;
                        UpdateEnableState();
                    }
                    ));
            }
        }

        private int _currentPage;
        /// <summary>
        /// 
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                UpdateEnableState();
            }
        }

        private int _usersPerPage;
        /// <summary>
        /// 
        /// </summary>
        public int UsersPerPage
        {
            get { return _usersPerPage; }
            set
            {
                _usersPerPage = value;
                OnPropertyChanged(nameof(UsersPerPage));
                UpdateUsersCount();
            }
        }

        private void UpdateUsersCount()
        {
            NumberOfPages = (int)Math.Ceiling((double)ListOfUsersForViewing.Count / UsersPerPage);
            NumberOfPages = NumberOfPages == 0 ? 1 : NumberOfPages;
            UpdateCollection(ListOfUsersForViewing.Take(UsersPerPage));
            CurrentPage = 1;
        }

        private int _numberOfPages;
        /// <summary>
        /// 
        /// </summary>
        public int NumberOfPages
        {
            get { return _numberOfPages; }
            set
            {
                _numberOfPages = value;
                OnPropertyChanged(nameof(NumberOfPages));
                UpdateEnableState();
            }
        }

        private void UpdateEnableState()
        {
            IsFirstEnable = CurrentPage > 1;
            IsPreviousEnable = CurrentPage > 1;
            IsNextEnable = CurrentPage < NumberOfPages;
            IsLastEnable = CurrentPage < NumberOfPages;
        }

        private bool _isFirstEnable;
        /// <summary>
        /// 
        /// </summary>
        public bool IsFirstEnable
        {
            get { return _isFirstEnable; }
            set
            {
                _isFirstEnable = value;
                OnPropertyChanged(nameof(IsFirstEnable));
            }
        }

        private bool _isPreviousEnable;
        /// <summary>
        /// 
        /// </summary>
        public bool IsPreviousEnable
        {
            get { return _isPreviousEnable; }
            set
            {
                _isPreviousEnable = value;
                OnPropertyChanged(nameof(IsPreviousEnable));
            }
        }

        private bool _isNextEnable;
        /// <summary>
        /// 
        /// </summary>
        public bool IsNextEnable
        {
            get { return _isNextEnable; }
            set
            {
                _isNextEnable = value;
                OnPropertyChanged(nameof(IsNextEnable));
            }
        }

        private bool _isLastEnable;
        /// <summary>
        /// 
        /// </summary>
        public bool IsLastEnable
        {
            get { return _isLastEnable; }
            set
            {
                _isLastEnable = value;
                OnPropertyChanged(nameof(IsLastEnable));
            }
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
    }
}
