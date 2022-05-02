using System.Windows;
using MaterialDesignThemes.Wpf;
using RideSharing.App.ViewModels;
using RideSharing.BL.Models;

namespace RideSharing.App.Views
{
    /// <summary>
    /// Interakční logika pro DashboardView.xaml
    /// </summary>
    public partial class DashboardView
    {
        public DashboardView()
        {
            InitializeComponent();
        }

        private void RatingBar_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            var ratingBar = (RatingBar)sender;
            var ride = (RideRecentListModel)ratingBar.Tag;
            var viewModel = (DashboardViewModel)DataContext;
            if (viewModel.ReviewSubmittedCommand.CanExecute(ride))
            {
                viewModel.ReviewSubmittedCommand.Execute(ride);
            }
        }
    }
}
