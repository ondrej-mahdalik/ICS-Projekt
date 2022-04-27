﻿using System.Windows;
using RideSharing.App.ViewModels;

namespace RideSharing.App.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }

        private void MenuButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(BtnMenuHome))
                Transitioner.SelectedIndex = 0;
            else if (sender.Equals(BtnMenuFindRide))
                Transitioner.SelectedIndex = 1;
            else if (sender.Equals(BtnMenuShareRide))
                Transitioner.SelectedIndex = 2;
            else if (sender.Equals(BtnMenuVehicles))
                Transitioner.SelectedIndex = 3;
            // TODO Add remaining slides
        }

    }
}
