using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core.Raw;
using RideSharing.App.Commands;
using RideSharing.App.Extensions;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.ViewModels;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;

namespace RideSharing.App.ViewModels;

public class UserDetailViewModel : ViewModelBase
{
    private readonly UserFacade _userFacade;
    private readonly IMediator _mediator;
    private Guid? _loggedUserid;

    public ICommand DeleteUser { get; }
    public ICommand SaveChanges { get; }

    public UserDetailViewModel(UserFacade userFacade, IMediator mediator)
    {
        _userFacade = userFacade;
        _mediator = mediator;

        DeleteUser = new RelayCommand(UserDeleted);
        SaveChanges = new RelayCommand(SaveAsync);

        mediator.Register<UpdateMessage<UserWrapper>>(UserUpdated);
        mediator.Register<LoginMessage<UserWrapper>>(UserLoggedIn);

    }

    public UserWrapper Model { get; set; }

    private async void UserLoggedIn(LoginMessage<UserWrapper> obj)
    {
        _loggedUserid = obj.Id;
        await LoadAsync();
    }
    private async void UserUpdated(UpdateMessage<UserWrapper> _) => await LoadAsync();
    private async void UserDeleted()
    {
        if (_loggedUserid is null)
            return;
        
        await _userFacade.DeleteAsync((Guid)_loggedUserid);
        _mediator.Send(new DeleteMessage<UserWrapper> { });
        _mediator.Send(new LogoutMessage<UserWrapper> { });
    }
    
    public async Task LoadAsync()
    {
        if (_loggedUserid is null)
            return;

        Model = await _userFacade.GetAsync((Guid)_loggedUserid);
    }
    public async void  SaveAsync()
    {
        if (_loggedUserid is null)
            return;

        Model = await _userFacade.SaveAsync(Model);
        _mediator.Send(new UpdateMessage<UserWrapper> { Model = Model });
    }
}
