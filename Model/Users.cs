using WpfApp1.Model.Interfaces;

namespace WpfApp1.Model
{
    internal class Users : IUsers
    {
        internal List<User> listOfUsersFromFile = new List<User>();
        internal List<User> listOfUsersFromDB = new List<User>();

        public List<User> ReturnListOfUsersFromFile()
        {
            return listOfUsersFromFile;
        }
        public List<User> ReturnListOfUsersFromDB()
        {
            return listOfUsersFromDB;
        }
        public void SetListOfUsersFromFile(List<User> listOfUsersFromFile)
        {
            this.listOfUsersFromFile = listOfUsersFromFile;
        }
        public void SetListOfUsersFromDB(List<User> listOfUsersFromDB)
        {
            this.listOfUsersFromDB = listOfUsersFromDB;
        }
    }
}
