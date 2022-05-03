using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;

namespace RideSharing.App.ViewModels;

public class UserDetailViewModel : ViewModelBase, IUserDetailViewModel
{
    private readonly UserFacade _userFacade;
    private readonly IMediator _mediator;

    public UserWrapper? DetailModel { get; private set; }

    public ICommand DeleteUser { get; }
    public ICommand SaveChanges { get; }

    public UserDetailViewModel(UserFacade userFacade, IMediator mediator) : base(mediator)
    {
        _userFacade = userFacade;
        _mediator = mediator;

        DeleteUser = new RelayCommand(UserDeleted);
        SaveChanges = new AsyncRelayCommand(SaveAsync);
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

        DetailModel = await _userFacade.SaveAsync(DetailModel);
        _mediator.Send(new UpdateMessage<UserWrapper> { Model = DetailModel });
    }

    public async Task DeleteAsync()
    {
        throw new NotImplementedException();
    }
}
