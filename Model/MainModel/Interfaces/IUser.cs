﻿namespace WpfApp1.Model.MainModel.Interfaces
{
    internal interface IUser
    {
        string City { get; set; }
        string Country { get; set; }
        DateOnly Date { get; set; }
        string FirstName { get; set; }
        int Id { get; set; }
        string LastName { get; set; }
        string Patronymic { get; set; }
        public List<User> ReturnListOfUsersFromFile();
        public List<User> ReturnListOfUsersFromDB();
        public void SetListOfUsersFromFile(List<User> ListOfUsersFromFile);
        public void SetListOfUsersFromDB(List<User> ListOfUsersFromDB);
    }
}