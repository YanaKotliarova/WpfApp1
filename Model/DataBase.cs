﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Windows.Markup;
using WpfApp1.View;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfApp1.Model
{
    internal class DataBase
    {
        private const string Space = " ";
        private const string Dot = ".";
        private const string Dash = "-";
        private const string Percent = "%";

        //private const int AmountOfUsersForSelection = 3;

        UIWorking uIWorking = new UIWorking();


        /// <summary>
        /// Асинхронный метод записи данных в БД.
        /// </summary>
        /// <returns></returns>
        public async Task AddToDBAsync(List<User> ListOfUsersFromFile)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                await db.Database.EnsureDeletedAsync();
                await db.Database.EnsureCreatedAsync();

                foreach (User user in ListOfUsersFromFile)
                    await db.Users.AddAsync(user);

                await db.SaveChangesAsync();

                ListOfUsersFromFile.Clear();
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
                using (ApplicationContext db = new ApplicationContext())
                {
                    ListOfUsersFromDB = await
                            (from user in db.Users
                             where
                             EF.Functions.Like(user.Date.ToString(), entranceInfo.DateOfEntrance) && //гггг-мм-дд
                             EF.Functions.Like(user.FirstName, person.FirstName) &&
                             EF.Functions.Like(user.LastName, person.LastName) &&
                             EF.Functions.Like(user.Patronymic, person.Patronymic) &&
                             EF.Functions.Like(user.City, entranceInfo.City) &&
                             EF.Functions.Like(user.Country, entranceInfo.Country)
                             select user).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                uIWorking.ShowMessage("Сначала загрузите файл!");
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