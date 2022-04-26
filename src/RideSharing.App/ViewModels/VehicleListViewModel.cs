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
    public class VehicleListViewModel : ViewModelBase // TODO add Interface
    {
        private readonly VehicleFacade _vehicleFacade;
        // TODO Add mediator and messageService


        public VehicleListViewModel(
            VehicleFacade vehicleFacade)
        {
            _vehicleFacade = vehicleFacade;
            // TODO Add mediator and messageService to ctor
            
            // TODO assign commands

            // TODO register mediator for creating, updating and deleting
        }

        public ICommand VehicleNewCommand { get; }
        public ICommand VehicleDeleteCommand { get; }
        public ICommand VehicleEditCommand { get; }

        public ObservableCollection<VehicleListModel> Vehicles { get; set; } = new();

        public async Task LoadAsync()
        {
            Vehicles.Clear();
            var vehicles = await _vehicleFacade.GetAsync();
            Vehicles.AddRange(vehicles);
        }
    }
}
