using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.Dialogs;
using RideSharing.App.Wrappers;
using RideSharing.BL;
using RideSharing.BL.Facades;
using RideSharing.Common.Enums;

namespace RideSharing.App.ViewModels;

public class UserDetailViewModel : ViewModelBase, IUserDetailViewModel
{
    private readonly UserFacade _userFacade;
    private readonly IMediator _mediator;
    private readonly ISnackbarMessageQueue _messageQueue;


    public UserWrapper? DetailModel { get; private set; }

    public bool IsValid
    {
        get => DetailModel is not null && DetailModel.IsValid && !UploadingImage;
    }

    public ICommand DeleteUser { get; }
    public ICommand SaveChanges { get; }
    public ICommand ChangeImage { get; }

    public UserDetailViewModel(UserFacade userFacade,
        IMediator mediator,
        ISnackbarMessageQueue messageQueue) : base(mediator)
    {
        _userFacade = userFacade;
        _mediator = mediator;
        _messageQueue = messageQueue;

        DeleteUser = new RelayCommand(UserDeleted);
        SaveChanges = new AsyncRelayCommand(SaveAsync);
        ChangeImage = new AsyncRelayCommand<string>(ChangeImageAsync);
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
            _messageQueue.Enqueue("Image has been uploaded.");
        }
        finally
        {
            UploadingImage = false;
        }
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
        await DeleteAsync();
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
            await DialogHost.Show(new MessageDialog("Saving Failed",
                "Could not save the changes due to invalid values.", DialogType.OK));

            return;
        }

        try
        {
            DetailModel = await _userFacade.SaveAsync(DetailModel);
            _mediator.Send(new UpdateMessage<UserWrapper> { Model = DetailModel });
            _mediator.Send(new SwitchTabMessage(ViewIndex.Dashboard));
            _messageQueue.Enqueue("Changes have been successfully saved");
        }
        catch
        {
            await DialogHost.Show(new MessageDialog("Saving Failed", "Failed to save the changes.", DialogType.OK));
        }
    }

    public async Task DeleteAsync()
    {
        if (LoggedUser is null)
            return;

        var delete = await DialogHost.Show(new MessageDialog("Delete Profile", "Do you really want to delete your profile?",
            DialogType.YesNo));
        if (delete is not ButtonType.Yes)
            return;

        await _userFacade.DeleteAsync(LoggedUser.Id);
        _mediator.Send(new DeleteMessage<UserWrapper> { });
        _mediator.Send(new LogoutMessage<UserWrapper> { });
    }
}
