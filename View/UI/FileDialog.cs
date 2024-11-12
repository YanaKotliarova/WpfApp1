using Microsoft.Win32;
using WpfApp1.View.UI.Interfaces;

namespace WpfApp1.View.UI
{
    internal class FileDialog: IFileDialog
    {
        private const string OpenFileExtensionFilter = "Special text files (*.csv) | *.csv";
        private const string SaveFileExtensionFilter = "Excel documents|*.xlsx|Xml documents|*.xml";

        private const string DefaultFileName = "Users";

        /// <summary>
        /// The method for opening the file selection dialog.
        /// </summary>
        /// <param name="fileName"> Name of chosen file. </param>
        /// <returns></returns>
        public bool OpenFileDialog(out string fileName)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = OpenFileExtensionFilter;
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

        public bool SaveFileDialog(out string fileName)
        {
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = DefaultFileName;
            saveFileDialog.Filter = SaveFileExtensionFilter;

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                fileName = saveFileDialog.FileName;
            }
            else
            {
                fileName = "";
                throw new Exception("Ошибка сохранения файла");
            }
            return (bool)result;
        }
    }
}
