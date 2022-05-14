using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

            UserReservationCommand = new AsyncRelayCommand<ushort>(CreateOrEditReservationAsync);
            ContactDriverCommand = new AsyncRelayCommand(ContactDriver);

            mediator.Register<DetailMessage<RideWrapper>>(RideSelected);
        }

        

        public RideWrapper? DetailModel { get; private set; }
        public UserWrapper? Driver { get; private set; }
        public VehicleWrapper? Vehicle { get; private set; }
        public ReservationWrapper? Reservation { get; private set; }
        public float DriverRating { get; private set; }
        public int TotalNumberOfReviews { get; private set; }
        public int AvailableSeats { get; private set; }
        public ICommand UserReservationCommand { get; }
        public ICommand ContactDriverCommand { get; }
        public int SelectedSeats { get; set; } = 1;


        public bool MapEnabled { get; set; }

        public bool ReservationPossible
        {
            get => _reservationPossible && SelectedSeats <= AvailableSeats;
        }
        public TimeSpan? Duration { get; private set; }

        public async Task ContactDriver()
        {
            if (Driver?.Phone is null)
                return;

            System.Diagnostics.Process.Start($"tel:{Regex.Replace(Driver.Phone, @"\s+", "")}");
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
            AvailableSeats = DetailModel.SharedSeats - DetailModel.OccupiedSeats;
            Duration = DetailModel.Arrival - DetailModel.Departure;
            MapEnabled = true;
            var currentRide = await _rideFacade.GetAsync(DetailModel.Id);
            if (currentRide?.Vehicle is null)
                return;

            Vehicle = await _vehicleFacade.GetAsync(currentRide.Vehicle.Id);
            Driver = await _userFacade.GetAsync(currentRide.Vehicle.OwnerId);
            var reservation = await _reservationFacade.GetUserReservationByRide(LoggedUser.Id, rideId);
            if (reservation is not null)
                Reservation = reservation;
            else
                Reservation = null;


            if (Reservation is not null)
            {
                SelectedSeats = Reservation.Seats;
            }

            CheckReservationPossibility();
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException(); // TODO Delete reservation (not ride)
        }

        public async Task CreateOrEditReservationAsync(ushort seats)
        {
            if (LoggedUser is null || DetailModel is null)
                return;
            bool creation = true;
            if (Reservation is null)
            {
                Reservation = new ReservationDetailModel(DateTime.Now, seats)
                {
                    ReservingUser = await _userFacade.GetAsync(LoggedUser.Id),
                    Ride = DetailModel
                };
            }
            else
            {
                creation = false;
                Reservation.Seats = seats;
            }
            await _reservationFacade.SaveAsync(Reservation);
            
            _messageQueue.Enqueue($"Reservation has been successfully {(creation ? "created" : "edited")}.");
        }


        private bool _reservationPossible { get; set; }
        private async void CheckReservationPossibility()
        {
            if (LoggedUser is null || DetailModel is null)
                return;

            _reservationPossible = await _reservationFacade.CanCreateReservation(LoggedUser.Id, DetailModel.Id); // TODO
        }


    }

    
}
