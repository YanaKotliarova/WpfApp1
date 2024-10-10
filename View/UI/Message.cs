using System.Windows;
using WpfApp1.View.UI.Interfaces;

namespace WpfApp1.View.UI
{
    internal class Message: IMessage
    {

        /// <summary>
        /// A method for displaying message boxes.
        /// </summary>
        /// <param name="message"> Text of message. </param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
