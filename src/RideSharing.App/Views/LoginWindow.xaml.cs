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
    }
}
