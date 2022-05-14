using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.Dialogs;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.Common.Enums;

namespace RideSharing.App.ViewModels
{
    public class RideManagementViewModel : ViewModelBase, IRideManagementViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly ReservationFacade _reservationFacade;
        private readonly VehicleFacade _vehicleFacade;
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;
        private readonly ISnackbarMessageQueue _messageQueue;

        public RideManagementViewModel(
            RideFacade rideFacade,
            ReservationFacade reservationFacade,
            VehicleFacade vehicleFacade,
            UserFacade userFacade,
            IMediator mediator,
            ISnackbarMessageQueue messageQueue) : base(mediator)
        {
            _rideFacade = rideFacade;
            _reservationFacade = reservationFacade;
            _vehicleFacade = vehicleFacade;
            _userFacade = userFacade;
            _mediator = mediator;
            _messageQueue = messageQueue;

            DeleteRideCommand = new AsyncRelayCommand(DeleteAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            DeleteReservationCommand = new AsyncRelayCommand<ReservationWrapper>(DeleteReservationAsync);
            ContactUserCommand = new AsyncRelayCommand<ReservationWrapper>(ContactUser);

            _mediator.Register<ManageMessage<RideWrapper>>(RideSelected);
        }

        private async void RideSelected(ManageMessage<RideWrapper> obj)
        {
            if (!obj.Id.HasValue)
                return;

            await LoadAsync(obj.Id.Value);
        }

        private async Task ContactUser(ReservationWrapper? reservation)
        {
            if (reservation is null)
                return;

            var user = await _userFacade.GetAsync(reservation.ReservingUserId);
            if (user is null)
                return;

            ProcessStartInfo ps =
                new($"tel:{Regex.Replace(user.Phone, @"\s+", "")}") { UseShellExecute = true, Verb = "open" };
            Process.Start(ps);
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteRideCommand { get; }
        public ICommand DeleteReservationCommand { get; }
        public ICommand ContactUserCommand { get; }
        public RideWrapper? DetailModel { get; private set; }
        public VehicleWrapper? Vehicle { get; private set; }
        public List<ReservationWrapper> Reservations { get; private set; }
        public TimeSpan? Duration { get => DetailModel?.Arrival - DetailModel?.Departure; }

        private bool CanSave() => DetailModel?.IsValid ?? false;
        public bool MapEnabled { get; set; }
        public bool EnoughSeats { get => DetailModel?.SharedSeats > DetailModel?.OccupiedSeats; }

        public async Task DeleteAsync()
        {
            if (DetailModel == null)
                throw new InvalidOperationException("Null model cannot be deleted");

            var delete = await DialogHost.Show(new MessageDialog("Delete Ride",
                "Do you really want to delete this ride?", DialogType.YesNo));

            if (delete is not ButtonType.Yes)
                return;

            try
            {
                await _rideFacade.DeleteAsync(DetailModel.Id);
                _mediator.Send(new DeleteMessage<RideWrapper>());
                _mediator.Send(new SwitchTabMessage(ViewIndex.Dashboard));
                _messageQueue.Enqueue("Ride has been successfully deleted.");
            }
            catch
            {
                await DialogHost.Show(new MessageDialog("Deleting Failed", "Failed to delete the ride.",
                    DialogType.OK));
            }

            _mediator.Send(new DeleteMessage<RideWrapper>
            {
                Model = DetailModel
            });
        }

        public async Task DeleteReservationAsync(ReservationWrapper? reservation)
        {
            if (DetailModel is null || reservation is null)
                return;

            var delete = await DialogHost.Show(new MessageDialog("Remove Reservation",
                $"Do you really wish to remove {reservation.ReservingUserName}'s reservation?", DialogType.YesNo));
            if (delete is not ButtonType.Yes)
                return;

            try
            {
                await _reservationFacade.DeleteAsync(reservation.Id);
                _messageQueue.Enqueue("Reservation has been successfully removed.");
            }
            catch
            {
                await DialogHost.Show(new MessageDialog("Deleting Failed", "Failed to delete the reservation.",
                    DialogType.OK));
            }
            finally
            {
                await LoadAsync(DetailModel.Id);
            }
        }

        public async Task LoadAsync(Guid id)
        {
            MapEnabled = false;
            DetailModel = await _rideFacade.GetAsync(id); // TODO Add empty model
            var vehicle = await _vehicleFacade.GetAsync(DetailModel.VehicleId);
            Vehicle = vehicle;
            Reservations = (await _reservationFacade.GetReservationsByRideAsync(id)).Select(x => (ReservationWrapper)x).ToList();
            MapEnabled = true;
        }

        public async Task SaveAsync()
        {
            if (DetailModel == null)
            {
                throw new InvalidOperationException("Cannot save null model");
            }

            DetailModel = await _rideFacade.SaveAsync(DetailModel);
            _mediator.Send(new UpdateMessage<RideWrapper> { Model = DetailModel });
        }
    }
}

