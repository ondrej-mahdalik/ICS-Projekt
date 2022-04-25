using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Extensions;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.MessageDialog;
using RideSharing.App.ViewModels.Interfaces;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;

namespace RideSharing.App.ViewModels;

public class LoginViewModel : ViewModelBase, ILoginViewModel
{
    private readonly IMediator _mediator;
    private readonly UserFacade _userFacade;
    //private readonly IMessageDialogService _messageDialogService;
    //private readonly UserWrapper? _model = UserDetailModel.Empty;

    public LoginViewModel(UserFacade userFacade,
        IMessageDialogService messageDialogService,
        IMediator mediator)
    {
        _userFacade = userFacade;
        _mediator = mediator;

        mediator.Register<NewMessage<UserWrapper>>(UserAdded);
        mediator.Register<UpdateMessage<UserWrapper>>(UserUpdated);
        mediator.Register<DeleteMessage<UserWrapper>>(UserDeleted);
        LoginCommand = new AsyncRelayCommand(Login);

    }

    public ObservableCollection<UserListModel> Users { get; set; } = new();

    public ICommand LoginCommand { get; }
    public ICommand NewUserCommand { get; }
    public ICommand DeleteUserCommand { get; }

    private void NewUser() => _mediator.Send(new NewMessage<UserWrapper>());
    private void DeleteUser() => _mediator.Send(new DeleteMessage<UserWrapper>());

    private async void UserAdded(NewMessage<UserWrapper> _) => await LoadAsync();
    private async void UserUpdated(UpdateMessage<UserWrapper> _) => await LoadAsync();
    private async void UserDeleted(DeleteMessage<UserWrapper> _) => await LoadAsync();

    public async Task LoadAsync()
    {
        Users.Clear();
        var users = await _userFacade.GetAsync();
        Users.AddRange(users);
    }

    public async Task Login()
    {

    }

    public override void LoadInDesignMode()
    {
        Users.Add(new UserListModel("John", "Smith", "700 858 999", "https://ownhubb.com/wp-content/uploads/2021/05/team.jpg"));
    }
}
