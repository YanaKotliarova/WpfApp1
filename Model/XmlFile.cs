﻿using Microsoft.IdentityModel.Tokens;
using System.Xml.Linq;
using WpfApp1.View;

namespace WpfApp1.Model
{
    internal class XmlFile
    {
        private const string UsersWord = "Users";
        private const string UserWord = "User";
        private const string IdWord = "ID";
        private const string DateWord = "Date";
        private const string FirstNameWord = "FirstName";
        private const string LastNameWord = "LastName";
        private const string PatronymicWord = "Patronymic";
        private const string CityWord = "City";
        private const string CountryWord = "Country";

        UIWorking uIWorking = new UIWorking();

        /// <summary>
        /// Асинхронный метод создания Xml файла.
        /// </summary>
        /// <param name="xmlFileName"> Имя создаваемого файла. </param>
        /// <returns></returns>
        public async Task CreateXmlFileAsync(string xmlFileName)
        {
            try
            {
                XDocument xDoc = new XDocument();
                XElement users = new XElement(UsersWord);
                xDoc.Add(users);

                await Task.Factory.StartNew(() => xDoc.Save(xmlFileName));
            }
            catch (Exception ex)
            {
                uIWorking.ShowMessage("Не удалось создать XML файл!" + ex.Message);
            }
        }

        /// <summary>
        /// Асинхронный метод дозаписи полученных из БД данный в созданный Xml файл.
        /// </summary>
        /// <param name="xmlFileName"> Имя Xml файла. </param>
        /// <param name="ListOfUsersFromDB"> Список пользователей для записи. </param>
        /// <returns></returns>
        public async Task AddToXmlFileAsync(string xmlFileName, List<User> ListOfUsersFromDB)
        {
            try
            {
                if (ListOfUsersFromDB.IsNullOrEmpty())
                    throw new Exception("Выборка не была осуществена!\r\nПроверьте введённые данные.");


                XDocument xDoc = XDocument.Load(xmlFileName);
                XElement? root = xDoc.Element(UsersWord);

                if (root != null)
                {
                    foreach (User user in ListOfUsersFromDB)
                    {
                        XElement newUser = new XElement(UserWord + user.Id.ToString());
                        XElement newUserDate = new XElement(DateWord, user.Date);
                        XElement newUserFirstName = new XElement(FirstNameWord, user.FirstName);
                        XElement newUserLastName = new XElement(LastNameWord, user.LastName);
                        XElement newUserPatronymic = new XElement(PatronymicWord, user.Patronymic);
                        XElement newUserCity = new XElement(CityWord, user.City);
                        XElement newUserCountry = new XElement(CountryWord, user.Country);

                        newUser.Add(newUserDate, newUserFirstName, newUserLastName, newUserPatronymic, newUserCity, newUserCountry);
                        root.Add(newUser);
                    }
                    await Task.Factory.StartNew(() => xDoc.Save(xmlFileName));
                    ListOfUsersFromDB.Clear();
                }
            }
            catch (Exception ex)
            {
                //uIWorking.ShowMessage("Не удалось создать XML файл!" + ex.Message);
            }
        }
    }
}
