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

            VehicleNewCommand = new RelayCommand(VehicleNew);
            VehicleDeleteCommand = new AsyncRelayCommand<VehicleListModel>(VehicleDelete);
            VehicleEditCommand = new RelayCommand(VehicleEdit);

            mediator.Register<UpdateMessage<VehicleWrapper>>(VehicleUpdated);
            mediator.Register<NewMessage<VehicleWrapper>>(VehicleCreated);
        }

        public ICommand VehicleNewCommand { get; }
        public ICommand VehicleDeleteCommand { get; }
        public ICommand VehicleEditCommand { get; }

        private void VehicleNew() => _mediator.Send(new NewMessage<VehicleWrapper>());

        private void VehicleEdit() => _mediator.Send(new ManageMessage<VehicleWrapper>());

        private async void VehicleUpdated(UpdateMessage<VehicleWrapper> _) => await LoadAsync();

        private async void VehicleCreated(NewMessage<VehicleWrapper> _) => await LoadAsync();

        public ObservableCollection<VehicleListModel> Vehicles { get; set; } = new();

        public async Task LoadAsync()
        {
            Vehicles.Clear();
            if (LoggedUser is null)
                return;

            var vehicles = await _vehicleFacade.GetByOwnerAsync(LoggedUser.Id);
            Vehicles.AddRange(vehicles);
        }

        public async Task VehicleDelete(VehicleListModel? vehicle)
        {
            try
            {
                await _vehicleFacade.DeleteAsync(vehicle!.Id);
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
