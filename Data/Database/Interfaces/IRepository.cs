using WpfApp1.Model;

namespace WpfApp1.Data.Database.Interfaces
{
    internal interface IRepository<T> : IDisposable
        where T : class
    {
        bool IsDBAvailable { get; set; }
        bool IsDBEmpty { get; set; }
        PersonInfoStruct? PersonInfo { get; set; }
        EntranceInfoStruct? EntranceInfo { get; set; }

        string ReturnConnectionStringValue();
        Task InitializeDBAsync(string connectionString);
        Task AddToDBAsync(List<User> listOfUsersFromFile);
        bool CheckAreThereUsersInDB();
        IAsyncEnumerable<List<User>> GetSelectionFromDBAsync(PersonInfoStruct person, EntranceInfoStruct entranceInfo);
    }
}
