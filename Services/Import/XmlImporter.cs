using System.Xml.Linq;
using WpfApp1.Model;

namespace WpfApp1.Services.Import
{
    class XmlImporter : IDataImporter
    {
        private const string UsersWord = "Users";
        private const string UserWord = "User";
        private const string DateWord = "Date";
        private const string FirstNameWord = "FirstName";
        private const string LastNameWord = "LastName";
        private const string PatronymicWord = "Patronymic";
        private const string CityWord = "City";
        private const string CountryWord = "Country";

        private const int AmountOfUsersToRead = 10000;

        public string ImporterName { get; set; } = "XmlImporter";

        /// <summary>
        /// The method for readind created xml file.
        /// </summary>
        /// <param name="fileName"> Name of file for reading. </param>
        /// <returns></returns>
        public async IAsyncEnumerable<List<User>> ReadFromFileAsync(string fileName)
        {
            List<User> users = new List<User>();

            XDocument xmlDocument = XDocument.Load(fileName);
            XElement? root = xmlDocument.Element(UsersWord);

            if (root != null)
            {
                foreach (XElement node in root.Elements(UserWord))
                {
                    PersonInfoStruct personInfo = new PersonInfoStruct(
                        node.Element(FirstNameWord)!.Value,
                        node.Element(LastNameWord)!.Value,
                        node.Element(PatronymicWord)!.Value);

                    EntranceInfoStruct entranceInfo = new EntranceInfoStruct(
                        DateOnly.Parse(node.Element(DateWord)!.Value),
                        node.Element(CityWord)!.Value,
                        node.Element(CountryWord)!.Value);

                    User user = new User(personInfo, entranceInfo);
                    users.Add(user);

                    if (users.Count >= AmountOfUsersToRead || node.NextNode is null)
                    {
                        yield return users;
                        users.Clear();
                    }
                }
            }
        }
    }
}