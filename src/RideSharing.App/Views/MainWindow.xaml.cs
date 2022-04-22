using System.Windows;
using System.Windows.Controls;
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
            if ((Button)sender == BtnMenuHome)
                Transitioner.SelectedIndex = 0;
            else if ((Button)sender == BtnMenuFindRide)
                Transitioner.SelectedIndex = 1;
        }
    }
}
