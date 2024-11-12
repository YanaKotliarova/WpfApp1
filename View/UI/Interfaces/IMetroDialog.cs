using WpfApp1.MVVM;

namespace WpfApp1.View.UI.Interfaces
{
    internal interface IMetroDialog
    {
        Task MetroDialogMessage(Object viewModel, string header, string message);
    }
}