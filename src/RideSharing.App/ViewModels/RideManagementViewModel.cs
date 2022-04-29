using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.MessageDialog;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;

namespace RideSharing.App.ViewModels
{
    public class RideManagementViewModel : ViewModelBase, IRideManagementViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly IMediator _mediator;
        private readonly IMessageDialogService _messageDialogService;

        public RideManagementViewModel(
            RideFacade rideFacade,
            IMediator mediator,
            IMessageDialogService messageDialogService)
        {
            _rideFacade = rideFacade;
            _mediator = mediator;
            _messageDialogService = messageDialogService;

            DeleteRideCommand = new AsyncRelayCommand(DeleteAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);

        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteRideCommand { get; }
        public RideWrapper? DetailModel { get; private set; }
        private bool CanSave() => DetailModel?.IsValid ?? false;

        public async Task DeleteAsync()
        {
            if (DetailModel == null)
            {
                throw new InvalidOperationException("Null model cannot be deleted");
            }

            var delete = _messageDialogService.Show(
                $"Delete",
                $"Do you want to delete this ride?.",
                MessageDialogButtonConfiguration.YesNo,
                MessageDialogResult.No);

            if (delete == MessageDialogResult.No) return;

            try
            {
                await _rideFacade.DeleteAsync(DetailModel!.Id);
            }
            catch
            {
                var _ = _messageDialogService.Show(
                    $"Deleting of the ride failed!",
                    "Deleting failed",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
            }

            _mediator.Send(new DeleteMessage<RideWrapper>
            {
                Model = DetailModel
            });
        }

        public async Task LoadAsync(Guid id)
        {
            DetailModel = await _rideFacade.GetAsync(id); // TODO Add empty model
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

