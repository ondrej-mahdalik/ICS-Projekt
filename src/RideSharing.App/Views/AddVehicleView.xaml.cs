using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using RideSharing.App.ViewModels;

namespace RideSharing.App.Views
{
    /// <summary>
    /// Interakční logika pro AddVehicleView.xaml
    /// </summary>
    public partial class AddVehicleView
    {
        public AddVehicleView()
        {
            InitializeComponent();
        }

        private void UploadImageBtn_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Multiselect = false,
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif"
            };

            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value && dialog.CheckFileExists)
            {
                var vm = DataContext as AddVehicleViewModel;
                vm?.ChangeImageCommand.Execute(dialog.FileName);
            }
        }

        private void SeatsTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
