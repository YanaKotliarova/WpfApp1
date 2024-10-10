namespace WpfApp1.Model.Database.Interfaces
{
    internal interface IDataFormatter
    {
        string FormateDate(DateTime? dateTime);
        string FormateStringData(string data);
    }
}