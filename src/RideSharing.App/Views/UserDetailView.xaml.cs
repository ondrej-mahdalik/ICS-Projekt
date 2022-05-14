using Microsoft.Win32;
using RideSharing.App.ViewModels;

namespace RideSharing.App.Views
{
    /// <summary>
    /// Interakční logika pro UserDetailView.xaml
    /// </summary>
    public partial class UserDetailView
    {
        public UserDetailView()
        {
            InitializeComponent();
        }

        private void UploadImageBtn_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Multiselect = false,
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif"
            };

            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value && dialog.CheckFileExists)
            {
                var vm = DataContext as UserDetailViewModel;
                vm?.ChangeImageCommand.Execute(dialog.FileName);
            }
        }
    }
}
