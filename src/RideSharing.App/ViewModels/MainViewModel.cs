using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using RideSharing.App.Factories;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Wrappers;

namespace RideSharing.App.ViewModels;

public class MainViewModel : ViewModelBase
{
    public event EventHandler? OnLogout;
    private readonly IMediator _mediator;


    private readonly IFactory<IDashboardViewModel> _dashboardViewModelFactory;
    private readonly IFactory<IFindRideViewModel> _findRideViewModelFactory;

    private Guid? _loggedUserId;

    public MainViewModel(
        IDashboardViewModel dashboardViewModel,
        IFindRideViewModel findRideViewModel,
        IShareRideViewModel shareRideViewModel,
        IRideDetailViewModel rideDetailViewModel,
        IRideManagementViewModel rideManagementViewModel,
        IUserDetailViewModel userDetailViewModel,
        IVehicleDetailViewModel vehicleDetailViewModel,
        IVehicleListViewModel vehicleListViewModel,
        IMediator mediator,
        IFactory<IDashboardViewModel> dashboardViewModelFactory,
        IFactory<IFindRideViewModel> findRideViewModelFactory) : base(mediator)
    {
        _dashboardViewModelFactory = dashboardViewModelFactory;
        _findRideViewModelFactory = findRideViewModelFactory;

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
        mediator.Register<NewMessage<UserWrapper>>(OnNewUserMessage);
        mediator.Register<LoginMessage<UserWrapper>>(LoggedIn);
        mediator.Register<LogoutMessage<UserWrapper>>(LoggedOut);

    }

    private void MenuTab(string? selectedIndex)
    {
        TransitionerSelectedIndex = selectedIndex ?? "0";
    }

    private void LoggedIn(LoginMessage<UserWrapper> obj)
    {
        _loggedUserId = obj.Id;
        TransitionerSelectedIndex = "0";
            IsLoggedIn = true;
    }

    private void LoggedOut(LogoutMessage<UserWrapper> obj) => LogOut();

    public bool IsLoggedIn { get; private set; }

    private void LogOut()
    {
        IsLoggedIn = false;
        TransitionerSelectedIndex = "0";
        _loggedUserId = null;
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
    /// </summary>
    public string TransitionerSelectedIndex { get; set; } = "0";

    private void OnNewUserMessage(NewMessage<UserWrapper> _)
    {
        
    }
}
