﻿using System.IO;
using WpfApp1.View;

namespace WpfApp1.Model
{
    internal class CsvFile
    {
        private const string Semicolon = ";";

        UIWorking uIWorking = new UIWorking();

        /// <summary>
        /// Асинхронный метод чтения данных из CSV файла.
        /// </summary>
        /// <param name="fileName"> Имя файла для чтения. </param>
        /// <returns></returns>
        public async Task ReadFromCsvFileAsync(string fileName, List<User> ListOfUsersFromFile)
        {
            try
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
            catch (Exception ex)
            {
                uIWorking.ShowMessage("Выбранный файл невозможно открыть. Возможно он поврежден.\r\n");
            }
        }

    }
}