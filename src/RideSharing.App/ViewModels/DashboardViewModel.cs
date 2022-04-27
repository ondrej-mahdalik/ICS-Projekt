using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core.Raw;
using RideSharing.App.Commands;
using RideSharing.App.Extensions;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.ViewModels.Interfaces;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;

namespace RideSharing.App.ViewModels;

public class DashboardViewModel : ViewModelBase, IDashboardViewModel
{
    private readonly RideFacade _rideFacade;
    private readonly IMediator _mediator;
    private Guid? _loggedUserid;

    public DashboardViewModel(RideFacade rideFacade, IMediator mediator)
    {
        _rideFacade = rideFacade;
        _mediator = mediator;

        ReviewSubmittedCommand = new RelayCommand(ReviewSubmitted);
        RideDetailClickedCommand = new RelayCommand<RideListModel>(RideDetailClicked);

        mediator.Register<UpdateMessage<RideWrapper>>(RideUpdated);
        mediator.Register<DeleteMessage<RideWrapper>>(RideDeleted);
        mediator.Register<SelectedMessage<UserWrapper>>(UserLoggedIn);
    }

    public string? UserName { get; set; } = "User";
    private async void UserLoggedIn(SelectedMessage<UserWrapper> obj)
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

    public ObservableCollection<RideListModel> UpcomingRides { get; set; } = new();
    public ObservableCollection<RideListModel> RecentRides { get; set; } = new();

    public ICommand ReviewSubmittedCommand { get; }
    public ICommand RideDetailClickedCommand { get; } // Same for ride detail and manage buttons

    private async void RideUpdated(UpdateMessage<RideWrapper> _) => await LoadAsync();
    private async void RideDeleted(DeleteMessage<RideWrapper> _) => await LoadAsync();

    private void ReviewSubmitted() => _mediator.Send(new NewMessage<ReviewWrapper>());

    private void RideDetailClicked(RideListModel? rideListModel)
    {
        // TODO choose between Ride Detail and Manage Ride

        if (rideListModel is not null)
            _mediator.Send(new SelectedMessage<RideWrapper> { Id = rideListModel.Id});
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
