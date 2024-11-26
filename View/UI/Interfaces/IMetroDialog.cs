namespace WpfApp1.View.UI.Interfaces
{
    internal interface IMetroDialog
    {
        object ReturnViewModel();
        Task ShowMessage(object viewModel, string header, string message);
    }
}