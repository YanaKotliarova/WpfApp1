namespace WpfApp1.View.UI.Interfaces
{
    internal interface IFileDialog
    {
        bool OpenFileDialog(out string fileName);
        bool SaveFileDialog(out string fileName);
    }
}
