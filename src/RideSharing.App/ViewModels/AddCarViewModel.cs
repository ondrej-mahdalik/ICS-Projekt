using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;

namespace RideSharing.App.ViewModels
{
    public class AddCarViewModel : ViewModelBase, IAddCarViewModel
    {
        private readonly VehicleFacade _vehicleFacade;
        private readonly IMediator _mediator;

        public AddCarViewModel(
            VehicleFacade vehicleFacade,
            IMediator mediator) : base(mediator)
        {
            _vehicleFacade = vehicleFacade;
            _mediator = mediator;
        }

        public async Task LoadAsync()
        {

        }

        public async Task SaveAsync()
        {

        }
    }

    
}
