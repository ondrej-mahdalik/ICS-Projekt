using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.ViewModels.Interfaces;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.App.Extensions;

namespace RideSharing.App.ViewModels
{
    public class VehicleDetailViewModel : ViewModelBase
    {
        private VehicleFacade _vehicleFacade;
        // TODO Add mediator and messageService

        public VehicleDetailViewModel(
            VehicleFacade vehicleFacade)
        {
            _vehicleFacade = vehicleFacade;
        }

        // TODO Add model
        public ICommand AddOrUpdateCommand { get; }
        public async Task LoadAsync(Guid id)
        {
          // Model = await _vehicleFacade.GetAsync(id);  // TODO add empty vehicleDetailModel
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

