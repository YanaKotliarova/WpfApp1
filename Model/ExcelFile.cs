using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.IO;
using WpfApp1.View;

namespace WpfApp1.Model
{
    internal class ExcelFile
    {
        private const string IdWord = "ID";
        private const string DateWord = "Date";
        private const string FirstNameWord = "FirstName";
        private const string LastNameWord = "LastName";
        private const string PatronymicWord = "Patronymic";
        private const string CityWord = "City";
        private const string CountryWord = "Country";
        private const string WorksheetWord = "Лист1";

        UIWorking uiWorking = new UIWorking();

        /// <summary>
        /// Асинхронный метод создания Excel файла.
        /// </summary>
        /// <param name="excelFileName"> Имя создаваемого файла. </param>
        /// <returns></returns>
        public async Task CreateExcelFileAsync(string excelFileName)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Worksheets.Add(WorksheetWord);

                    var headerRow = new List<string[]>() { new string[] { IdWord, DateWord, FirstNameWord, LastNameWord, PatronymicWord, CityWord, CountryWord } };
                    string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                    var worksheet = excelPackage.Workbook.Worksheets[WorksheetWord];
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    FileInfo excelFile = new FileInfo(excelFileName);

                    await excelPackage.SaveAsAsync(excelFile);
                }
            }
            catch (Exception ex)
            {
                uiWorking.ShowMessage("Не удалось создать Excel файл!\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// Асинхронный метод дозаписи данных, полученных из БД в Excel файл.
        /// </summary>
        /// <param name="excelFileName"> Имя Excel файла.</param>
        /// <param name="ListOfUsersFromDB"> Список пользователей для записи в файл. </param>
        /// <returns></returns>
        public async Task AddToExcelFileAsync(string excelFileName, List<User> ListOfUsersFromDB)
        {
            try
            {
                if (ListOfUsersFromDB.IsNullOrEmpty())
                    throw new Exception("Выборка не была осуществена!\r\nПроверьте введённые данные.");

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage(excelFileName))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();

                    int rows = worksheet.Dimension.Rows;

                    List<string[]> dataForFile = new List<string[]>();
                    string[] cellData;

                    foreach (User user in ListOfUsersFromDB)
                    {
                        cellData = new string[7] { user.Id.ToString(), user.Date.ToString(), user.FirstName, user.LastName, user.Patronymic,
                            user.City, user.Country};
                        dataForFile.Add(cellData);
                    }

                    worksheet.Cells[rows + 1, 1].LoadFromArrays(dataForFile);


                    await excelPackage.SaveAsync();
                }
                ListOfUsersFromDB.Clear();
            }
            catch (Exception ex)
            {
                //uiWorking.ShowMessage(ex.Message);                
            }
        }

    }
}
