using OfficeOpenXml;
using System.IO;
using WpfApp1.Model;

namespace WpfApp1.Services.Import
{
    class ExcelImporter : IDataImporter
    {
        private const int AmountOfUsersToRead = 10000;

        public string ImporterName { get; set; } = "ExcelImporter";

        /// <summary>
        /// The method for readind created excel file.
        /// </summary>
        /// <param name="fileName"> Name of file for reading. </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async IAsyncEnumerable<List<User>> ReadFromFileAsync(string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo existingFile = new FileInfo(fileName);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault()!;

                if (worksheet != null && worksheet.Dimension != null)
                {
                    int lastCollumn = worksheet.Dimension.End.Column;
                    int lastRow = worksheet.Dimension.End.Row;

                    int currentRow = 1;
                    int rowToReadTo = 0;

                    while (currentRow < lastRow)
                    {
                        rowToReadTo = currentRow + AmountOfUsersToRead > lastRow ? lastRow : currentRow + AmountOfUsersToRead;

                        List<User> users = worksheet.Cells
                        [$"{worksheet.Cells[currentRow, 1]}:{worksheet.Cells[rowToReadTo, lastCollumn]}"]
                        .ToCollectionWithMappings<User>(row =>
                        {
                            PersonInfoStruct personInfo = new PersonInfoStruct(
                                row.GetValue<string>(2), row.GetValue<string>(3), row.GetValue<string>(4));
                            EntranceInfoStruct entranceInfo = new EntranceInfoStruct(
                                DateOnly.FromDateTime(row.GetValue<DateTime>(1)), row.GetValue<string>(5), row.GetValue<string>(6));

                            User user = new User(personInfo, entranceInfo);
                            return user;
                        }, options => options.HeaderRow = 0);

                        currentRow += AmountOfUsersToRead;

                        if (users.Count >= AmountOfUsersToRead || currentRow > lastRow)
                        {
                            yield return users;
                            users.Clear();
                        }
                    }
                }
                else throw new Exception(Properties.Resources.ExExcelFileIsEmpty);
            }
        }
    }
}
