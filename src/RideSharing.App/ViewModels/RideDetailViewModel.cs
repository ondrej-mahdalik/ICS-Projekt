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
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;

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
            ContactDriverCommand = new AsyncRelayCommand(ContactDriver);

            mediator.Register<DetailMessage<RideWrapper>>(RideSelected);
        }


        private bool _reservationCreation = true;

        public RideWrapper? DetailModel { get; private set; }
        public UserWrapper? Driver { get; private set; }
        public VehicleWrapper? Vehicle { get; private set; }
        public ReservationWrapper? Reservation { get; private set; }

        public int MaxAvailableSeats { get; private set; }

        public int SelectedSeats { get; set; }

        public bool ReservationConflict { get; set; } = false;

        public bool CanSave()
        {
            return _reservationCreation || SelectedSeats != Reservation?.Seats;
        }


public ICommand UserReservationCommand { get; }
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
                _reservationCreation = false;
                Reservation = reservation;
                SelectedSeats = Reservation.Seats;
            }
            else
            { // creating reservation
                Reservation = null;
                CheckReservationConflict();
            }
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException(); // TODO Delete reservation (not ride)
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
            
            _messageQueue.Enqueue($"Reservation has been successfully {(_reservationCreation ? "created" : "edited")}.");
        }

        private async void CheckReservationConflict()
        {
            if (LoggedUser is null || DetailModel is null)
                return;

            ReservationConflict = _reservationCreation && await _reservationFacade.HasConflictingRide(LoggedUser.Id, DetailModel.Id);
        }


    }

    
}
