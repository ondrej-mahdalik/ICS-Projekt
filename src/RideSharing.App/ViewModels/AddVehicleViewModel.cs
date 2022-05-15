using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.Dialogs;
using RideSharing.App.Wrappers;
using RideSharing.BL;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.Common.Enums;
using AsyncRelayCommand = RideSharing.App.Commands.AsyncRelayCommand;

namespace RideSharing.App.ViewModels
{
    public class AddVehicleViewModel : ViewModelBase, IAddVehicleViewModel
    {
        private readonly VehicleFacade _vehicleFacade;
        private readonly IMediator _mediator;
        private readonly ISnackbarMessageQueue _messageQueue;

        public AddVehicleViewModel(
            VehicleFacade vehicleFacade,
            IMediator mediator,
            ISnackbarMessageQueue messageQueue) : base(mediator)
        {
            _vehicleFacade = vehicleFacade;
            _mediator = mediator;
            _messageQueue = messageQueue;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            ChangeImageCommand = new Commands.AsyncRelayCommand<string>(ChangeImageAsync);
            CancelCommand = new RelayCommand(Cancel);

            mediator.Register<NewMessage<VehicleWrapper>>(async _ => await LoadAsync());
        }

        private void Cancel()
        {
            _mediator.Send(new SwitchTabMessage(ViewIndex.VehicleList));
        }

        public VehicleWrapper? DetailModel { get; private set; }

        public ICommand SaveCommand { get; }
        public ICommand ChangeImageCommand { get; }
        public ICommand CancelCommand { get; }

        public static IEnumerable<string> VehicleTypes
        {
            get => Enum.GetNames<VehicleType>();
        }


        public async Task LoadAsync()
        {
            if (LoggedUser is null)
                return;

            DetailModel = new VehicleWrapper(VehicleDetailModel.Empty)
            {
                OwnerId = LoggedUser.Id
            };
        }

        private bool CanSave() => (DetailModel?.IsValid ?? false) && !UploadingImage;

        public async Task SaveAsync()
        {
            if (DetailModel == null)
                return;

            try
            {
                DetailModel = await _vehicleFacade.SaveAsync(DetailModel);
                _mediator.Send(new AddedMessage<VehicleWrapper>());
                _mediator.Send(new SwitchTabMessage(ViewIndex.VehicleList));
                _messageQueue.Enqueue("Vehicle has been successfully added.");
            }
            catch
            {
                await DialogHost.Show(new MessageDialog("Creating Failed", "Failed to create the vehicle.",
                    DialogType.OK));
            }
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
    }

    
}
