using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using RideSharing.App.Extensions;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.Dialogs;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.Common.Enums;

namespace RideSharing.App.ViewModels;

public class SelectUserViewModel : ViewModelBase, ISelectUserViewModel
{
    private readonly IMediator _mediator;
    private readonly UserFacade _userFacade;


    public SelectUserViewModel(UserFacade userFacade,
        IMediator mediator) : base(mediator)
    {
        _userFacade = userFacade;
        _mediator = mediator;

        NewUserCommand = new RelayCommand(NewUser);

        mediator.Register<UpdateMessage<UserWrapper>>(async _ => await LoadAsync());
        mediator.Register<AddedMessage<UserWrapper>>(async _ => await LoadAsync());
        mediator.Register<DeleteMessage<UserWrapper>>(async _ => await LoadAsync());

        mediator.Register<LogoutMessage<UserWrapper>>(async _ => await LoadAsync());

        mediator.Register<AddedMessage<VehicleWrapper>>(async _ => await LoadAsync());
        mediator.Register<DeleteMessage<VehicleWrapper>>(async _ => await LoadAsync());

        mediator.Register<AddedMessage<ReservationWrapper>>(async _ => await LoadAsync());
        mediator.Register<DeleteMessage<ReservationWrapper>>(async _ => await LoadAsync());

        mediator.Register<AddedMessage<RideWrapper>>(async _ => await LoadAsync());
        mediator.Register<DeleteMessage<RideWrapper>>(async _ => await LoadAsync());

        LoginCommand = new Commands.RelayCommand<Guid>(Login);
    }

    private void NewUser()
    {
        _mediator.Send(new NewMessage<UserWrapper>());
        _mediator.Send(new SwitchTabLoginMessage(LoginViewIndex.CreateUser));
    }
    
    public ObservableCollection<UserListModel> Users { get; set; } = new();

    public ICommand LoginCommand { get; }
    public ICommand NewUserCommand { get; }
    
    public async Task LoadAsync()
    {
        Users.Clear();
        var users = await _userFacade.GetUsers();
        Users.AddRange(users);
    }

    public UserWrapper? Model { get; private set; }
    public async void Login(Guid userId)
    {
        Model = await _userFacade.GetAsync(userId);
        _mediator.Send(new LoginMessage<UserWrapper> { Model = Model });
    }
}
