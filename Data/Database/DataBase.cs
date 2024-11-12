using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Windows;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;

namespace WpfApp1.Data.Database
{
    internal class DataBase : IRepository<User>
    {
        private const int AmountOfUsersForSelection = 1000;

        private readonly ApplicationContext db;
        private readonly IConnectionStringValidation _connectionStringValidation;

        public DataBase(IConnectionStringValidation connectionStringValidation)
        {
            db = new ApplicationContext();
            _connectionStringValidation = connectionStringValidation;
        }

        private int _amoutOfUsersInDB = 0;
        private int _amountOfViewedUsers = 0;

        private bool _isDBEmpty;

        /// <summary>
        /// Return value of connection string.
        /// </summary>
        /// <returns></returns>
        public string ReturnConnectionStringValue()
        {
            return db.ReturnConnectionString();
        }

        /// <summary>
        /// A method for asynchronous DB initialization using migration.
        /// </summary>
        /// <param name="db"> An object of the ApplicationContext class, for calling methods of this class.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task InitializeDBAsync(string connectionString)
        {
            db.SetConnectionString(connectionString);
            if (!_connectionStringValidation.ValidateConnectionString(connectionString))
                throw new Exception("Не валидатная строка подключения к БД.");
            await db.Database.MigrateAsync();
            _isDBEmpty = AreThereUsersInDB();
        }

        /// <summary>
        /// Asynchronous method of writing data to DB.
        /// </summary>
        /// <returns></returns>
        public async Task AddToDBAsync(List<User> listOfUsersFromFile)
        {
            await db.Users.AddRangeAsync(listOfUsersFromFile);
            await db.SaveChangesAsync();

            listOfUsersFromFile.Clear();
            _isDBEmpty = AreThereUsersInDB();
        }

        /// <summary>
        /// The method for returning amount of users in DB.
        /// </summary>
        /// <returns></returns>
        public async Task<int> ReturnAmountOfUsersInDBAsync()
        {
            _amoutOfUsersInDB = await db.Users.CountAsync();
            return _amoutOfUsersInDB;
        }

        /// <summary>
        /// The method for checking if DB is empty.
        /// </summary>
        /// <returns></returns>
        public bool AreThereUsersInDB()
        {
            return db.Users.IsNullOrEmpty();
        }

        /// <summary>
        /// The method for returning the value of DB emptiness checking.
        /// </summary>
        /// <returns></returns>
        public bool ReturnIsDBEmpty()
        {
            return _isDBEmpty;
        }

        /// <summary>
        /// The method for setting amount of viewed users in DB.
        /// </summary>
        /// <param name="amount"></param>
        public void SetAmountOfViewedUsers(int amount)
        {
            _amountOfViewedUsers = amount;
        }

        /// <summary>
        /// The method for returning amount of viewed users in DB.
        /// </summary>
        /// <returns></returns>
        public int ReturnAmountOfViewedUsers()
        {
            return _amountOfViewedUsers;
        }

        /// <summary>
        /// Asynchronous method of reading data from DB 
        /// to create a selection for any combination of fields.
        /// </summary>
        /// <param name="person"> The structure with the user's personal data. </param>
        /// <param name="entranceInfo"> A structure with user entrance data. </param>
        /// <returns></returns>
        public async IAsyncEnumerable<List<User>> GetSelectionFromDBAsync(PersonStruct person, EntranceInfoStruct entranceInfo)
        {
            List<User> listOfUsersFromDB = new List<User>();

            while (_amountOfViewedUsers < _amoutOfUsersInDB)
            {
                listOfUsersFromDB.AddRange(await
                (from user in db.Users.Skip(_amountOfViewedUsers).Take(AmountOfUsersForSelection)
                 where
                 EF.Functions.Like(user.Date.ToString(), entranceInfo.DateOfEntrance) &&
                 EF.Functions.Like(user.FirstName, person.FirstName) &&
                 EF.Functions.Like(user.LastName, person.LastName) &&
                 EF.Functions.Like(user.Patronymic, person.Patronymic) &&
                 EF.Functions.Like(user.City, entranceInfo.City) &&
                 EF.Functions.Like(user.Country, entranceInfo.Country)
                 select user).ToListAsync());

                _amountOfViewedUsers += AmountOfUsersForSelection;

                if (listOfUsersFromDB.Count >= AmountOfUsersForSelection || _amountOfViewedUsers >= _amoutOfUsersInDB)
                {
                    yield return listOfUsersFromDB;
                    listOfUsersFromDB.Clear();
                }
            }
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}