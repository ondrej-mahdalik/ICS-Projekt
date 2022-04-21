using System;
using System.Windows;
using MaterialDesignThemes.Wpf;
using RideSharing.App;

namespace RideSharing.App.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            webview.IsEnabled = false;
        }

        private void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            webview.ExecuteScriptAsync($"setRoute(\"{TxtFrom.Text}\", \"{TxtTo.Text}\");");
        }
    }
}
