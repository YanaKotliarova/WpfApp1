using OfficeOpenXml;
using System.IO;
using WpfApp1.Model;

namespace WpfApp1.Services.Export
{
    internal class ExcelExporter : IDataExporter
    {
        private const string WorksheetWord = "Лист1";

        public string ExporterName { get; set; } = "ExcelExporter";

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
                int rows = 1;
                bool IsHeaderNeeded = true;
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault()!;

                if (worksheet.Dimension != null)
                {
                    rows = worksheet.Dimension.Rows + 1;
                    IsHeaderNeeded = false;
                }

                worksheet.Cells[rows, 1].LoadFromCollection(listOfUsersFromDB, IsHeaderNeeded);

                worksheet.Columns.AutoFit();

                await excelPackage.SaveAsync();
            }
        }

    }
}
