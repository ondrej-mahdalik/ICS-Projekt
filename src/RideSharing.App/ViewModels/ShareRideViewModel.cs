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
    public class ShareRideViewModel : ViewModelBase, IShareRideViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly UserFacade _userFacade;
        private readonly VehicleFacade _vehicleFacade;
        private readonly IMediator _mediator;

        public ShareRideViewModel(
            RideFacade rideFacade,
            UserFacade userFacade,
            VehicleFacade vehicleFacade,
            IMediator mediator) : base(mediator)
        {
            _rideFacade = rideFacade;
            _userFacade = userFacade;
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
