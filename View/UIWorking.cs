using System.Windows;

namespace WpfApp1.View
{
    internal class UIWorking
    {
        private const string FileExtensionFilter = "Text files (*.csv) | *.csv";

        internal void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public bool OpenFileDialog(out string fileName)
        {


            var openFileDialog = new Microsoft.Win32.OpenFileDialog();

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
