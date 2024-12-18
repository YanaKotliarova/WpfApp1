﻿using System.IO;
using WpfApp1.Model;
using WpfApp1.MVVM;
using WpfApp1.Services.Import;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.Factories;

namespace WpfApp1.ViewModel.ViewModels
{
    class ViewFilePageViewModel : ViewPageViewModel
    {
        private const string ExcelImporterName = "ExcelImporter";
        private const string XmlImporterName = "XmlImporter";

        private const string XmlExtension = ".xml";
        private const string ExcelExtension = ".xlsx";

        private readonly IImporterFactory _importer;
        private readonly IMetroDialog _metroDialog;
        private readonly IFileDialog _fileDialog;
        public ViewFilePageViewModel(IImporterFactory importer, IMetroDialog metroDialog, IFileDialog fileDialog)
        {
            _importer = importer;
            _metroDialog = metroDialog;
            _fileDialog = fileDialog;
        }

        /// <summary>
        /// The method for getting data from created excel or xml file for viewing.
        /// </summary>
        /// <returns></returns>
        public async override Task GetData()
        {
            try
            {
                var fileImporter = GetDataImporter();
                var metroDialogController = await _metroDialog.ShowMessageWithProgressBar(this,
                                    Properties.Resources.HeaderWaitPlease, Properties.Resources.DataIsLoading);
                await Task.Run(async () =>
                {
                    await foreach (List<User> listOfUsers in fileImporter.ReadFromFileAsync(FileName))
                    {
                        ListOfUsersForViewing.AddRange(listOfUsers);
                    }
                });
                await _metroDialog.CloseMessageWithProgressBar(metroDialogController);
            }
            catch (Exception)
            {
                await _metroDialog.ShowMessage(Properties.Resources.HeadetOpenFileEx,
                    Properties.Resources.OpenFileForViewEx);
            }
        }

        /// <summary>
        /// The method for getting needed type of file importer.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IDataImporter GetDataImporter()
        {
            if (Path.GetExtension(FileName).Equals(ExcelExtension))
            {
                var fileImporter = _importer.GetImporter(ExcelImporterName);
                return fileImporter;
            }
            else if (Path.GetExtension(FileName).Equals(XmlExtension))
            {
                var fileImporter = _importer.GetImporter(XmlImporterName);
                return fileImporter;
            }
            else throw new Exception(Properties.Resources.ExWrongFileFormat);
        }

        private RelayCommand _openFileCommand;
        /// <summary>
        /// The command associated with the button to open created excel or xml file.
        /// </summary>
        public RelayCommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ??
                    (_openFileCommand = new RelayCommand(async obj =>
                    {
                        try
                        {
                            if (_fileDialog.OpenFileDialog(out string fileName, Properties.Resources.OpenExcelXmlFileExtensionFilter))
                            {
                                ListOfUsersForViewing.Clear();
                                FileName = fileName;
                                ShowSelectionCommand.Execute(obj);
                            }
                        }
                        catch (Exception ex)
                        {
                            await _metroDialog.ShowMessage(Properties.Resources.HeadetOpenFileEx, 
                                Properties.Resources.ChooseFileEx);
                        }
                    }
                    ));
            }
        }

        private RelayCommand _pageIsLoadedCommand;
        /// <summary>
        /// The command which is called when page is loaded and made file name text box empty.
        /// </summary>
        public RelayCommand PageIsLoadedCommand
        {
            get
            {
                return _pageIsLoadedCommand ??
                    (_pageIsLoadedCommand = new RelayCommand(obj =>
                    {
                        FileName = "";
                    }));
            }
        }

        private string _fileNameTextBoxText;
        /// <summary>
        /// A property associated with the text field used to display information.
        /// </summary>
        public string FileName
        {
            get { return _fileNameTextBoxText; }
            set
            {
                _fileNameTextBoxText = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
    }
}
