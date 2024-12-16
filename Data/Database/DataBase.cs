using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.ViewModel.Events;

namespace WpfApp1.Data.Database
{
    internal class DataBase : IRepository<User>
    {
        private const int AmountOfUsersForSelection = 10000;

        private readonly ApplicationContext db;
        private readonly IConnectionStringValidation _connectionStringValidation;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDataFormatter _dataFormatter;

        public DataBase(IConnectionStringValidation connectionStringValidation, IEventAggregator eventAggregator, 
            IDataFormatter dataFormatter)
        {
            db = new ApplicationContext();
            _connectionStringValidation = connectionStringValidation;
            _eventAggregator = eventAggregator;
            _dataFormatter = dataFormatter;
        }

        private bool _isDBAvailable = true;
        /// <summary>
        /// A property for tracking availability of DB.
        /// </summary>
        public bool IsDBAvailable
        {
            get { return _isDBAvailable; }
            set
            {
                _isDBAvailable = value;
                _eventAggregator.GetEvent<ImportExportAvailabilityEvent>().Publish(_isDBAvailable);
            }
        }
        private bool _isDBEmpty = true;
        /// <summary>
        /// A property for tracking emptiness of DB.
        /// </summary>
        public bool IsDBEmpty 
        { 
            get { return _isDBEmpty; } 
            set
            {
                _isDBEmpty = value;
                _eventAggregator.GetEvent<ExportingAvailabilityEvent>().Publish(_isDBEmpty);
            }
        }

        private bool _wasExport = false;
        /// <summary>
        /// A property for tracking if export was and ended.
        /// </summary>
        public bool WasExport
        {
            get { return _wasExport; }
            set
            {
                _wasExport = value;
                _eventAggregator.GetEvent<ViewingAvailabilityEvent>().Publish(_wasExport);
            }
        }

        public PersonInfoStruct? PersonInfo { get; set; }
        public EntranceInfoStruct? EntranceInfo { get; set; }

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
                throw new Exception(Properties.Resources.ExWrongConnectionString);
            await db.Database.MigrateAsync();
            IsDBAvailable = true;
            IsDBEmpty = CheckAreThereUsersInDB();
        }

        /// <summary>
        /// The method for returning value of connection string.
        /// </summary>
        /// <returns></returns>
        public string ReturnConnectionStringValue()
        {
            return db.ReturnConnectionString();
        }

        /// <summary>
        /// Asynchronous method of writing data to DB.
        /// </summary>
        /// <returns></returns>
        public async Task AddToDBAsync(List<User> listOfUsersFromFile)
        {
            await db.Users.AddRangeAsync(listOfUsersFromFile);
            await db.SaveChangesAsync();

            if (IsDBEmpty) IsDBEmpty = CheckAreThereUsersInDB();
        }

        /// <summary>
        /// The method for checking if DB is empty.
        /// </summary>
        /// <returns></returns>
        public bool CheckAreThereUsersInDB()
        {
            return db.Users.IsNullOrEmpty();
        }

        /// <summary>
        /// Asynchronous method of reading data from DB 
        /// to create a selection for any combination of fields.
        /// </summary>
        /// <param name="person"> The structure with the user's personal data. </param>
        /// <param name="entranceInfo"> A structure with user entrance data. </param>
        /// <returns></returns>
        public async IAsyncEnumerable<List<User>> GetSelectionFromDBAsync(PersonInfoStruct person, EntranceInfoStruct entranceInfo)
        {
            List<User> listOfUsersFromDB = new List<User>();
            int amountOfUsersInDB = await db.Users.CountAsync();
            int amountOfViewUsers = 0;

            PersonInfo = person;
            EntranceInfo = entranceInfo;

            string dateForComparison = _dataFormatter.FormateDateOnly(entranceInfo.DateOfEntrance);

            while (amountOfViewUsers < amountOfUsersInDB)
            {
                listOfUsersFromDB.AddRange(await
                (from user in db.Users.Skip(amountOfViewUsers).Take(AmountOfUsersForSelection)
                 where
                 EF.Functions.Like(user.Date.ToString(), dateForComparison) &&
                 EF.Functions.Like(user.FirstName, person.FirstName) &&
                 EF.Functions.Like(user.LastName, person.LastName) &&
                 EF.Functions.Like(user.Patronymic, person.Patronymic) &&
                 EF.Functions.Like(user.City, entranceInfo.City) &&
                 EF.Functions.Like(user.Country, entranceInfo.Country)
                 select user).ToListAsync());

                amountOfViewUsers += AmountOfUsersForSelection;

                if (listOfUsersFromDB.Count >= AmountOfUsersForSelection || amountOfViewUsers >= amountOfUsersInDB)
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