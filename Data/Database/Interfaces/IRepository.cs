﻿using WpfApp1.Model;

namespace WpfApp1.Data.Database.Interfaces
{
    internal interface IRepository<T> : IDisposable
        where T : class
    {
        string ReturnConnectionStringValue();
        Task InitializeDBAsync(string connectionString);
        Task AddToDBAsync(List<User> ListOfUsersFromFile);
        Task<int> ReturnAmountOfUsersInDBAsync();
        void SetAmountOfViewedUsers(int amount);
        int ReturnAmountOfViewedUsers();
        Task<List<User>> GetFromDBAsync(PersonStruct person, EntranceInfoStruct entranceInfo, List<User> ListOfUsersFromDB);
    }
}
