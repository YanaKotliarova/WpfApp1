using Microsoft.Win32;
using WpfApp1.View.UI.Interfaces;

namespace WpfApp1.View.UI
{
    internal class FileDialog: IFileDialog
    {
        private const string FileExtensionFilter = "Text files (*.csv) | *.csv";

        /// <summary>
        /// The method for opening the file selection dialog.
        /// </summary>
        /// <param name="fileName"> Name of chosen file. </param>
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
                fileName = "";
                throw new Exception("Ошибка открытия файла");
            }
            return (bool)result;
        }
    }
}
