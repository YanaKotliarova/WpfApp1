using OfficeOpenXml.Attributes;
using System.ComponentModel.DataAnnotations;
using WpfApp1.Model.Interfaces;

namespace WpfApp1.Model
{
    internal class User : IUser
    {
        [Key]
        [EpplusTableColumn(NumberFormat = "0")]
        public int Id { get; set; }

        [EpplusTableColumn(Header = "Дата", NumberFormat = "dd/mm/yyyy")]
        public DateOnly? Date { get; set; }

        [EpplusTableColumn(Header = "Имя")]
        public string FirstName { get; set; }

        [EpplusTableColumn(Header = "Фамилия")]
        public string LastName { get; set; }

        [EpplusTableColumn(Header = "Отчество")]
        public string Patronymic { get; set; }

        [EpplusTableColumn(Header = "Город")]
        public string City { get; set; }

        [EpplusTableColumn(Header = "Страна")]
        public string Country { get; set; }

        public User() { }

        /// <summary>
        /// The constructor of the User class, 
        /// which takes as a parameter two data structures about the user.
        /// </summary>
        /// <param name="person"> A structure that stores the user's first name, last name, and patronymic. </param>
        /// <param name="entranceInfo"> A structure that stores information about the user's entrance date, city, and country. </param>
        public User(PersonInfoStruct person, EntranceInfoStruct entranceInfo)
        {
            Date = entranceInfo.DateOfEntrance;
            FirstName = person.FirstName;
            LastName = person.LastName;
            Patronymic = person.Patronymic;
            City = entranceInfo.City;
            Country = entranceInfo.Country;
        }
    }
}
