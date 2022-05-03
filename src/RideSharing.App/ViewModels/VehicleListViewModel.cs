using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.App.Extensions;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.MessageDialog;
using RideSharing.App.Wrappers;

namespace RideSharing.App.ViewModels
{
    public class VehicleListViewModel : ViewModelBase, IVehicleListViewModel
    {
        private readonly VehicleFacade _vehicleFacade;
        private readonly IMediator _mediator;
        private readonly IMessageDialogService _messageDialogService;

        public VehicleListViewModel(
            VehicleFacade vehicleFacade,
            IMediator mediator,
            IMessageDialogService messageDialogService) : base(mediator)
        {
            _vehicleFacade = vehicleFacade;
            _mediator = mediator;
            _messageDialogService = messageDialogService;

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

            try
            {
                await _vehicleFacade.DeleteAsync(vehicle.Id);
            }

            catch
            {
                var _ = _messageDialogService.Show(
                    $"Deleting of vehicle failed!",
                    "Deleting failed",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
            }
            finally
            {
                await LoadAsync();
            }
        }
    }
}
