using OfficeOpenXml;
using System.IO;
using WpfApp1.Model;

namespace WpfApp1.Services.Export
{
    internal class ExcelExporter : IDataExporter
    {
        public string ExporterName { get; set; } = "ExcelExporter";

        private const string IdWord = "Id";

        /// <summary>
        /// Asynchronous method for creating an Excel file and adding a header row.
        /// </summary>
        /// <param name="excelFileName"> Name of file to be created. </param>
        /// <returns></returns>
        public async Task CreateFileAsync(string excelFileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(Properties.Resources.WorksheetWord);

                List<string[]> headerRow = new List<string[]>()
                {
                    new string[] { IdWord, Properties.Resources.DateWord, Properties.Resources.FirstNameWord,
                        Properties.Resources.LastNameWord, Properties.Resources.PatronymicWord,
                        Properties.Resources.CityWord, Properties.Resources.CountryWord }
                };

                worksheet.Cells["A1"].LoadFromArrays(headerRow);

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
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage(excelFileName))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault()!;
                int rows = worksheet.Dimension.Rows + 1;

                worksheet.Cells[rows, 1].LoadFromCollection(listOfUsersFromDB);

                worksheet.Columns.AutoFit();

                await excelPackage.SaveAsync();
            }
        }

    }
}
