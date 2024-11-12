using Microsoft.IdentityModel.Tokens;
using WpfApp1.Model;
using WpfApp1.Model.Interfaces;
using WpfApp1.MVVM;

namespace WpfApp1.ViewModel.ViewModels
{
    class ViewingPageViewModel : ViewModelBase
    {
        private const string Space = " ";

        private readonly IUsers _users;
        public ViewingPageViewModel(IUsers users)
        {
            _users = users;
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
                    (_showSelectionCommand = new RelayCommand(obj =>
                    {
                        OutputUsersToTextbox(_users.ReturnListOfUsersForView());
                    }
                    ));
            }
        }

        /// <summary>
        /// The method for displaying the created selection in the text field.
        /// </summary>
        /// <param name="listOfUsersForView"> List of users who were put into selection from DB. </param>
        internal void OutputUsersToTextbox(List<User> listOfUsersForView)
        {
            if (!listOfUsersForView.IsNullOrEmpty())
            {
                ViewText = "";
                if (listOfUsersForView.Count == 1000)
                    ViewText += "В данном окне будет показана только первая 1000 результатов запроса. " +
                        "Полную выборку можно увидеть в созданном файле.\r\n";

                ViewText += "Созданная выборка:\r\n";
                foreach (User u in listOfUsersForView)
                    ViewText += u.Date.ToString() + Space + u.FirstName + Space + u.LastName + Space
                            + u.Patronymic + Space + u.City + Space + u.Country + "\r\n";
                listOfUsersForView.Clear();
            }
            else ViewText = "Выборка не была создана!";
        }
    }
}
