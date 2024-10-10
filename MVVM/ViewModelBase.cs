using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1.MVVM
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// A method for taking into account changes 
        /// in the visual component of the application. 
        /// It is mainly used for text fields.
        /// </summary>
        /// <param name="propertyName"> Имя компонента. </param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
