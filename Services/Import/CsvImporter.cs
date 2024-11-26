using System.IO;
using WpfApp1.Model;

namespace WpfApp1.Services.Import
{
    internal class CsvImporter : IDataImporter
    {
        private const string Semicolon = ";";
        private const int AmountOfUsersToRead = 10000;

        /// <summary>
        /// Asynchronous method of reading data from a CSV file.
        /// </summary>
        /// <param name="fileName"> Path to file for reading. </param>
        /// <returns></returns>
        public async IAsyncEnumerable<List<User>> ReadFromFileAsync(string fileName)
        {
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                List<User> listOfUsersFromFile = new List<User>();
                listOfUsersFromFile.Capacity = AmountOfUsersToRead;

                string stringFromFile;
                string[] dataFromString = new string[5];
                User newUser;
                while ((stringFromFile = await streamReader.ReadLineAsync()) != null)
                {
                    dataFromString = stringFromFile.Split(Semicolon);

                    PersonInfoStruct person = new PersonInfoStruct(dataFromString[1], dataFromString[2], dataFromString[3]);
                    EntranceInfoStruct entranceInfo = new EntranceInfoStruct(dataFromString[0], dataFromString[4], dataFromString[5]);

                    newUser = new User(person, entranceInfo);

                    listOfUsersFromFile.Add(newUser);

                    if (listOfUsersFromFile.Count >= AmountOfUsersToRead || streamReader.EndOfStream)
                    {
                        yield return listOfUsersFromFile;
                        listOfUsersFromFile.Clear();
                    }
                }
            }
        }
    }

}