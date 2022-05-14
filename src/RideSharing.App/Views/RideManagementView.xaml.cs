using System.Threading.Tasks;
using System.Windows;
using Microsoft.Web.WebView2.Core;

namespace RideSharing.App.Views
{
    /// <summary>
    /// Interakční logika pro RideManagementView.xaml
    /// </summary>
    public partial class RideManagementView
    {
        public RideManagementView()
        {
            InitializeComponent();
        }

        private async void MapView_OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MapView.IsEnabled)
                await MapView.ExecuteScriptAsync($"setRoute(\"{FromToBlock.FromName}\", \"{FromToBlock.ToName}\")");
        }

        private async void MapView_OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
                await MapView.ExecuteScriptAsync("document.querySelector('body').style.overflow='hidden'");
        }
    }
}
