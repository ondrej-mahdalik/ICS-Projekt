using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace RideSharing.App.Services.Dialogs;

public class MessageDialog
{
    public MessageDialog(string title, string message, DialogType dialogType)
    {
        Title = title;
        Message = message;
        DialogType = dialogType;

        InitButtons();
    }

    public string Title { get; set; }
    public string Message { get; set; }
    public DialogType DialogType { get; set; }

    public Button[] Buttons { get; set; }

    private void InitButtons()
    {
        Buttons = DialogType switch
        {
            DialogType.OK => new[] { GetButton("OK", ButtonType.OK) },
            DialogType.YesNo => new[] { GetButton("Yes", ButtonType.Yes), GetButton("No", ButtonType.No) },
            _ => Buttons
        };
    }

    private Button GetButton(string text, ButtonType buttonType)
    {
        return new Button()
        {
            Content = text, Command = DialogHost.CloseDialogCommand, CommandParameter = buttonType
        };
    }
}
