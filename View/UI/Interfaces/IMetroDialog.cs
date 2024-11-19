using WpfApp1.MVVM;

namespace WpfApp1.View.UI.Interfaces
{
    internal interface IMetroDialog
    {
        Task ShowMessage(Object viewModel, string header, string message);
    }
}