using Microsoft.IdentityModel.Tokens;
using WpfApp1.Model.Database.Interfaces;

namespace WpfApp1.Model.Database
{
    internal class DataFormatter : IDataFormatter
    {
        private const string Percent = "%";
        private const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// The method of converting the date to the DB format.
        /// </summary>
        /// <param name="date"> Date to be converted. </param>
        /// <returns></returns>
        public string FormateDate(DateTime? dateTime)
        {
            string date;
            if (dateTime.HasValue) date = dateTime.Value.ToString(DateFormat);
            else date = Percent;
            return date;
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
