using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
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

        public User() { }

        /// <summary>
        /// Конструктор класса User, принимающий в качестве параметра массив строк,
        /// содержащий в себе дату, имя, фамилию, отчество, город и страну пользователя.
        /// </summary>
        /// <param name="data"> Массив строк. </param>
        public User(string[] data)
        {
            Date = DateOnly.Parse(data[0]);
            FirstName = data[1];
            LastName = data[2];
            Patronymic = data[3];
            City = data[4];
            Country = data[5];
        }
    }
}
