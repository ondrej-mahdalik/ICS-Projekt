using System;
using System.Threading.Tasks;
using RideSharing.App.ViewModels.Interfaces;
using RideSharing.BL.Facades;

namespace RideSharing.App.ViewModels;

public class DashboardViewModel : ViewModelBase, IDashboardViewModel
{
    private readonly UserFacade _userFacade;

    public DashboardViewModel(UserFacade userFacade) // TODO add mediator
    {
        _userFacade = userFacade;
    }


#pragma warning disable CS1998 // V této asynchronní metodě chybí operátory await a spustí se synchronně.
    public async Task LoadAsync()
#pragma warning restore CS1998 // V této asynchronní metodě chybí operátory await a spustí se synchronně.
    {
        throw new NotImplementedException();
    }
}
