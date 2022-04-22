using System.Windows;

namespace RideSharing.App.Views
{
    /// <summary>
    /// Interakční logika pro MapTestView.xaml
    /// </summary>
    public partial class MapTestView
    {
        public MapTestView()
        {
            InitializeComponent();
        }

        private async void BtnCalcRoute_OnClick(object sender, RoutedEventArgs e)
        {
            await WebView.ExecuteScriptAsync($"setRoute(\"{TxtFrom.Text}\", \"{TxtTo.Text}\");");
        }
    }
}
