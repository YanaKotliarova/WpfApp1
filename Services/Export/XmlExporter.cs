using Microsoft.IdentityModel.Tokens;
using System.Xml.Linq;
using WpfApp1.Model;

namespace WpfApp1.Services.Export
{
    internal class XmlExporter : IDataExporter
    {
        public string ExporterName { get; set; } = "XmlExporter";

        private const string UsersWord = "Users";
        private const string UserWord = "User";
        private const string IdWord = "ID";
        private const string DateWord = "Date";
        private const string FirstNameWord = "FirstName";
        private const string LastNameWord = "LastName";
        private const string PatronymicWord = "Patronymic";
        private const string CityWord = "City";
        private const string CountryWord = "Country";

        /// <summary>
        /// Asynchronous method for creating an Xml file.
        /// </summary>
        /// <param name="xmlFileName"> Name of file to be created. </param>
        /// <returns></returns>
        public async Task CreateFileAsync(string xmlFileName)
        {

            XDocument xDoc = new XDocument();
            XElement users = new XElement(UsersWord);
            xDoc.Add(users);

            await Task.Factory.StartNew(() => xDoc.Save(xmlFileName));
        }

        /// <summary>
        /// Asynchronous method for additional data recording
        /// received from the database into an Xml file.
        /// </summary>
        /// <param name="xmlFileName"> Name of Xml file. </param>
        /// <param name="listOfUsersFromDB"> List of users for recording. </param>
        /// <returns></returns>
        public async Task AddToFileAsync(string xmlFileName, List<User> listOfUsersFromDB)
        {
            if (listOfUsersFromDB.IsNullOrEmpty())
                throw new Exception("Выборка не была осуществена!\r\nПроверьте введённые данные.");


            XDocument xDoc = XDocument.Load(xmlFileName);
            XElement? root = xDoc.Element(UsersWord);

            if (root != null)
            {
                foreach (User user in listOfUsersFromDB)
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
                listOfUsersFromDB.Clear();
            }
        }
    }
}
