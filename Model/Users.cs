using WpfApp1.Model.Interfaces;

namespace WpfApp1.Model
{
    internal class Users : IUsers
    {
        internal List<User> listOfUsersFromFile = new List<User>();
        internal List<User> listOfUsersFromDB = new List<User>();
        internal List<User> listOfUsersForView = new List<User>();

        public Users()
        {
            listOfUsersFromFile.Capacity = 1000;
            listOfUsersFromDB.Capacity = 1000;
            listOfUsersForView.Capacity = 1000;
        }

        public List<User> ReturnListOfUsersFromFile()
        {
            return listOfUsersFromFile;
        }
        public List<User> ReturnListOfUsersFromDB()
        {
            return listOfUsersFromDB;
        }
        public List<User> ReturnListOfUsersForView()
        {
            return listOfUsersForView;
        }
        public void SetListOfUsersFromFile(List<User> listOfUsersFromFile)
        {
            this.listOfUsersFromFile = listOfUsersFromFile;
        }
        public void SetListOfUsersFromDB(List<User> listOfUsersFromDB)
        {
            this.listOfUsersFromDB = listOfUsersFromDB;
        }
        public void SetListOfUsersForView(List<User> listOfUsersForView)
        {
            this.listOfUsersForView = listOfUsersForView;
        }
    }
}
