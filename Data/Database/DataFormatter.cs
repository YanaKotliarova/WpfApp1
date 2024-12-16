using Microsoft.IdentityModel.Tokens;
using WpfApp1.Data.Database.Interfaces;

namespace WpfApp1.Data.Database
{
    internal class DataFormatter : IDataFormatter
    {
        private const string Percent = "%";
        private const string DbDateFormat = "yyyy-MM-dd";

        /// <summary>
        /// The method of converting the date to the DB format.
        /// </summary>
        /// <param name="date"> Date to be converted. </param>
        /// <returns></returns>
        public string FormateDateOnly(DateOnly? dateOnly)
        {
            string date;
            if (dateOnly.HasValue) date = dateOnly.Value.ToString(DbDateFormat);
            else date = Percent;
            return date;
        }

        /// <summary>
        /// The method of converting the date from DateTime to DateOnly format.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public DateOnly? FormateDateTime(DateTime? dateTime)
        {
            if (dateTime == null) return null;
            return DateOnly.FromDateTime(dateTime.Value);
        }

        /// <summary>
        /// The method of converting an empty string to search in DB.
        /// </summary>
        /// <param name="data"> String to be converted. </param>
        /// <returns></returns>
        public string FormateStringData(string data)
        {
            if (data.IsNullOrEmpty()) data = Percent;
            return data;
        }
    }
}
