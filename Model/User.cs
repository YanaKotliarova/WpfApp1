using System.ComponentModel.DataAnnotations;

namespace WpfApp1.Model
{
    internal class User
    {
        [Key]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        internal List<User> ListOfUsersFromFile = new List<User>();
        internal List<User> ListOfUsersFromDB = new List<User>();

        public User() { }

        /// <summary>
        /// Конструктор класса User, принимающий в качестве параметра две структуры данных о пользователе,
        /// содержащий в себе дату входа, имя, фамилию, отчество, город и страну пользователя.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="entranceInfo"></param>
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
