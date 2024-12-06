using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using System.Windows.Controls;
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
        public async Task ShowMessage(object viewModel, string header, string message)
        {
            await _dialogCoordinator.ShowMessageAsync(viewModel, header, message);
        }

        public async Task<ProgressDialogController> ShowMessageWithProgressBar(object viewModel, string header, string message)
        {
            ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(viewModel, header, message);
            controller.SetIndeterminate();
            return controller;
        }

        public async Task CloseShowMessageWithProgressBar(ProgressDialogController controller)
        {
            await controller.CloseAsync();
        }

        /// <summary>
        /// The method for returning viewmodel of the current open page.
        /// </summary>
        /// <returns></returns>
        public object ReturnViewModel()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            Frame frame = window.FindName("MainFrame") as Frame;
            Page page = frame.Content as Page;
            object viewModel = page.DataContext;

            return viewModel;
        }
    }
}
