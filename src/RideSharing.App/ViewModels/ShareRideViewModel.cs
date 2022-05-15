using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.Dialogs;
using RideSharing.App.Wrappers;
using RideSharing.BL;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.Common.Enums;
using AsyncRelayCommand = RideSharing.App.Commands.AsyncRelayCommand;

namespace RideSharing.App.ViewModels
{
    public class ShareRideViewModel : ViewModelBase, IShareRideViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly VehicleFacade _vehicleFacade;
        private readonly ReservationFacade _reservationFacade;
        private readonly IMediator _mediator;
        private readonly ISnackbarMessageQueue _messageQueue;

        public ShareRideViewModel(
            RideFacade rideFacade,
            VehicleFacade vehicleFacade,
            ReservationFacade reservationFacade,
            IMediator mediator,
            ISnackbarMessageQueue messageQueue) : base(mediator)
        {
            _rideFacade = rideFacade;
            _vehicleFacade = vehicleFacade;
            _reservationFacade = reservationFacade;
            _mediator = mediator;
            _messageQueue = messageQueue;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            UpdateRouteCommand = new AsyncRelayCommand(UpdateRoute);
            ClearCommand = new AsyncRelayCommand(async _ => await LoadAsync());
        }

        

        public RideWrapper? DetailModel { get; set; }
        public List<VehicleListModel>? Vehicles { get; set; }
        public VehicleListModel? SelectedVehicle { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand UpdateRouteCommand { get; }

        public override async void UserLoggedIn(LoginMessage<UserWrapper> obj)
        {
            base.UserLoggedIn(obj);

            await LoadAsync();
        }

        public async Task LoadAsync()
        {
            if (LoggedUser is null)
                return;

            DetailModel = new(RideDetailModel.Empty);
            Vehicles = await _vehicleFacade.GetByOwnerAsync(LoggedUser.Id);
        }

        public bool CanSave() => DetailModel is not null && DetailModel.Distance > 0 && DetailModel.Arrival > DetailModel.Departure && SelectedVehicle is not null && (Vehicles?.Any(x => x.Id == SelectedVehicle.Id) ?? false);
        
        public async Task SaveAsync()
        {
            if (LoggedUser is null || DetailModel is null)
                return;

            // Check for conflicting rides/reservations
            if (await _reservationFacade.HasConflictingRide(LoggedUser.Id, DetailModel.Departure, DetailModel.Arrival))
            {
                await DialogHost.Show(new MessageDialog("Conficting Ride or Reservation",
                    "You are already signed up for another ride at the entered date and time.", DialogType.OK));
                return;
            }

            try
            {
                DetailModel.VehicleId = SelectedVehicle!.Id;
                await _rideFacade.SaveAsync(DetailModel);
                _mediator.Send(new AddedMessage<RideWrapper>());
                _mediator.Send(new SwitchTabMessage(ViewIndex.Dashboard));
                _messageQueue.Enqueue("Ride has been successfully created");

                await LoadAsync(); // Clear fields
            }
            catch
            {
                await DialogHost.Show(new MessageDialog("Creating Failed", "Failed to create the ride.", DialogType.OK));
            }
        }

        public TimeSpan Duration { get; private set; }
        public DateTime Today { get; set; } = DateTime.Now;
        
        private async Task UpdateRoute()
        {
            if (DetailModel?.FromName is null || DetailModel?.ToName is null)
                return;

            var routeInfo = await BusinessLogic.GetRouteInfoAsync(DetailModel.FromName, DetailModel.ToName);
            if (!routeInfo.Item1)
                return;

            DetailModel.Distance = routeInfo.Item2!.Value;
            Duration = routeInfo.Item3!.Value;
            DetailModel.Arrival = DetailModel.Departure.Add(Duration);
        }
    }

    
}
