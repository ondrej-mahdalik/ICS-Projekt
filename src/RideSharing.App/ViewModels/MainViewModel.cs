using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Wrappers;
using RideSharing.Common.Enums;

namespace RideSharing.App.ViewModels;

public class MainViewModel : ViewModelBase
{
    public event EventHandler? OnLogout;
    private readonly IMediator _mediator;

    public MainViewModel(
        IDashboardViewModel dashboardViewModel,
        IFindRideViewModel findRideViewModel,
        IShareRideViewModel shareRideViewModel,
        IRideDetailViewModel rideDetailViewModel,
        IRideManagementViewModel rideManagementViewModel,
        IUserDetailViewModel userDetailViewModel,
        IVehicleDetailViewModel vehicleDetailViewModel,
        IVehicleListViewModel vehicleListViewModel,
        IMediator mediator) : base(mediator)
    {

        DashboardViewModel = dashboardViewModel;
        FindRideViewModel = findRideViewModel;
        ShareRideViewModel = shareRideViewModel;
        RideDetailViewModel = rideDetailViewModel;
        RideManagementViewModel = rideManagementViewModel;
        UserDetailViewModel = userDetailViewModel;
        VehicleDetailViewModel = vehicleDetailViewModel;
        VehicleListViewModel = vehicleListViewModel;

        LogOutCommand = new RelayCommand(LogOut);
        MenuTabCommand = new RelayCommand<string>(MenuTab);

        _mediator = mediator;
        mediator.Register<LoginMessage<UserWrapper>>(LoggedIn);
        mediator.Register<LogoutMessage<UserWrapper>>(LoggedOut);

        // Switch tab messages
        mediator.Register<SwitchTabMessage>(SwitchTab);
    }

    private void SwitchTab(SwitchTabMessage obj)
    {
        TransitionerSelectedIndex = obj.index;
    }

    private void MenuTab(string? selectedIndex)
    {
        TransitionerSelectedIndex = Enum.TryParse(selectedIndex, out ViewIndex result) ? result : ViewIndex.Dashboard;
    }

    private void LoggedIn(LoginMessage<UserWrapper> obj)
    {
        TransitionerSelectedIndex = 0;
        IsLoggedIn = true;
    }

    private void LoggedOut(LogoutMessage<UserWrapper> obj) => LogOut();

    public bool IsLoggedIn { get; private set; }

    private void LogOut()
    {
        IsLoggedIn = false;
        TransitionerSelectedIndex = 0;
        OnLogout?.Invoke(this, EventArgs.Empty);
    }

    public IDashboardViewModel DashboardViewModel { get; }
    public IFindRideViewModel FindRideViewModel { get; }
    public IShareRideViewModel ShareRideViewModel { get; }
    public IRideDetailViewModel RideDetailViewModel { get; }
    public IRideManagementViewModel RideManagementViewModel { get; }
    public IUserDetailViewModel UserDetailViewModel { get; }
    public IVehicleDetailViewModel VehicleDetailViewModel { get; }
    public IVehicleListViewModel VehicleListViewModel { get; }


    public ICommand LogOutCommand { get; }
    public ICommand MenuTabCommand { get; }

    /// <summary>
    /// 0 - Home
    /// 1 - Find ride
    /// 2 - Share ride
    /// 3 - Manage vehicles
    /// 4 - Profile settings
    /// 5 - Ride detail (non-driver)
    /// 6 - Rude detail (driver)
    /// 7 - Vehicle detail
    /// </summary>
    public ViewIndex TransitionerSelectedIndex { get; set; }
}
