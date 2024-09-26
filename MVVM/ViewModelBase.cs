using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1.MVVM
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Метод учитывания изменения визуального компонента приложения.
        /// В основном используется для текстовых полей.
        /// </summary>
        /// <param name="propertyName"> Имя компонента. </param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
