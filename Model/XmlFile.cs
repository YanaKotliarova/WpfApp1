using Microsoft.IdentityModel.Tokens;
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
        /// Асинхронный метод записи выборки из БД в Xml файл.
        /// </summary>
        /// <param name="xmlFileName"> Имя создаваемого файла. </param>
        /// <returns></returns>
        public async Task WriteIntoXmlFileAsync(string xmlFileName, List<User> ListOfUsersFromDB)
        {
            try
            {
                if (ListOfUsersFromDB.IsNullOrEmpty())
                    throw new Exception("Выборка не была осуществена!");

                XDocument xDoc = new XDocument();
                XElement users = new XElement(UsersWord);

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
                    users.Add(newUser);
                }
                xDoc.Add(users);

                await Task.Factory.StartNew(() => xDoc.Save(xmlFileName));

                uIWorking.ShowMessage("XML файл " + xmlFileName + " создан");
                ListOfUsersFromDB.Clear();
            }
            catch (Exception ex)
            {
                uIWorking.ShowMessage("Не удалось создать XML файл!");
            }
        }
    }
}
