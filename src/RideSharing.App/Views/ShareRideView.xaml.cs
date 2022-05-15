using System.Windows;
using Microsoft.Web.WebView2.Core;
using RideSharing.App.ViewModels;

namespace RideSharing.App.Views
{
    /// <summary>
    /// Interakční logika pro ShareRideView.xaml
    /// </summary>
    public partial class ShareRideView
    {
        public ShareRideView()
        {
            InitializeComponent();
        }

        private async void MapView_OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            // Hide scroll bars
            if (e.IsSuccess)
                await MapView.ExecuteScriptAsync("document.querySelector('body').style.overflow='hidden'");
        }
        
        private async void Textbox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (FromTextbox.Text != string.Empty && ToTextbox.Text != string.Empty)
            {
                await MapView.ExecuteScriptAsync($"setRoute(\"{FromTextbox.Text}\", \"{ToTextbox.Text}\")");
                var viewModel = (ShareRideViewModel)DataContext;
                if (viewModel.UpdateRouteCommand.CanExecute(false))
                    viewModel.UpdateRouteCommand.Execute(false);
            }
        }

        private void MapView_OnEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MapView.IsEnabled)
                MapView.Reload();
        }
    }
}
