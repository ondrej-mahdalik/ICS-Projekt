using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.MessageDialog;
using RideSharing.App.ViewModels.Interfaces;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;

namespace RideSharing.App.ViewModels
{
    public class RideDetailViewModel : ViewModelBase, IRideDetailViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly IMediator _mediator;
        private readonly IMessageDialogService _messageDialogService;

        public RideDetailViewModel(
            RideFacade rideFacade,
            IMediator mediator,
            IMessageDialogService messageDialogService)
        {
            _rideFacade = rideFacade;
            _mediator = mediator;
            _messageDialogService = messageDialogService;

            DeleteRideCommand = new AsyncRelayCommand(DeleteAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        }
        public RideWrapper? Model { get; private set; }

        public ICommand SaveCommand { get; }
        public ICommand DeleteRideCommand { get; }
        public ICommand UserReservationCommand { get; }

        public RideWrapper? Ride { get; private set; }

        private bool CanSave() => Ride?.IsValid ?? false;


        public async Task SaveAsync()
        {
            if (Ride == null)
            {
                throw new InvalidOperationException("Cannot save null model");
            }

            Ride = await _rideFacade.SaveAsync(Ride);
             _mediator.Send(new UpdateMessage<RideWrapper> { Model = Ride });

        }

        public async Task LoadAsync(Guid rideId)
        {
            Ride = await _rideFacade.GetAsync(rideId) ?? throw new InvalidOperationException("Failed to load the selected ride");
        }

        public async Task DeleteAsync()
        {
            if (Ride == null)
            {
                throw new InvalidOperationException("Null model cannot be deleted");
            }

            var delete = _messageDialogService.Show(
                    $"Delete",
                    $"Do you want to delete this ride?.",
                    MessageDialogButtonConfiguration.YesNo,
                    MessageDialogResult.No);

            if (delete == MessageDialogResult.No) return;

            try
            {
                await _rideFacade.DeleteAsync(Ride!.Id);
            }
            catch
            {
                var _ = _messageDialogService.Show(
                       $"Deleting of the ride failed!",
                       "Deleting failed",
                       MessageDialogButtonConfiguration.OK,
                       MessageDialogResult.OK);
            }

            _mediator.Send(new DeleteMessage<RideWrapper>
            {
                Model = Ride
            });
        }
    }
}
