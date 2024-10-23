﻿using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WpfApp1.Data.Database.Interfaces;
using WpfApp1.Model;
using WpfApp1.View.UI.Interfaces;
using WpfApp1.ViewModel.DependencyInjection;
using WpfApp1.ViewModel.Factories.Interfaces;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1.View.Pages
{
    public partial class MenuPage : Page
    {
        private IAbstractFactory<IRepository<User>> _repositoryFactory =
            App.serviceProvider.GetService<IAbstractFactory<IRepository<User>>>()!;

        private IAbstractFactory<IMessage> _messageFactory =
            App.serviceProvider.GetService<IAbstractFactory<IMessage>>()!;

        public MenuPage()
        {
            DependencyStruct dependencyStruct = new DependencyStruct(_repositoryFactory, _messageFactory);

            InitializeComponent();
            DataContext = new MenuPageViewModel(dependencyStruct);
        }
    }
}
