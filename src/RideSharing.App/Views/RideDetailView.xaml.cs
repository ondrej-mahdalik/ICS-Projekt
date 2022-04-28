using System.Windows;

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
    }
}
