namespace WpfApp1.Model.Interfaces
{
    internal interface IUsers
    {
        List<User> ReturnListOfUsersFromDB();
        List<User> ReturnListOfUsersFromFile();
        List<User> ReturnListOfUsersForView();
        void SetListOfUsersFromDB(List<User> listOfUsersFromDB);
        void SetListOfUsersFromFile(List<User> listOfUsersFromFile);
        void SetListOfUsersForView(List<User> listOfUsersForView);
    }
}