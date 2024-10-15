using WpfApp1.Model.MainModel;

namespace WpfApp1.Model.Database.Interfaces
{
    internal interface IRepository<T> : IDisposable
        where T : class
    {
        Task InitializeDBAsync();
        Task AddToDBAsync(List<User> ListOfUsersFromFile);
        Task<int> ReturnAmountOfUsersInDBAsync();
        void SetAmountOfViewedUsers(int amount);
        int ReturnAmountOfViewedUsers();
        Task<List<User>> GetFromDBAsync(PersonStruct person, EntranceInfoStruct entranceInfo, List<User> ListOfUsersFromDB);
    }
}
