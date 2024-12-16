using Microsoft.IdentityModel.Tokens;
using System.Xml.Linq;
using WpfApp1.Model;

namespace WpfApp1.Services.Export
{
    internal class XmlExporter : IDataExporter
    {
        public string ExporterName { get; set; } = "XmlExporter";

        private const string IdWord = "Id";

        /// <summary>
        /// Asynchronous method for creating an Xml file.
        /// </summary>
        /// <param name="xmlFileName"> Name of file to be created. </param>
        /// <returns></returns>
        public async Task CreateFileAsync(string xmlFileName)
        {

            XDocument xmlDocument = new XDocument();
            XElement users = new XElement(Properties.Resources.UsersWord);
            xmlDocument.Add(users);

            await Task.Factory.StartNew(() => xmlDocument.Save(xmlFileName));
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
            XDocument xmlDocument = XDocument.Load(xmlFileName);
            XElement? root = xmlDocument.Element(Properties.Resources.UsersWord);

            if (root != null)
            {
                foreach (User user in listOfUsersFromDB)
                {
                    XElement newUser = new XElement(Properties.Resources.UserWord);
                    XAttribute newUserId = new XAttribute(IdWord, user.Id.ToString());
                    XElement newUserDate = new XElement(Properties.Resources.DateWord, user.Date);
                    XElement newUserFirstName = new XElement(Properties.Resources.FirstNameWord, user.FirstName);
                    XElement newUserLastName = new XElement(Properties.Resources.LastNameWord, user.LastName);
                    XElement newUserPatronymic = new XElement(Properties.Resources.PatronymicWord, user.Patronymic);
                    XElement newUserCity = new XElement(Properties.Resources.CityWord, user.City);
                    XElement newUserCountry = new XElement(Properties.Resources.CountryWord, user.Country);

                    newUser.Add(newUserId, newUserDate, newUserFirstName, newUserLastName, newUserPatronymic, newUserCity, newUserCountry);
                    root.Add(newUser);
                }
                await Task.Factory.StartNew(() => xmlDocument.Save(xmlFileName));
                listOfUsersFromDB.Clear();
            }
        }
    }
}
