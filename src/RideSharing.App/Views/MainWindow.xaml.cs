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

        private void Transitioner_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Transitioner.SelectedIndex)
            {
                case 0:
                    MenuBtnHome.IsChecked = true;
                    break;
                case 1:
                    MenuBtnFindRide.IsChecked = true;
                    break;
                case 2:
                    MenuBtnShareRide.IsChecked = true;
                    break;
                case 3:
                    MenuBtnManageVehicles.IsChecked = true;
                    break;
                case 4:
                    MenuProfileSettings.IsChecked = true;
                    break;
            }
        }

        private void RadioButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuBtnHome.IsChecked = true;
        }
    }
}
