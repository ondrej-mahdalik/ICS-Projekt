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
        }
        public RideWrapper? Model { get; private set; }

        public ICommand EditNoteCommand { get; }
        public ICommand DeleteRideCommand { get; }

        public async Task LoadAsync(Guid rideId)
        {
            Model = await _rideFacade.GetAsync(rideId);
        }

        public async Task DeleteAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be deleted");
            }

            try
            {
                await _rideFacade.DeleteAsync(Model!.Id);
            }
            catch
            {

            }

            _mediator.Send(new DeleteMessage<RideWrapper>
            {
                Model = Model
            });
        }
    }
}
