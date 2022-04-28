using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Extensions;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.ViewModels;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;

namespace RideSharing.App.ViewModels;

public class FindRideViewModel : ViewModelBase, IFindRideViewModel
{
    private readonly RideFacade _rideFacade;
    private readonly IMediator _mediator;

    public FindRideViewModel(RideFacade rideFacade, IMediator mediator)
    {
        _rideFacade = rideFacade;
        _mediator = mediator;

        FindRideCommand = new RelayCommand(FindRides);


        mediator.Register<UpdateMessage<RideWrapper>>(RideUpdated);
        mediator.Register<NewMessage<RideWrapper>>(RideNew);
        mediator.Register<DeleteMessage<RideWrapper>>(RideDeleted);
    }

    public string SelFrom { get; set; } = "";
    public string SelTo { get; set; } = "";
    public DateTime? SelDate { get; set; } = null;
    public TimeSpan? SelTime { get; set; } = null;
    public bool SelDeparture { get; set; } = true;
    public int SelSeats { get; set; } = 1;



    public ICommand FindRideCommand { get; }
    private async void FindRides() => await LoadAsync();

    private async void RideUpdated(UpdateMessage<RideWrapper> _) => await LoadAsync();
    private async void RideNew(NewMessage<RideWrapper> _) => await LoadAsync();
    private async void RideDeleted(DeleteMessage<RideWrapper> _) => await LoadAsync();

    public ObservableCollection<FoundRideModel> FoundRides { get; set; } = new();
    
#pragma warning disable CS1998 // V této asynchronní metodě chybí operátory await a spustí se synchronně.
    public async Task LoadAsync()
#pragma warning restore CS1998 // V této asynchronní metodě chybí operátory await a spustí se synchronně.
    {
        DateTime? deprartTime = null;
        DateTime? arrivalTime = null;
        if (SelDeparture)
        {
            deprartTime = SelDate + SelTime;
        }
        else
        {
            arrivalTime = SelDate + SelTime;
        }

        FoundRides.Clear();
        var foundRides = await _rideFacade.GetFilteredAsync(deprartTime, arrivalTime, SelFrom, SelTo, SelSeats);
       FoundRides.AddRange(foundRides);

    }
}
