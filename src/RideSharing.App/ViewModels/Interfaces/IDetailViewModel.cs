using System;
using System.Threading.Tasks;

namespace RideSharing.App.ViewModels
{
    public interface IDetailViewModel<out TDetail> : IViewModel
    {
        TDetail? DetailModel { get; }
        Task LoadAsync(Guid id);
        Task DeleteAsync();
        Task SaveAsync();
    }
}
