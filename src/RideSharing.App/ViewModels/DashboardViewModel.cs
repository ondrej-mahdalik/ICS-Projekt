using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
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

    public DashboardViewModel(RideFacade rideFacade, IMediator mediator)
    {
        _rideFacade = rideFacade;
        _mediator = mediator;

        ReviewSubmittedCommand = new RelayCommand(ReviewSubmitted);
        RideDetailClickedCommand = new RelayCommand<RideListModel>(RideDetailClicked);

        mediator.Register<UpdateMessage<RideWrapper>>(RideUpdated);
        mediator.Register<DeleteMessage<RideWrapper>>(RideDeleted);
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


#pragma warning disable CS1998 // V této asynchronní metodě chybí operátory await a spustí se synchronně.
    public async Task LoadAsync()
#pragma warning restore CS1998 // V této asynchronní metodě chybí operátory await a spustí se synchronně.
    {
        throw new NotImplementedException();
    }
}
