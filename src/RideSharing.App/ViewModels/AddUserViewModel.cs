using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Wrappers;
using RideSharing.BL;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.Common.Enums;

namespace RideSharing.App.ViewModels
{
    public class AddUserViewModel : ViewModelBase, IAddUserViewModel
    {
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;

        public AddUserViewModel(
            UserFacade userFacade,
            IMediator mediator) : base(mediator)
        {
            _userFacade = userFacade;
            _mediator = mediator;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            ChangeImageCommand = new AsyncRelayCommand<string>(ChangeImageAsync);

            mediator.Register<NewMessage<UserWrapper>>(async _ => await LoadAsync());
        }

        private void Cancel()
        {
            _mediator.Send(new SwitchTabLoginMessage(LoginViewIndex.SelectUser));
        }

        public UserWrapper? DetailModel { get; private set; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ChangeImageCommand { get; }

        public bool CanSave()
        {
            return DetailModel is not null && DetailModel.Name!.Length > 0 &&
                   DetailModel.Surname!.Length > 0 && DetailModel.Phone!.Length > 0;
        }

        public async Task LoadAsync()
        {
            DetailModel = new UserWrapper(UserDetailModel.Empty);
        }

        public async Task SaveAsync()
        {
            if (DetailModel is null || !CanSave())
                return;

            await _userFacade.SaveAsync(DetailModel);
            _mediator.Send(new AddedMessage<UserWrapper>());
            _mediator.Send(new SwitchTabLoginMessage(LoginViewIndex.SelectUser));
            
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
