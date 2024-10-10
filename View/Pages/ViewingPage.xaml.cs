﻿using System.Windows.Controls;
using WpfApp1.ViewModel.ViewModels;

namespace WpfApp1.View.Pages
{
    public partial class ViewingPage : Page
    {
        public ViewingPage()
        {
            InitializeComponent();
            DataContext = new ViewingPageViewModel();
        }
    }
}
