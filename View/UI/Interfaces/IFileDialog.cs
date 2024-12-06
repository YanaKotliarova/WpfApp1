namespace WpfApp1.View.UI.Interfaces
{
    internal interface IFileDialog
    {
        bool OpenFileDialog(out string fileName, string extensionFilter);
        bool SaveFileDialog(out string fileName);
    }
}
