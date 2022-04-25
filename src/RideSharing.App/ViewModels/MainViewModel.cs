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

        // TODO Commands

        mediator.Register<NewMessage<UserWrapper>>(OnNewUserMessage);
    }

    public IDashboardViewModel DashboardViewModel { get; }
    public IFindRideViewModel FindRideViewModel { get; }

    private void OnNewUserMessage(NewMessage<UserWrapper> _)
    {
        
    }
}
