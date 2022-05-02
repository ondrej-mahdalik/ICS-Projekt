using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Wrappers;

namespace RideSharing.App.ViewModels;

public abstract class ViewModelBase : IViewModel, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public UserWrapper? LoggedUser;

    protected ViewModelBase(IMediator? mediator)
    {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            LoadInDesignMode();
        }

        if (mediator is not null)
        {
            mediator.Register<LoginMessage<UserWrapper>>(UserLoggedIn);
            mediator.Register<LogoutMessage<UserWrapper>>(UserLoggedOut);
        }
    }

    public virtual void UserLoggedOut(LogoutMessage<UserWrapper> obj)
    {
        LoggedUser = null;
    }

    public virtual void UserLoggedIn(LoginMessage<UserWrapper> obj)
    {
        LoggedUser = obj.Model;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public virtual void LoadInDesignMode() { }
}
