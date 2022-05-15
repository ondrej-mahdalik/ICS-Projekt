using System;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Wrappers;
using RideSharing.Common.Enums;

namespace RideSharing.App.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IMediator _mediator;

    public event EventHandler? OnLogin;

    public LoginViewModel(
        ISelectUserViewModel selectUserViewModel,
        IAddUserViewModel addUserViewModel,
        IMediator mediator,
        ISnackbarMessageQueue messageQueue) : base(mediator)
    {

        SelectUserViewModel = selectUserViewModel;
        AddUserViewModel = addUserViewModel;

        _mediator = mediator;

        // Switch tab messages
        mediator.Register<SwitchTabLoginMessage>(SwitchTab);
        mediator.Register<LoginMessage<UserWrapper>>(_ => OnLogin?.Invoke(this, EventArgs.Empty));

        MessageQueue = messageQueue;
    }

    public ISnackbarMessageQueue MessageQueue { get; set; }

    private void SwitchTab(SwitchTabLoginMessage obj)
    {
        TransitionerSelectedIndex = obj.index;
    }

    private void LoggedIn(LoginMessage<UserWrapper> obj)
    {
        TransitionerSelectedIndex = 0;
    }

    public ISelectUserViewModel SelectUserViewModel { get; }
    public IAddUserViewModel AddUserViewModel { get; }
    public LoginViewIndex TransitionerSelectedIndex { get; set; }
}
