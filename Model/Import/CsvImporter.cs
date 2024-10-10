using System.IO;
using WpfApp1.Model.MainModel;

namespace WpfApp1.Model.Import
{
    internal class CsvImporter : IDataImporter
    {
        private const string Semicolon = ";";

        /// <summary>
        /// Asynchronous method of reading data from a CSV file.
        /// </summary>
        /// <param name="fileName"> Name of file for reading. </param>
        /// <returns></returns>
        public async Task ReadFromFileAsync(string fileName, List<User> ListOfUsersFromFile)
        {
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                string stringFromFile;
                string[] dataFromString = new string[5];
                User newUser;
                while ((stringFromFile = await streamReader.ReadLineAsync()) != null)
                {
                    dataFromString = stringFromFile.Split(Semicolon);

                    PersonStruct person = new PersonStruct(dataFromString[1], dataFromString[2], dataFromString[3]);
                    EntranceInfoStruct entranceInfo = new EntranceInfoStruct(dataFromString[0], dataFromString[4], dataFromString[5]);

                    newUser = new User(person, entranceInfo);

                    ListOfUsersFromFile.Add(newUser);
                }
            }
        }
    }

}