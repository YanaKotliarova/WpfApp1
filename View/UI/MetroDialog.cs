using MahApps.Metro.Controls.Dialogs;
using WpfApp1.View.UI.Interfaces;

namespace WpfApp1.View.UI
{
    internal class MetroDialog : IMetroDialog
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        public MetroDialog(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
        }
        /// <summary>
        /// The method to show message in MetroMahapps dialog.
        /// </summary>
        /// <param name="viewModel"> ViewModel for creating a bound. </param>
        /// <param name="header"> Header of dialog. </param>
        /// <param name="message"> Message of dialog. </param>
        /// <returns></returns>
        public async Task MetroDialogMessage(Object viewModel, string header, string message)
        {
            await _dialogCoordinator.ShowMessageAsync(viewModel, header, message);
        }
    }
}
