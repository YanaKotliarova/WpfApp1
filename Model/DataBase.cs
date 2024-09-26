using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WpfApp1.View;

namespace WpfApp1.Model
{
    internal class DataBase
    {
        private const string Space = " ";
        private const string Dot = ".";
        private const string Dash = "-";
        private const string Percent = "%";

        private const int AmountOfUsersForSelection = 3;

        internal int amoutOfUsersInDB = 0;
        internal int amountOfViewedUsers = 0;

        UIWorking uiWorking = new UIWorking();


        public async Task<ApplicationContext> InitializeDBAsync(ApplicationContext db)
        {
            if (!db.ValidateConnectionString()) 
                throw new Exception("Не валидатная строка подключения к БД.");

            await db.Database.MigrateAsync();

            return db;
        }

        /// <summary>
        /// Асинхронный метод записи данных в БД.
        /// </summary>
        /// <returns></returns>
        public async Task AddToDBAsync(List<User> ListOfUsersFromFile)
        {
            try
            {
                using (ApplicationContext db = await InitializeDBAsync(new ApplicationContext()))
                //using (ApplicationContext db = new ApplicationContext())
                {
                    //await db.Database.EnsureDeletedAsync();
                    //await db.Database.EnsureCreatedAsync();
                    //await db.Database.MigrateAsync();

                    foreach (User user in ListOfUsersFromFile)
                        await db.Users.AddAsync(user);

                    await db.SaveChangesAsync();

                    ListOfUsersFromFile.Clear();
                }
            }
            catch (Exception ex)
            {
                uiWorking.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// Асинхронный метод чтения данных из БД для создания выборки 
        /// по любой комбинации полей.
        /// </summary>
        /// <param name="dataForExport"> Массив значений полей для выборки. </param>
        /// <returns></returns>
        public async Task<List<User>> GetFromDBAsync(PersonStruct person, EntranceInfoStruct entranceInfo, List<User> ListOfUsersFromDB)
        {
            try
            {
                using (ApplicationContext db = await InitializeDBAsync(new ApplicationContext()))
                //using (ApplicationContext db = new ApplicationContext())
                {
                    amoutOfUsersInDB = db.Users.Count();

                        ListOfUsersFromDB = await
                            (from user in db.Users
                             where
                             EF.Functions.Like(user.Date.ToString(), entranceInfo.DateOfEntrance) && //гггг-мм-дд
                             EF.Functions.Like(user.FirstName, person.FirstName) &&
                             EF.Functions.Like(user.LastName, person.LastName) &&
                             EF.Functions.Like(user.Patronymic, person.Patronymic) &&
                             EF.Functions.Like(user.City, entranceInfo.City) &&
                             EF.Functions.Like(user.Country, entranceInfo.Country)
                             select user).Skip(amountOfViewedUsers).Take(AmountOfUsersForSelection).ToListAsync();

                    amountOfViewedUsers += AmountOfUsersForSelection;
                }
            }
            catch (Exception ex)
            {
                uiWorking.ShowMessage(ex.Message);
                amountOfViewedUsers = -1;
            }
            return ListOfUsersFromDB;
        }

        /// <summary>
        /// Метод преобразования даты в формат БД.
        /// </summary>
        /// <param name="date"> Преобразуемая дата. </param>
        /// <returns></returns>
        public string FormateDate(DateTime? dateTime)
        {
            string date;
            if (dateTime.HasValue) date = dateTime.Value.ToString("yyyy-MM-dd");
            else date = Percent;
            return date;
        }

        public string FormateStringData(string data)
        {
            if (data.IsNullOrEmpty()) data = Percent;
            return data;
        }
    }
}
