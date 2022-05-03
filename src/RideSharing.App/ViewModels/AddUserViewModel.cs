using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideSharing.App.Commands;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.MessageDialog;
using RideSharing.App.Wrappers;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;

namespace RideSharing.App.ViewModels
{
    public class AddUserViewModel : ViewModelBase, IAddUserViewModel
    {
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;
        private readonly IMessageDialogService _messageDialogService;

        public AddUserViewModel(
            UserFacade userFacade,
            IMediator mediator,
            IMessageDialogService messageDialogService) : base(mediator)
        {
            _userFacade = userFacade;
            _mediator = mediator;
            _messageDialogService = messageDialogService;
        }

        public async Task LoadAsync()
        {

        }

        public async Task SaveAsync()
        {

        }
    }

    
}
