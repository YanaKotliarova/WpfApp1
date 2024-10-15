using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WpfApp1.Model.MainModel;
using WpfApp1.Model.MainModel.Interfaces;
using WpfApp1.MVVM;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class ViewingPageViewModel : ViewModelBase
    {
        private const string Space = " ";

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
                        var user = MainWindowViewModel.serviceProvider.GetService<IAbstractFactory<IUser>>()!.Create();
                        OutputUsersToTextbox(user.ReturnListOfUsersFromFile());
                    }
                    ));
            }
        }

        /// <summary>
        /// The method for displaying the created selection in the text field.
        /// </summary>
        /// <param name="ListOfUsersFromFile"> List of users who were put into selection from DB. </param>
        internal void OutputUsersToTextbox(List<User> ListOfUsersFromFile)
        {
            if (!ListOfUsersFromFile.IsNullOrEmpty())
            {
                ViewText = "Созданная выборка:\r\n";
                foreach (User u in ListOfUsersFromFile)
                    ViewText += u.Date.ToString() + Space + u.FirstName + Space + u.LastName + Space
                            + u.Patronymic + Space + u.City + Space + u.Country + "\r\n";
                ListOfUsersFromFile.Clear();
            }
            else ViewText = "Выборка не была создана!";
        }
    }
}
