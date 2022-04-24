using System.Drawing.Text;
using RideSharing.App.Factories;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.ViewModels.Interfaces;
using RideSharing.App.Wrappers;

namespace RideSharing.App.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IFactory<IDashboardViewModel> _dashboardViewModelFactory;
    private readonly IFactory<IFindRideViewModel> _findRideViewModelFactory;
    private readonly IFactory<ILoginViewModel> _loginViewModelFactory;

    public MainViewModel(
        IDashboardViewModel dashboardViewModel,
        IFindRideViewModel findRideViewModel,
        ILoginViewModel loginViewModel,
        IMediator mediator,
        IFactory<IDashboardViewModel> dashboardViewModelFactory,
        IFactory<IFindRideViewModel> findRideViewModelFactory,
        IFactory<ILoginViewModel> loginLoginViewModelFactory)
    {
        _dashboardViewModelFactory = dashboardViewModelFactory;
        _findRideViewModelFactory = findRideViewModelFactory;
        _loginViewModelFactory = loginLoginViewModelFactory;

        DashboardViewModel = dashboardViewModel;
        FindRideViewModel = findRideViewModel;
        LoginViewModel = loginViewModel;

        // TODO Commands

        mediator.Register<NewMessage<UserWrapper>>(OnNewUserMessage);
    }

    public IDashboardViewModel DashboardViewModel { get; }
    public IFindRideViewModel FindRideViewModel { get; }
    public ILoginViewModel LoginViewModel { get; }

    private void OnNewUserMessage(NewMessage<UserWrapper> _)
    {
        
    }
}
