namespace WpfApp1.Data.Database.Interfaces
{
    internal interface IDataFormatter
    {
        string FormateDateOnly(DateOnly? dateOnly);
        DateOnly? FormateDateTime(DateTime? dateTime);
        string FormateStringData(string data);
    }
}