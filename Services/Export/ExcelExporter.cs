using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.IO;
using WpfApp1.Model;

namespace WpfApp1.Services.Export
{
    internal class ExcelExporter : IDataExporter
    {
        public string ExporterName { get; set; } = "ExcelExporter";

        private const string IdWord = "ID";
        private const string DateWord = "Дата";
        private const string FirstNameWord = "Имя";
        private const string LastNameWord = "Фамилия";
        private const string PatronymicWord = "Отчество";
        private const string CityWord = "Город";
        private const string CountryWord = "Страна";
        private const string WorksheetWord = "Лист1";

        /// <summary>
        /// Asynchronous method for creating an Excel file.
        /// </summary>
        /// <param name="excelFileName"> Name of file to be created. </param>
        /// <returns></returns>
        public async Task CreateFileAsync(string excelFileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Worksheets.Add(WorksheetWord);

                var headerRow = new List<string[]>() { new string[] 
                { IdWord, DateWord, FirstNameWord, LastNameWord, PatronymicWord, CityWord, CountryWord } };

                string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                var worksheet = excelPackage.Workbook.Worksheets[WorksheetWord];
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                FileInfo excelFile = new FileInfo(excelFileName);

                await excelPackage.SaveAsAsync(excelFile);
            }
        }

        /// <summary>
        /// Asynchronous method for additional data recording
        /// received from the database into an Excel file.
        /// </summary>
        /// <param name="excelFileName"> Name of Excel file.</param>
        /// <param name="listOfUsersFromDB"> List of users for recording. </param>
        /// <returns></returns>
        public async Task AddToFileAsync(string excelFileName, List<User> listOfUsersFromDB)
        {
            if (listOfUsersFromDB.IsNullOrEmpty())
                throw new Exception("Выборка не была осуществена!\r\nПроверьте введённые данные.");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage(excelFileName))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();

                int rows = worksheet.Dimension.Rows;

                worksheet.Column(2).Style.Numberformat.Format = "dd/mm/yyyy";

                List<Object[]> dataForFile = new List<Object[]>();
                Object[] cellData;

                foreach (User user in listOfUsersFromDB)
                {
                    cellData = [user.Id, user.Date, user.FirstName, user.LastName, user.Patronymic, user.City, user.Country];
                    dataForFile.Add(cellData);
                }

                worksheet.Cells[rows + 1, 1].LoadFromArrays(dataForFile);

                worksheet.Columns.AutoFit();

                await excelPackage.SaveAsync();
            }
            listOfUsersFromDB.Clear();
        }

    }
}
