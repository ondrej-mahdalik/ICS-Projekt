using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;

namespace RideSharing.App.ViewModels;

public class UserDetailViewModel : ViewModelBase
{
    private readonly UserFacade _userFacade;
    private readonly IMediator _mediator;

    public ICommand DeleteUser { get; }
    public ICommand SaveChanges { get; }

    public UserDetailViewModel(UserFacade userFacade, IMediator mediator) : base(mediator)
    {
        _userFacade = userFacade;
        _mediator = mediator;

        DeleteUser = new RelayCommand(UserDeleted);
        SaveChanges = new RelayCommand(SaveAsync);
    }

    public UserWrapper? Model { get; set; }

    public override async void UserLoggedIn(LoginMessage<UserWrapper> obj)
    {
        base.UserLoggedIn(obj);
        await LoadAsync();
    }

    private async void UserUpdated(UpdateMessage<UserWrapper> _) => await LoadAsync();
    private async void UserDeleted()
    {
        if (LoggedUser is null)
            return;
        
        await _userFacade.DeleteAsync(LoggedUser.Id);
        _mediator.Send(new DeleteMessage<UserWrapper> { });
        _mediator.Send(new LogoutMessage<UserWrapper> { });
    }
    
    public async Task LoadAsync()
    {
        if (LoggedUser is null)
            return;

        Model = await _userFacade.GetAsync(LoggedUser.Id) ?? throw new InvalidOperationException("Failed to load the selected ride");
    }
    public async void  SaveAsync()
    {
        if (LoggedUser is null && Model is null)
            return;

        Model = await _userFacade.SaveAsync(Model);
        _mediator.Send(new UpdateMessage<UserWrapper> { Model = Model });
    }
}
