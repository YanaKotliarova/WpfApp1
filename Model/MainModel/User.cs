using System.ComponentModel.DataAnnotations;
using WpfApp1.Model.MainModel.Interfaces;

namespace WpfApp1.Model.MainModel
{
    internal class User : IUser
    {
        [Key]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

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
        public User() { }

        /// <summary>
        /// The constructor of the User class, 
        /// which takes as a parameter two data structures about the user.
        /// </summary>
        /// <param name="person"> A structure that stores the user's first name, last name, and patronymic. </param>
        /// <param name="entranceInfo"> A structure that stores information about the user's entrance date, city, and country. </param>
        public User(PersonStruct person, EntranceInfoStruct entranceInfo)
        {
            Date = DateOnly.Parse(entranceInfo.DateOfEntrance);
            FirstName = person.FirstName;
            LastName = person.LastName;
            Patronymic = person.Patronymic;
            City = entranceInfo.City;
            Country = entranceInfo.Country;
        }
    }
}
