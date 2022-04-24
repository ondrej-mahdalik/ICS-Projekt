using System.Windows;
using RideSharing.App.Services.MessageDialog;

namespace RideSharing.App.Services.MessageDialog
{
    public class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult Show(
            string title, 
            string caption, 
            MessageDialogButtonConfiguration buttonConfiguration, 
            MessageDialogResult defaultResult)
        {
            var messageDialog = new RideSharing.App.Services.MessageDialog.MessageDialog(title, caption, defaultResult, buttonConfiguration)
            {
                Owner = Application.Current.MainWindow
            };
            return messageDialog.ShowDialog();
        }
    }
}
