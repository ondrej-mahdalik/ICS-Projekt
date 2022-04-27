using System;
using System.Drawing.Text;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using RideSharing.App.Factories;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.ViewModels.Interfaces;
using RideSharing.App.Wrappers;

namespace RideSharing.App.ViewModels;

public class MainViewModel : ViewModelBase
{
    public event EventHandler? OnLogout;

    private readonly IFactory<IDashboardViewModel> _dashboardViewModelFactory;
    private readonly IFactory<IFindRideViewModel> _findRideViewModelFactory;

    private Guid? _loggedUserId;

    public MainViewModel(
        IDashboardViewModel dashboardViewModel,
        IFindRideViewModel findRideViewModel,
        IMediator mediator,
        IFactory<IDashboardViewModel> dashboardViewModelFactory,
        IFactory<IFindRideViewModel> findRideViewModelFactory)
    {
        _dashboardViewModelFactory = dashboardViewModelFactory;
        _findRideViewModelFactory = findRideViewModelFactory;

        DashboardViewModel = dashboardViewModel;
        FindRideViewModel = findRideViewModel;

        LogOutCommand = new RelayCommand(LogOut);

        mediator.Register<NewMessage<UserWrapper>>(OnNewUserMessage);
        mediator.Register<SelectedMessage<UserWrapper>>(LoggedIn);
    }

    private void LoggedIn(SelectedMessage<UserWrapper> obj)
    {
        _loggedUserId = obj.Id;
        IsLoggedIn = true;
    }

    public bool IsLoggedIn { get; private set; }

    private void LogOut()
    {
        IsLoggedIn = false;
        OnLogout?.Invoke(this, EventArgs.Empty);
    }

    public IDashboardViewModel DashboardViewModel { get; }
    public IFindRideViewModel FindRideViewModel { get; }

    public ICommand LogOutCommand { get; }

    private void OnNewUserMessage(NewMessage<UserWrapper> _)
    {
        
    }
}
