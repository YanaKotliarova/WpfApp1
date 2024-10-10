namespace WpfApp1.ViewModel.Factories.Interfaces
{
    internal interface IAbstractFactory<T>
    {
        T Create();
    }
}