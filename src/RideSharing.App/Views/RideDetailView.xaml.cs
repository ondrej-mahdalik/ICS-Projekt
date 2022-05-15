using System.Windows;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace RideSharing.App.Views
{
    /// <summary>
    /// Interakční logika pro RideDetailView.xaml
    /// </summary>
    public partial class RideDetailView
    {
        public RideDetailView()
        {
            InitializeComponent();
        }

        private async void MapView_OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MapView.IsEnabled)
                await MapView.ExecuteScriptAsync($"setRoute(\"{FromToBlock.FromName}\", \"{FromToBlock.ToName}\")");
        }

        private async void MapView_OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            // Hide scroll bars
            if (e.IsSuccess)
                await MapView.ExecuteScriptAsync("document.querySelector('body').style.overflow='hidden'");
        }
    }
}
