using System.Xml.Linq;
using WpfApp1.Model;

namespace WpfApp1.Services.Import
{
    class XmlImporter : IDataImporter
    {
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
            XElement? root = xmlDocument.Element(Properties.Resources.UsersWord);

            if (root != null)
            {
                foreach (XElement node in root.Elements(Properties.Resources.UserWord))
                {
                    PersonInfoStruct personInfo = new PersonInfoStruct(
                        node.Element(Properties.Resources.FirstNameWord)!.Value,
                        node.Element(Properties.Resources.LastNameWord)!.Value,
                        node.Element(Properties.Resources.PatronymicWord)!.Value);

                    EntranceInfoStruct entranceInfo = new EntranceInfoStruct(
                        DateOnly.Parse(node.Element(Properties.Resources.DateWord)!.Value),
                        node.Element(Properties.Resources.CityWord)!.Value,
                        node.Element(Properties.Resources.CountryWord)!.Value);

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