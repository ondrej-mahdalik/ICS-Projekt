using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Extensions;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;

namespace RideSharing.App.ViewModels;

public class DashboardViewModel : ViewModelBase, IDashboardViewModel
{
    private readonly RideFacade _rideFacade;
    private readonly ReviewFacade _reviewFacade;
    private readonly IMediator _mediator;

    public DashboardViewModel(RideFacade rideFacade, ReviewFacade reviewFacade, IMediator mediator) : base(mediator)
    {
        _rideFacade = rideFacade;
        _reviewFacade = reviewFacade;
        _mediator = mediator;

        ReviewSubmittedCommand = new RelayCommand<RideRecentListModel>(ReviewSubmitted);
        UpcomingRideDetailClickedCommand = new RelayCommand<RideUpcomingListModel>(UpcomingRideDetailClicked);

        mediator.Register<UpdateMessage<RideWrapper>>(RideUpdated);
        mediator.Register<DeleteMessage<RideWrapper>>(RideDeleted);
    }

    public string? UserName { get; set; } = "User";
    public override async void UserLoggedIn(LoginMessage<UserWrapper> obj)
    {
        base.UserLoggedIn(obj);

        if (obj.Model is not null)
        {
            UserName = obj.Model.Name;
            await LoadAsync();
        }
    }

    public override void UserLoggedOut(LogoutMessage<UserWrapper> obj)
    {
        base.UserLoggedOut(obj);

        RecentDriverFilter = false;
        RecentPassengerFilter = false;
        UpcomingDriverFilter = false;
        UpcomingPassengerFilter = false;
    }

    private bool _upcomingDriverFilter = false;
    public bool UpcomingDriverFilter
    {
        get => _upcomingDriverFilter;
        set
        {
            _upcomingDriverFilter = value;
            _ = LoadUpcomingFilteredRides();
        }
    }

    private bool _upcomingPassengerFilter = false;
    public bool UpcomingPassengerFilter
    {
        get => _upcomingPassengerFilter;
        set
        {
            _upcomingPassengerFilter = value;
            _ = LoadUpcomingFilteredRides();
        }
    }

    private bool _recentDriverFilter = false;
    public bool RecentDriverFilter
    {
        get => _recentDriverFilter;
        set
        {
            _recentDriverFilter = value;
            _ = LoadRecentFilteredRides();
        }
    }

    private bool _recentPassengerFilter = false;
    public bool RecentPassengerFilter
    {
        get => _recentPassengerFilter;
        set
        {
            _recentPassengerFilter = value;
            _ = LoadRecentFilteredRides();
        }
    }

    public ObservableCollection<RideUpcomingListModel> UpcomingRides { get; set; } = new();
    public ObservableCollection<RideRecentListModel> RecentRides { get; set; } = new();

    public ICommand ReviewSubmittedCommand { get; }
    public ICommand UpcomingRideDetailClickedCommand { get; }

    private async void RideUpdated(UpdateMessage<RideWrapper> _) => await LoadAsync();
    private async void RideDeleted(DeleteMessage<RideWrapper> _) => await LoadAsync();

    private async void ReviewSubmitted(RideRecentListModel? rideListModel)
    {
        if (rideListModel is null || LoggedUser is null) 
            return;

        RecentRides.FirstOrDefault(rideListModel).CanReview = false;

        var review = new ReviewDetailModel(rideListModel.Id, LoggedUser.Id, rideListModel.UserRating);
        await _reviewFacade.SaveAsync(review);

        await LoadAsync();
    }

    private void UpcomingRideDetailClicked(RideUpcomingListModel? rideListModel)
    {
        if (rideListModel is null)
            return;

        if (rideListModel.IsDriver)
            _mediator.Send(new ManageMessage<RideWrapper> { Id = rideListModel.Id});
        else
            _mediator.Send(new DetailMessage<RideWrapper> { Id = rideListModel.Id});
    }

    public async Task LoadAsync()
    {
        if (LoggedUser is null)
            return;

        UpcomingRides.Clear();
        var upcomingRides = await _rideFacade.GetUserUpcomingRidesAsync(LoggedUser.Id);
        UpcomingRides.AddRange(upcomingRides);

        RecentRides.Clear();
        var recentRides = await _rideFacade.GetUserRecentRidesAsync(LoggedUser.Id);
        RecentRides.AddRange(recentRides);
    }

    public async Task LoadUpcomingFilteredRides()
    {
        if (LoggedUser is null)
            return;

        UpcomingRides.Clear();
        var upcomingRides = await _rideFacade.GetUserUpcomingRidesAsync(LoggedUser.Id, _upcomingDriverFilter, _upcomingPassengerFilter);
        UpcomingRides.AddRange(upcomingRides);
    }

    public async Task LoadRecentFilteredRides()
    {
        if (LoggedUser is null)
            return;

        RecentRides.Clear();
        var recentRides = await _rideFacade.GetUserRecentRidesAsync(LoggedUser.Id, _recentDriverFilter, _recentPassengerFilter);
        RecentRides.AddRange(recentRides);
    }
}
