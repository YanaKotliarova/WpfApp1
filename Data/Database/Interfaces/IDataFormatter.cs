namespace WpfApp1.Data.Database.Interfaces
{
    internal interface IDataFormatter
    {
        string FormateDate(DateTime? dateTime);
        string FormateStringData(string data);
    }
}