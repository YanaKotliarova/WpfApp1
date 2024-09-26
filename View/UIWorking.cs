using Microsoft.Win32;
using System.Windows;

namespace WpfApp1.View
{
    internal class UIWorking
    {
        private const string FileExtensionFilter = "Text files (*.csv) | *.csv";

        /// <summary>
        /// Метод показа окон с сообщениями.
        /// </summary>
        /// <param name="message"></param>
        internal void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// Метод для открытия диалога выбора файлов.
        /// </summary>
        /// <param name="fileName"> Имя выбранного для открытия файла. </param>
        /// <returns></returns>
        public bool OpenFileDialog(out string fileName)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = FileExtensionFilter;
            openFileDialog.Multiselect = false;

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                fileName = openFileDialog.FileName;
            }
            else
            {
                fileName = "Ошибка открытия файла";
                ShowMessage(fileName);
            }
            return (bool)result;
        }
    }
}
