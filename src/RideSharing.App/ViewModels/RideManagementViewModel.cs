using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using RideSharing.App.Commands;
using RideSharing.App.Extensions;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.Dialogs;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;

namespace RideSharing.App.ViewModels
{
    public class RideManagementViewModel : ViewModelBase, IRideManagementViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly ReservationFacade _reservationFacade;
        private readonly VehicleFacade _vehicleFacade;
        private readonly IMediator _mediator;

        public RideManagementViewModel(
            RideFacade rideFacade,
            ReservationFacade reservationFacade,
            VehicleFacade vehicleFacade,
            IMediator mediator) : base(mediator)
        {
            _rideFacade = rideFacade;
            _reservationFacade = reservationFacade;
            _vehicleFacade = vehicleFacade;
            _mediator = mediator;

            DeleteRideCommand = new AsyncRelayCommand(DeleteAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            RemoveReservationCommand = new AsyncRelayCommand<Guid>(DeleteReservationAsync);

        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteRideCommand { get; }
        public ICommand RemoveReservationCommand { get; }
        public RideWrapper? DetailModel { get; private set; }

        public ObservableCollection<VehicleListModel> DriversVehicles { get; set; } = new();
        private bool CanSave() => DetailModel?.IsValid ?? false;

        public async Task DeleteAsync()
        {
            if (DetailModel == null)
            {
                throw new InvalidOperationException("Null model cannot be deleted");
            }

            var delete = await DialogHost.Show(new MessageDialog("Delete Ride",
                "Do you really want to delete this ride?", DialogType.YesNo));

            if (delete is not ButtonType.Yes)
                return;

            try
            {
                await _rideFacade.DeleteAsync(DetailModel!.Id);
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

        public async Task DeleteReservationAsync(Guid reservingUserId)
        {
            if (DetailModel?.Reservations is not null)
            {
                var reservationId = DetailModel.Reservations.First(i => i.ReservingUserId == reservingUserId).Id;
                try
                {
                    await _reservationFacade.DeleteAsync(reservationId);
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
        }

        public async Task LoadUsersVehicles(Guid id)
        {
            var driversVehicles = await _vehicleFacade.GetByOwnerAsync(id);
            DriversVehicles.AddRange(driversVehicles);
        }

        public async Task LoadAsync(Guid id)
        {
            DetailModel = await _rideFacade.GetAsync(id); // TODO Add empty model
            await LoadUsersVehicles(id);
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

