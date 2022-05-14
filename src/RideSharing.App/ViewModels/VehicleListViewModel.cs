using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.App.Extensions;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.Dialogs;
using RideSharing.App.Wrappers;

namespace RideSharing.App.ViewModels
{
    public class VehicleListViewModel : ViewModelBase, IVehicleListViewModel
    {
        private readonly VehicleFacade _vehicleFacade;
        private readonly IMediator _mediator;
        private readonly ISnackbarMessageQueue _messageQueue;

        public VehicleListViewModel(
            VehicleFacade vehicleFacade,
            IMediator mediator,
            ISnackbarMessageQueue messageQueue) : base(mediator)
        {
            _vehicleFacade = vehicleFacade;
            _mediator = mediator;
            _messageQueue = messageQueue;

            VehicleNewCommand = new RelayCommand(NewVehicle);
            VehicleDeleteCommand = new AsyncRelayCommand<VehicleListModel>(DeleteVehicle);
            VehicleEditCommand = new RelayCommand<VehicleListModel>(EditVehicle);

            mediator.Register<UpdateMessage<VehicleWrapper>>(VehicleUpdated);
            mediator.Register<NewMessage<VehicleWrapper>>(VehicleCreated);
        }

        public ICommand VehicleNewCommand { get; }
        public ICommand VehicleDeleteCommand { get; }
        public ICommand VehicleEditCommand { get; }

        private void NewVehicle() => _mediator.Send(new NewMessage<VehicleWrapper>());

        private void EditVehicle(VehicleListModel? model)
        {
            if (model is not null)
                _mediator.Send(new ManageMessage<VehicleWrapper> { Id = model.Id });
        }

        private async void VehicleUpdated(UpdateMessage<VehicleWrapper> _) => await LoadAsync();

        private async void VehicleCreated(NewMessage<VehicleWrapper> _) => await LoadAsync();

        public ObservableCollection<VehicleListModel> Vehicles { get; set; } = new();

        public override async void UserLoggedIn(LoginMessage<UserWrapper> obj)
        {
            base.UserLoggedIn(obj);
            await LoadAsync();
        }

        public async Task LoadAsync()
        {
            Vehicles.Clear();
            if (LoggedUser is null)
                return;

            var vehicles = await _vehicleFacade.GetByOwnerAsync(LoggedUser.Id);
            Vehicles.AddRange(vehicles);
        }

        public async Task DeleteVehicle(VehicleListModel? vehicle)
        {
            if (vehicle is null)
                return;

            var delete = await DialogHost.Show(new MessageDialog("Delete Vehicle",
                "Do you really want to delete the vehicle?\n All related rides and their reservations will be deleted as well.",
                DialogType.YesNo));
            if (delete is not ButtonType.Yes)
                return;

            try
            {
                await _vehicleFacade.DeleteAsync(vehicle.Id);
                _messageQueue.Enqueue("Vehicle has been successfully deleted");
            }

            catch
            {
                await DialogHost.Show(new MessageDialog("Deleting Failed", "Failed to delete the vehicle.",
                    DialogType.OK));
            }
            finally
            {
                await LoadAsync();
            }
        }
    }
}
