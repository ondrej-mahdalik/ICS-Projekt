using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using RideSharing.App.Commands;
using RideSharing.BL.Facades;
using RideSharing.App.Services;
using RideSharing.App.Wrappers;
using RideSharing.App.Messages;
using RideSharing.BL;
using RideSharing.Common.Enums;

namespace RideSharing.App.ViewModels
{
    public class VehicleDetailViewModel : ViewModelBase, IVehicleDetailViewModel
    {
        private readonly VehicleFacade _vehicleFacade;
        private readonly IMediator _mediator;
        private readonly ISnackbarMessageQueue _messageQueue;

        public VehicleDetailViewModel(
            VehicleFacade vehicleFacade,
            IMediator mediator,
            ISnackbarMessageQueue messageQueue) : base(mediator)
        {
            _vehicleFacade = vehicleFacade;
            _mediator = mediator;
            _messageQueue = messageQueue;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            ChangeImageCommand = new AsyncRelayCommand<string>(ChangeImageAsync);
            CancelCommand = new RelayCommand(() => _mediator.Send(new SwitchTabMessage(ViewIndex.VehicleList)));
            mediator.Register<ManageMessage<VehicleWrapper>>(async delegate(ManageMessage<VehicleWrapper> message)
            {
                if (message.Id.HasValue)
                    await LoadAsync(message.Id.Value);
            });
        }
        public ICommand SaveCommand { get; }
        public ICommand ChangeImageCommand { get; }
        public ICommand CancelCommand { get; }
        public VehicleWrapper? DetailModel { get; private set; }

        public static IEnumerable<string> VehicleTypes
        {
            get => Enum.GetNames<VehicleType>();
        }
        public bool UploadingImage { get; private set; }

        private async Task ChangeImageAsync(string? filePath)
        {
            if (filePath is null || DetailModel is null)
                return;

            UploadingImage = true;
            try
            {
                var imageUrl = await BusinessLogic.UploadImageAsync(filePath);
                DetailModel.ImageUrl = imageUrl;
            }
            finally
            {
                UploadingImage = false;
            }
        }

        public async Task LoadAsync(Guid id)
        {
           DetailModel = await _vehicleFacade.GetAsync(id);  // TODO add empty vehicleDetailModel
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        private bool CanSave() => DetailModel?.IsValid ?? false;


        public async Task SaveAsync()
        {
            if (DetailModel == null)
                throw new InvalidOperationException("Cannot save null model");

            DetailModel = await _vehicleFacade.SaveAsync(DetailModel);
            _mediator.Send(new UpdateMessage<VehicleWrapper> { Model = DetailModel });
            _mediator.Send(new SwitchTabMessage(ViewIndex.VehicleList));
            _messageQueue.Enqueue("Changes have been successfully saved.");
        }
    }

}

