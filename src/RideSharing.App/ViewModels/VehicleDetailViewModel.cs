using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.ViewModels;
using RideSharing.BL.Facades;
using RideSharing.App.Services;
using RideSharing.App.Services.MessageDialog;
using RideSharing.App.Wrappers;
using RideSharing.App.Messages;

namespace RideSharing.App.ViewModels
{
    public class VehicleDetailViewModel : ViewModelBase, IVehicleDetailViewModel
    {
        private readonly VehicleFacade _vehicleFacade;
        private readonly IMediator _mediator;
        private readonly IMessageDialogService _messageDialogService;

        public VehicleDetailViewModel(
            VehicleFacade vehicleFacade,
            IMediator mediator,
            IMessageDialogService messageDialogService)
        {
            _vehicleFacade = vehicleFacade;
            _mediator = mediator;
            _messageDialogService = messageDialogService;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        }
        public ICommand SaveCommand { get; }
        public VehicleWrapper? Vehicle { get; private set; }
        public async Task LoadAsync(Guid id)
        {
           Vehicle = await _vehicleFacade.GetAsync(id);  // TODO add empty vehicleDetailModel
        }

        public async Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        private bool CanSave() => Vehicle?.IsValid ?? false;


        public async Task SaveAsync()
        {
            if (Vehicle == null)
                throw new InvalidOperationException("Cannot save null model");

            Vehicle = await _vehicleFacade.SaveAsync(Vehicle);
            _mediator.Send(new UpdateMessage<VehicleWrapper> { Model = Vehicle });

        }
    }

}

