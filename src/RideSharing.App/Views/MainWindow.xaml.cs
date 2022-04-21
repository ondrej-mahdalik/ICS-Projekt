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
            //webview.IsEnabled = false;
        }

        //private void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
        //{
        //    webview.ExecuteScriptAsync($"setRoute(\"{TxtFrom.Text}\", \"{TxtTo.Text}\");");
        //}

            /*<wv2:WebView2 Margin="5px"
            Grid.Row="1" x:Name="webview"
                      Source="https://ondrej-mahdalik.github.io/ridesharing-cdn/EmbedMap/" />*/
}
}
