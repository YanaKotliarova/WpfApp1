﻿using WpfApp1.Model;

namespace WpfApp1.Services.Export
{
    internal interface IDataExporter
    {
        string ExporterName { get; set; }
        Task AddToFileAsync(string excelFileName, List<User> ListOfUsersFromDB);
        Task CreateFileAsync(string excelFileName);
    }
}