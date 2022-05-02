using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.MessageDialog;
using RideSharing.App.ViewModels;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;

namespace RideSharing.App.ViewModels
{
    public class RideDetailViewModel : ViewModelBase, IRideDetailViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;
        private readonly IMessageDialogService _messageDialogService;

        public RideDetailViewModel(
            RideFacade rideFacade,
            UserFacade userFacade,
            IMediator mediator,
            IMessageDialogService messageDialogService) : base(mediator)
        {
            _rideFacade = rideFacade;
            _userFacade = userFacade;
            _mediator = mediator;
            _messageDialogService = messageDialogService;

            UserReservationCommand = new AsyncRelayCommand<ushort>(CreateReservationAsync);
        }
        public RideWrapper? DetailModel { get; private set; }

        public ICommand UserReservationCommand { get; }


        public bool MapEnabled { get; set; }

        public TimeSpan? Duration { get; private set; }

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
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CreateReservationAsync(ushort seats)
        {
            ReservationDetailModel Reservation = new ReservationDetailModel(DateTime.Now, seats)
            {
              //  ReservingUser = await _userFacade.GetAsync(userId),
                Ride = DetailModel
            };

            //if (DetailModel.SharedSeats - DetailModel.)
            //{

            //}
            throw new NotImplementedException();
        }
    }
}
