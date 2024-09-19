using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    internal class CSVFile
    {
        private const string Semicolon = ";";

        //Program program = Program.Instance;
        internal List<User> users = new List<User>();

        /// <summary>
        /// Асинхронный метод чтения данных из CSV файла.
        /// </summary>
        /// <param name="fileName"> Имя файла для чтения. </param>
        /// <returns></returns>
        public async Task ReadFromCsvFileAsync(string fileName)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(fileName))
                {
                    string stringFromFile;
                    string[] dataFromString = new string[5];
                    User user;
                    while ((stringFromFile = await streamReader.ReadLineAsync()) != null)
                    {
                        dataFromString = stringFromFile.Split(Semicolon);
                        user = new User(dataFromString);
                        //program.ListOfUsersFromFile.Add(user);
                        users.Add(user);
                    }
                    string str = "";
                    foreach (User u in users)
                    {
                        str += u.FirstName.ToString() + "\r\n";
                    }
                    MessageBox.Show(str);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Выбранный файл невозможно открыть. Возможно он поврежден.\r\n" + ex);
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(async obj => { await ReadFromCsvFileAsync("Users.csv"); }));
            }
        }
    }
}
