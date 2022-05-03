using System;
using System.Threading.Tasks;

namespace RideSharing.App.ViewModels;

public interface ICreateViewModel<out TDetail> : IViewModel
{
    Task LoadAsync();
    Task SaveAsync();
}
