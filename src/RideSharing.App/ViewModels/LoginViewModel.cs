using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using RideSharing.App.Extensions;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.MessageDialog;
using RideSharing.App.ViewModels.Interfaces;
using RideSharing.App.Views;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;

namespace RideSharing.App.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IMediator _mediator;
    private readonly UserFacade _userFacade;
    //private readonly IMessageDialogService _messageDialogService;
    //private readonly UserWrapper? _model = UserDetailModel.Empty;

    public event EventHandler OnRequestClose;

    public LoginViewModel(UserFacade userFacade,
        IMessageDialogService messageDialogService,
        IMediator mediator)
    {
        _userFacade = userFacade;
        _mediator = mediator;

        DeleteUserCommand = new Commands.AsyncRelayCommand<UserListModel>(DeleteUserAsync);
        NewUserCommand = new RelayCommand(NewUser);

        mediator.Register<NewMessage<UserWrapper>>(UserAdded);
        mediator.Register<UpdateMessage<UserWrapper>>(UserUpdated);
        LoginCommand = new Commands.RelayCommand<Guid>(Login);

    }

    private void NewUser()
    {
        // TODO Open new user view
        throw new NotImplementedException();
    }

    private async Task DeleteUserAsync(UserListModel? arg1)
    {
        if (arg1 is null)
            return;

        await _userFacade.DeleteAsync(arg1.Id);
        await LoadAsync();
    }

    public ObservableCollection<UserListModel> Users { get; set; } = new();

    public ICommand LoginCommand { get; }
    public ICommand NewUserCommand { get; }
    public ICommand DeleteUserCommand { get; }

    private async void UserAdded(NewMessage<UserWrapper> _) => await LoadAsync();
    private async void UserUpdated(UpdateMessage<UserWrapper> _) => await LoadAsync();

    public async Task LoadAsync()
    {
        Users.Clear();
        var users = await _userFacade.GetUsers();
        Users.AddRange(users);
    }

    public void Login(Guid userId)
    {
        _mediator.Send(new SelectedMessage<UserWrapper>{Id = userId});
        OnRequestClose(this, EventArgs.Empty);
    }

    //public override void LoadInDesignMode()
    //{
    //    Users.Add(new UserListModel("John", "Smith", "700 858 999", "https://ownhubb.com/wp-content/uploads/2021/05/team.jpg"));
    //}
}
