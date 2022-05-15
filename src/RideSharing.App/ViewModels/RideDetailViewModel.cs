using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Automation.Provider;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.Dialogs;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.Common.Enums;

namespace RideSharing.App.ViewModels
{
    public class RideDetailViewModel : ViewModelBase, IRideDetailViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly UserFacade _userFacade;
        private readonly VehicleFacade _vehicleFacade;
        private readonly ReservationFacade _reservationFacade;
        private readonly IMediator _mediator;
        private readonly ISnackbarMessageQueue _messageQueue;

        public RideDetailViewModel(
            RideFacade rideFacade,
            UserFacade userFacade,
            VehicleFacade vehicleFacade,
            ReservationFacade reservationFacade,
            IMediator mediator,
            ISnackbarMessageQueue messageQueue) : base(mediator)
        {
            _rideFacade = rideFacade;
            _userFacade = userFacade;
            _vehicleFacade = vehicleFacade;
            _reservationFacade = reservationFacade;
            _mediator = mediator;
            _messageQueue = messageQueue;

            UserReservationCommand = new AsyncRelayCommand(CreateOrEditReservationAsync, CanSave);
            DeleteReservationCommand = new AsyncRelayCommand(DeleteAsync);
            ContactDriverCommand = new AsyncRelayCommand(ContactDriver);

            mediator.Register<DetailMessage<RideWrapper>>(RideSelected);
        }


        public bool ReservationCreation { get; set; } = true;

        public RideWrapper? DetailModel { get; private set; }
        public UserWrapper? Driver { get; private set; }
        public VehicleWrapper? Vehicle { get; private set; }
        public ReservationWrapper? Reservation { get; private set; }

        public int MaxAvailableSeats { get; private set; }

        public int SelectedSeats { get; set; }

        public bool ReservationConflict { get; set; }
        public bool RideFull { get; set; }
        public bool SliderVisible
        {
            get
            {
                return (!ReservationConflict && MaxAvailableSeats != 0);
            }
        }
        public bool CanSave()
        {
            return (ReservationCreation && !ReservationConflict && MaxAvailableSeats != 0) || 
                   (!ReservationCreation && SelectedSeats != Reservation?.Seats);
        }


        public ICommand UserReservationCommand { get; }
        public ICommand DeleteReservationCommand { get; }
        public ICommand ContactDriverCommand { get; }


        public bool MapEnabled { get; set; }

        public TimeSpan? Duration { get; private set; }

        public async Task ContactDriver()
        {
            if (Driver?.Phone is null)
                return;

            ProcessStartInfo ps =
                new($"tel:{Regex.Replace(Driver.Phone, @"\s+", "")}") { UseShellExecute = true, Verb = "open" };
            Process.Start(ps);
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        private async void RideSelected(DetailMessage<RideWrapper> obj)
        {
            if (!obj.Id.HasValue)
                return;

            await LoadAsync(obj.Id.Value);
        }

        public async Task LoadAsync(Guid rideId)
        {
            if (LoggedUser is null)
                return;

            MapEnabled = false;
            DetailModel = await _rideFacade.GetAsync(rideId) ?? throw new InvalidOperationException("Failed to load the selected ride");
            Duration = DetailModel.Arrival - DetailModel.Departure;
            MaxAvailableSeats = DetailModel.SharedSeats - DetailModel.OccupiedSeats;

            MapEnabled = true;
            var currentRide = await _rideFacade.GetAsync(DetailModel.Id);
            if (currentRide?.Vehicle is null)
                return;

            Vehicle = await _vehicleFacade.GetAsync(currentRide.Vehicle.Id);
            Driver = await _userFacade.GetAsync(currentRide.Vehicle.OwnerId);
            var reservation = await _reservationFacade.GetUserReservationByRideAsync(LoggedUser.Id, rideId);
            if (reservation is not null)
            { // editing reservation
                MaxAvailableSeats += reservation.Seats;
                ReservationCreation = false;
                Reservation = reservation;
                SelectedSeats = Reservation.Seats;
            }
            else
            { // creating reservation
                Reservation = null;
                ReservationCreation = true;
                CheckReservationConflict();
                SelectedSeats = 1;
                RideFull = MaxAvailableSeats == 0;
            }

        }

        public async Task DeleteAsync()
        {
            if (LoggedUser is null || Reservation is null)
                return;

            var delete = await DialogHost.Show(new MessageDialog("Delete Reservation", "Do you really want to delete your reservation?",
                DialogType.YesNo));
            if (delete is not ButtonType.Yes)
                return;
            try
            {
                await _reservationFacade.DeleteAsync(Reservation.Id);
                _mediator.Send(new DeleteMessage<ReservationWrapper> {});
                _mediator.Send(new SwitchTabMessage(ViewIndex.Dashboard));
                _messageQueue.Enqueue("Changes have been successfully saved");
            }
            catch
            {
                await DialogHost.Show(new MessageDialog("Deleting Failed", "Failed to save the changes.", DialogType.OK));
            }

}

public async Task CreateOrEditReservationAsync()
        {
            if (LoggedUser is null || DetailModel is null)
                return;

            if (Reservation is null)
            {
                Reservation = new ReservationDetailModel(DateTime.Now, (ushort)SelectedSeats, LoggedUser.Id, DetailModel.Id);
            }
            else
            {
                Reservation.Seats = (ushort) SelectedSeats;
            }
            
            await _reservationFacade.SaveAsync(Reservation);
            _mediator.Send(new NewMessage<ReservationWrapper> { });
            _mediator.Send(new SwitchTabMessage(ViewIndex.Dashboard));
            _messageQueue.Enqueue($"Reservation has been successfully {(ReservationCreation ? "created" : "edited")}.");
        }

        private async void CheckReservationConflict()
        {
            if (LoggedUser is null || DetailModel is null)
                return;

            ReservationConflict = ReservationCreation && await _reservationFacade.HasConflictingRide(LoggedUser.Id, DetailModel.Id);
        }


    }

    
}
