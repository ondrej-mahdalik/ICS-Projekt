using System;
using System.Threading.Tasks;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.ViewModels.Interfaces;
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

       // ReviewSubmittedCommand = new RelayCommand(ReviewSubmitted);
        //RideDetailClickedCommand = new RelayCommand<RideListModel>(RideDetailClicked);

        mediator.Register<UpdateMessage<RideWrapper>>(FindRide);
    }

    public string SelFrom { get; set; } = "";
    public string SelTo { get; set; } = "";
    public DateTime? SelDate { get; set; } = null;
    public TimeSpan? SelTime { get; set; } = null;
    public bool SelDeparture { get; set; } = true;




    private async void FindRide(UpdateMessage<RideWrapper> _) => await LoadAsync();


#pragma warning disable CS1998 // V této asynchronní metodě chybí operátory await a spustí se synchronně.
    public async Task LoadAsync()
#pragma warning restore CS1998 // V této asynchronní metodě chybí operátory await a spustí se synchronně.
    {
       
    }
}
