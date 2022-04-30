using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RideSharing.App.Commands;
using RideSharing.App.Extensions;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.ViewModels;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.Common.Enums;

namespace RideSharing.App.ViewModels;

public class FindRideViewModel : ViewModelBase, IFindRideViewModel
{
    private readonly RideFacade _rideFacade;
    private readonly IMediator _mediator;
    private Guid? _loggedUserid;


    public FindRideViewModel(RideFacade rideFacade, IMediator mediator)
    {
        _rideFacade = rideFacade;
        _mediator = mediator;

        FindRideCommand = new RelayCommand(FindRides);


        mediator.Register<UpdateMessage<RideWrapper>>(RideUpdated);
        mediator.Register<NewMessage<RideWrapper>>(RideNew);
        mediator.Register<DeleteMessage<RideWrapper>>(RideDeleted);
        mediator.Register<LoginMessage<UserWrapper>>(UserLoggedIn);
        mediator.Register<LogoutMessage<UserWrapper>>(ResetViewModel);

    }

    private void UserLoggedIn(LoginMessage<UserWrapper> obj)
    {
        _loggedUserid = obj.Id;
    }

    public string SelFrom { get; set; } = "";
    public string SelTo { get; set; } = "";
    public DateTime? SelDate { get; set; } = null;
    public TimeSpan? SelTime { get; set; } = null;
    public bool SelDeparture { get; set; } = true;

    private RideSortType _rideSortType = RideSortType.Departure;
    public RideSortType RideOrder
    {
        get => _rideSortType;
        set
        {
            _rideSortType = value;
            _ = LoadAsync();
        }
    }

    public ICommand FindRideCommand { get; }
    private async void FindRides() => await LoadAsync();

    private void ResetViewModel(LogoutMessage<UserWrapper> obj)
    {
        _loggedUserid = null;
        SelFrom = "";
        SelTo = "";
        SelDate = null;
        SelTime = null;
        SelDeparture = true;
        RideOrder = RideSortType.Departure;
    }

    private async void RideUpdated(UpdateMessage<RideWrapper> _) => await LoadAsync();
    private async void RideNew(NewMessage<RideWrapper> _) => await LoadAsync();
    private async void RideDeleted(DeleteMessage<RideWrapper> _) => await LoadAsync();

    public ObservableCollection<RideFoundListModel> FoundRides { get; set; } = new();
    
#pragma warning disable CS1998 // V této asynchronní metodě chybí operátory await a spustí se synchronně.
    public async Task LoadAsync()
#pragma warning restore CS1998 // V této asynchronní metodě chybí operátory await a spustí se synchronně.
    {
        if (_loggedUserid is null ||  String.Equals(SelFrom, ""))
            return;

        DateTime? departTime = null;
        DateTime? arrivalTime = null;
        if (SelDeparture)
        {
            departTime = SelDate + SelTime;
        }
        else
        {
            arrivalTime = SelDate + SelTime;
        }

        FoundRides.Clear();
        var foundRides = await _rideFacade.GetFilteredAsync(_loggedUserid, departTime, arrivalTime, SelFrom, SelTo);
        var foundRidesSorted = RideOrder switch
        {
            RideSortType.Departure => foundRides.OrderBy(x => x.Departure),
            RideSortType.Duration => foundRides.OrderBy(x => x.Duration),
            RideSortType.Distance => foundRides.OrderBy(x => x.Distance),
            RideSortType.Rating => foundRides.OrderBy(x => x.Rating),
            _ => throw new NotImplementedException()
        };
        FoundRides.AddRange(foundRidesSorted);
    }
}
