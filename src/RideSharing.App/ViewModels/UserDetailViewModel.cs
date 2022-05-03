using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.MessageDialog;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;

namespace RideSharing.App.ViewModels;

public class UserDetailViewModel : ViewModelBase, IUserDetailViewModel
{
    private readonly UserFacade _userFacade;
    private readonly IMediator _mediator;
    private readonly IMessageDialogService _messageDialogService;


    public UserWrapper? DetailModel { get; private set; }

    public ICommand DeleteUser { get; }
    public ICommand SaveChanges { get; }
    public ICommand ChangeImage { get; }

    public UserDetailViewModel(UserFacade userFacade, 
        IMessageDialogService messageDialogService,
        IMediator mediator) : base(mediator)
    {
        _userFacade = userFacade;
        _mediator = mediator;
        _messageDialogService = messageDialogService;


        DeleteUser = new RelayCommand(UserDeleted);
        SaveChanges = new AsyncRelayCommand(SaveAsync);
        ChangeImage = new AsyncRelayCommand<string>(ChangeImageAsync);
    }

    private async Task ChangeImageAsync(string? filePath)
    {
        if (filePath is null || DetailModel is null)
            return;

        var imageUrl = await UserFacade.UploadImageAsync(filePath);
        DetailModel.ImageUrl = imageUrl;
    }

    public override async void UserLoggedIn(LoginMessage<UserWrapper> obj)
    {
        base.UserLoggedIn(obj);

        if (obj.Model is not null)
            await LoadAsync(obj.Model.Id);
    }

    private async void UserUpdated(UpdateMessage<UserWrapper> _) => await LoadAsync(DetailModel.Id);
    private async void UserDeleted()
    {
        if (LoggedUser is null)
            return;

        var delete = _messageDialogService.Show(
            "Delete profile",
            $"Do you really want to delete your profile?",
            MessageDialogButtonConfiguration.DeleteCancel,
            MessageDialogResult.Cancel);

        if (delete == MessageDialogResult.Cancel)
        {
            return;
        }

        await _userFacade.DeleteAsync(LoggedUser.Id);
        _mediator.Send(new DeleteMessage<UserWrapper> { });
        _mediator.Send(new LogoutMessage<UserWrapper> { });
    }
    
    public async Task LoadAsync(Guid id)
    {
        DetailModel = await _userFacade.GetAsync(id) ?? throw new InvalidOperationException("Failed to load the selected ride");
    }
    public async Task SaveAsync()
    {
        if (DetailModel is null)
            return;

        if (!DetailModel!.IsValid)
        {
           _ = _messageDialogService.Show(
                "Error",
                $"Couldn't save profile due to invalid values.",
                MessageDialogButtonConfiguration.OK,
                MessageDialogResult.OK);

                return;

        }

        DetailModel = await _userFacade.SaveAsync(DetailModel);
        _mediator.Send(new UpdateMessage<UserWrapper> { Model = DetailModel });
    }

    public async Task DeleteAsync()
    {
        throw new NotImplementedException();
    }
}
