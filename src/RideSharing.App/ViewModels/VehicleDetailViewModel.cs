using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.ViewModels.Interfaces;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.App.Extensions;
using RideSharing.App.Services;
using RideSharing.App.Services.MessageDialog;

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

            //SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        }
        public ICommand SaveCommand { get; }
        //public VehicleWrapper? Model { get; private set; }
        // TODO One command for adding and updating or two separate ones?
        public async Task LoadAsync(Guid id)
        {
           Model = await _vehicleFacade.GetAsync(id);  // TODO add empty vehicleDetailModel
        }



       /* public async SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Cannot save null model");
            }

            Model = await _vehicleFacade.SaveAsync(Model.Model);
            // TODO send Message

        }*/
    }

}

