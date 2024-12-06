using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.View.UI.Interfaces;

namespace WpfApp1.ViewModel.ViewModels
{
    class ViewSelectionPageViewModel : ViewPageViewModel
    {
        private readonly IRepository<User> _repository;
        private readonly IMetroDialog _metroDialog;
        public ViewSelectionPageViewModel(IRepository<User> repository, IMetroDialog metroDialog)
        {
            _repository = repository;
            _metroDialog = metroDialog;
        }

        /// <summary>
        /// The method for getting selection from DB for viewing.
        /// </summary>
        /// <returns></returns>
        public async override Task GetData()
        {
            try
            {
                var metroDialogController = await _metroDialog.ShowMessageWithProgressBar(this,
                                    "Пожалуйста подождите!", "Данные подгружаются...");
                await Task.Run(async () =>
                {
                    await foreach (List<User> listOfUsers in
                _repository.GetSelectionFromDBAsync(_repository.PersonInfo!.Value, _repository.EntranceInfo!.Value))
                    {
                        ListOfUsersForViewing.AddRange(listOfUsers);
                    }
                });
                await _metroDialog.CloseMessageWithProgressBar(metroDialogController);
            }
            catch (Exception ex)
            {
                await _metroDialog.ShowMessage(this, "При создании выборки произошла непредвиденная ошибка", "Попробуйте снова");
            }
        }
    }
}
