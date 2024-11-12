using WpfApp1.Model;

namespace WpfApp1.Data.Database.Interfaces
{
    internal interface IRepository<T> : IDisposable
        where T : class
    {
        string ReturnConnectionStringValue();
        Task InitializeDBAsync(string connectionString);
        Task AddToDBAsync(List<User> listOfUsersFromFile);
        Task<int> ReturnAmountOfUsersInDBAsync();
        void SetAmountOfViewedUsers(int amount);
        int ReturnAmountOfViewedUsers();
        bool AreThereUsersInDB();
        bool ReturnIsDBEmpty();
        IAsyncEnumerable<List<User>> GetSelectionFromDBAsync(PersonStruct person, EntranceInfoStruct entranceInfo);
    }
}
