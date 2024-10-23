using Microsoft.EntityFrameworkCore;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.ViewModel.Factories.Interfaces;

namespace WpfApp1.Data.Database
{
    internal class DataBase : IRepository<User>
    {
        private const int AmountOfUsersForSelection = 3;

        internal int amoutOfUsersInDB = 0;
        internal int amountOfViewedUsers = 0;

        private ApplicationContext db;

        private readonly IAbstractFactory<IConnectionStringValidation> _connectionStringValidationFactory;

        public DataBase(IAbstractFactory<IConnectionStringValidation> connectionStringValidationFactory)
        {
            db = new ApplicationContext();
            _connectionStringValidationFactory = connectionStringValidationFactory;
        }

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
            var validateString = _connectionStringValidationFactory.Create();

            if (!validateString.ValidateConnectionString(connectionString))
                throw new Exception("Не валидатная строка подключения к БД.");

            await db.Database.MigrateAsync();
        }

        /// <summary>
        /// Asynchronous method of writing data to DB.
        /// </summary>
        /// <returns></returns>
        public async Task AddToDBAsync(List<User> ListOfUsersFromFile)
        {
            foreach (User user in ListOfUsersFromFile)
                await db.Users.AddAsync(user);

            await db.SaveChangesAsync();

            ListOfUsersFromFile.Clear();
        }

        /// <summary>
        /// The method for returning amount of users in DB.
        /// </summary>
        /// <returns></returns>
        public async Task<int> ReturnAmountOfUsersInDBAsync()
        {
            amoutOfUsersInDB = await db.Users.CountAsync();
            return amoutOfUsersInDB;
        }

        /// <summary>
        /// The method for setting amount of viewed users in DB.
        /// </summary>
        /// <param name="amount"></param>
        public void SetAmountOfViewedUsers(int amount)
        {
            amountOfViewedUsers = amount;
        }

        /// <summary>
        /// The method for returning amount of viewed users in DB.
        /// </summary>
        /// <returns></returns>
        public int ReturnAmountOfViewedUsers()
        {
            return amountOfViewedUsers;
        }

        /// <summary>
        /// Asynchronous method of reading data from DB 
        /// to create a selection for any combination of fields.
        /// </summary>
        /// <param name="person"> The structure with the user's personal data. </param>
        /// <param name="entranceInfo"> A structure with user entrance data. </param>
        /// <param name="ListOfUsersFromDB"> List of users from DB. </param>
        /// <returns></returns>
        public async Task<List<User>> GetFromDBAsync(PersonStruct person, EntranceInfoStruct entranceInfo, List<User> ListOfUsersFromDB)
        {
            ListOfUsersFromDB = await
                (from user in db.Users.Skip(amountOfViewedUsers).Take(AmountOfUsersForSelection)
                 where
                 EF.Functions.Like(user.Date.ToString(), entranceInfo.DateOfEntrance) &&
                 EF.Functions.Like(user.FirstName, person.FirstName) &&
                 EF.Functions.Like(user.LastName, person.LastName) &&
                 EF.Functions.Like(user.Patronymic, person.Patronymic) &&
                 EF.Functions.Like(user.City, entranceInfo.City) &&
                 EF.Functions.Like(user.Country, entranceInfo.Country)
                 select user).ToListAsync();

            amountOfViewedUsers += AmountOfUsersForSelection;
            return ListOfUsersFromDB;
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