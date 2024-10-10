using WpfApp1.Model.MainModel;

namespace WpfApp1.Model.Database.Interfaces
{
    internal interface IRepository<T> : IDisposable
        where T : class
    {
        void InitializeDB();
        Task AddToDBAsync(List<User> ListOfUsersFromFile);
        int ReturnAmountOfUsersInDB();
        void SetAmountOfViewedUsers(int amount);
        int ReturnAmountOfViewedUsers();
        Task<List<User>> GetFromDBAsync(PersonStruct person, EntranceInfoStruct entranceInfo, List<User> ListOfUsersFromDB);
    }
}
