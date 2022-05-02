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
    private Guid? _loggedUserid;

    public DashboardViewModel(RideFacade rideFacade, ReviewFacade reviewFacade, IMediator mediator)
    {
        _rideFacade = rideFacade;
        _reviewFacade = reviewFacade;
        _mediator = mediator;

        ReviewSubmittedCommand = new RelayCommand<RideRecentListModel>(ReviewSubmitted);
        UpcomingRideDetailClickedCommand = new RelayCommand<RideUpcomingListModel>(UpcomingRideDetailClicked);
        RecentRideDetailClickedCommand = new RelayCommand<RideRecentListModel>(RecentRideDetailClicked);

        mediator.Register<UpdateMessage<RideWrapper>>(RideUpdated);
        mediator.Register<DeleteMessage<RideWrapper>>(RideDeleted);
        mediator.Register<LoginMessage<UserWrapper>>(UserLoggedIn);
        mediator.Register<LogoutMessage<UserWrapper>>(ResetViewModel);
    }

    public string? UserName { get; set; } = "User";
    private async void UserLoggedIn(LoginMessage<UserWrapper> obj)
    {
        _loggedUserid = obj.Id;
        if (obj.Model is not null)
        {
            UserName = obj.Model.Name;
            await LoadAsync();
        }
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
    public ICommand RecentRideDetailClickedCommand { get; }


    private async void RideUpdated(UpdateMessage<RideWrapper> _) => await LoadAsync();
    private async void RideDeleted(DeleteMessage<RideWrapper> _) => await LoadAsync();

    private async void ReviewSubmitted(RideRecentListModel? rideListModel)
    {
        if (rideListModel is null) 
            return;

        RecentRides.FirstOrDefault(rideListModel).HasReviewed = true;

        var review = new ReviewDetailModel(rideListModel.Id, _loggedUserid, rideListModel.UserRating);
        await _reviewFacade.SaveAsync(review);

        await LoadAsync();
    }

    private void UpcomingRideDetailClicked(RideUpcomingListModel? rideListModel)
    {
        if (rideListModel is not null)
            _mediator.Send(new SelectedMessage<RideWrapper> { Id = rideListModel.Id});
    }
    private void RecentRideDetailClicked(RideRecentListModel? rideListModel)
    {
        if (rideListModel is not null)
            _mediator.Send(new SelectedMessage<RideWrapper> { Id = rideListModel.Id });
    }

    private void ResetViewModel(LogoutMessage<UserWrapper> obj)
    {
        _loggedUserid = null;
        RecentDriverFilter = false;
        RecentPassengerFilter = false;
        UpcomingDriverFilter = false;
        UpcomingPassengerFilter = false;
    }

    public async Task LoadAsync()
    {
        if (_loggedUserid is null)
            return;

        UpcomingRides.Clear();
        var upcomingRides = await _rideFacade.GetUserUpcomingRidesAsync(_loggedUserid.Value);
        UpcomingRides.AddRange(upcomingRides);

        RecentRides.Clear();
        var recentRides = await _rideFacade.GetUserRecentRidesAsync(_loggedUserid.Value);
        RecentRides.AddRange(recentRides);
    }

    public async Task LoadUpcomingFilteredRides()
    {
        if (_loggedUserid is null)
            return;

        UpcomingRides.Clear();
        var upcomingRides = await _rideFacade.GetUserUpcomingRidesAsync(_loggedUserid.Value, _upcomingDriverFilter, _upcomingPassengerFilter);
        UpcomingRides.AddRange(upcomingRides);
    }

    public async Task LoadRecentFilteredRides()
    {
        if (_loggedUserid is null)
            return;

        RecentRides.Clear();
        var recentRides = await _rideFacade.GetUserRecentRidesAsync(_loggedUserid.Value, _recentDriverFilter, _recentPassengerFilter);
        RecentRides.AddRange(recentRides);
    }
}
