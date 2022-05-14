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

        DeleteUserCommand = new Commands.AsyncRelayCommand<UserListModel>(DeleteUserAsync);
        NewUserCommand = new RelayCommand(NewUser);

        mediator.Register<UpdateMessage<UserWrapper>>(UserUpdated);
        mediator.Register<AddedMessage<UserWrapper>>(UserAdded);
        mediator.Register<DeleteMessage<UserWrapper>>(UserDeleted);
        mediator.Register<DeleteMessage<VehicleWrapper>>(async _ => await LoadAsync());

        LoginCommand = new Commands.RelayCommand<Guid>(Login);

    }

    private void NewUser()
    {
        _mediator.Send(new NewMessage<UserWrapper>());
        _mediator.Send(new SwitchTabLoginMessage(LoginViewIndex.CreateUser));
    }

    private async Task DeleteUserAsync(UserListModel? model)
    {
        if (model is null)
            return;

        var delete = await DialogHost.Show(new MessageDialog("Delete Profile", $"Do you really want to delete profile {Model?.Name} {Model?.Surname}?", DialogType.YesNo));
        if (delete is not ButtonType.Yes)
            return;

        await _userFacade.DeleteAsync(model.Id);
        await LoadAsync();
    }

    public ObservableCollection<UserListModel> Users { get; set; } = new();

    public ICommand LoginCommand { get; }
    public ICommand NewUserCommand { get; }
    public ICommand DeleteUserCommand { get; }

    private async void UserAdded(AddedMessage<UserWrapper> _) => await LoadAsync();
    private async void UserUpdated(UpdateMessage<UserWrapper> _) => await LoadAsync();
    private async void UserDeleted(DeleteMessage<UserWrapper> _) => await LoadAsync();

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
