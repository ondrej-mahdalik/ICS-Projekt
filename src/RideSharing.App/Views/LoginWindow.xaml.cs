using RideSharing.App.ViewModels;

namespace RideSharing.App.Views
{
    /// <summary>
    /// Interakční logika pro LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {

        public LoginWindow(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            DataContext = loginViewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }
    }
}
