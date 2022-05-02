using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Services;
using RideSharing.App.Services.MessageDialog;
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
        private readonly IMediator _mediator;
        private readonly IMessageDialogService _messageDialogService;

        public RideDetailViewModel(
            RideFacade rideFacade,
            UserFacade userFacade,
            VehicleFacade vehicleFacade,
            IMediator mediator,
            IMessageDialogService messageDialogService) : base(mediator)
        {
            _rideFacade = rideFacade;
            _userFacade = userFacade;
            _vehicleFacade = vehicleFacade;
            _mediator = mediator;
            _messageDialogService = messageDialogService;

            UserReservationCommand = new AsyncRelayCommand<ushort>(CreateReservationAsync);
            ContactDriverCommand = new AsyncRelayCommand(ContactDriver);

            
        }
        public RideWrapper? DetailModel { get; private set; }
        public UserWrapper? Driver { get; private set; }
        public float DriverRating { get; private set; }
        public int TotalNumberOfReviews { get; private set; }
        public int AvailableSeats { get; private set; }
        public VehicleWrapper? Vehicle { get; private set; }
        public ICommand UserReservationCommand { get; }
        public ICommand ContactDriverCommand { get; }


        public bool MapEnabled { get; set; }

        public TimeSpan? Duration { get; private set; }

        public void ContactDriver()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();

        }

        public async Task LoadAsync(Guid rideId)
        {
            MapEnabled = false;
            DetailModel = await _rideFacade.GetAsync(rideId) ?? throw new InvalidOperationException("Failed to load the selected ride");
            Duration = DetailModel.Arrival - DetailModel.Departure;
            MapEnabled = true;
            var currentRide = await _rideFacade.GetAsync(DetailModel.Id);
            if (currentRide?.Vehicle is null)
                return;

            Vehicle = await _vehicleFacade.GetAsync(currentRide.Vehicle.Id);
            Driver = await _userFacade.GetAsync(currentRide.Vehicle.OwnerId);

            AvailableSeats = DetailModel.SharedSeats - DetailModel.OccupiedSeats;
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CreateReservationAsync(ushort seats)
        {
            if (LoggedUser is null || DetailModel is null)
                return;

            ReservationDetailModel reservation = new ReservationDetailModel(DateTime.Now, seats)
            {
                ReservingUser = await _userFacade.GetAsync(LoggedUser.Id),
                Ride = DetailModel
            };

            if (DetailModel.SharedSeats - DetailModel.OccupiedSeats < seats)
            {
                throw new InvalidOperationException("This ride is full");
            }
            throw new NotImplementedException();
        }
    }
}
