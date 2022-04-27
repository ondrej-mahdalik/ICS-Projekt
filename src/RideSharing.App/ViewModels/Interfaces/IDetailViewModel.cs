﻿using System;
using System.Threading.Tasks;

namespace RideSharing.App.ViewModels.Interfaces
{
    public interface IDetailViewModel<out TDetail> : IViewModel
    {
        TDetail? Vehicle { get; }
        Task LoadAsync(Guid id);
        Task DeleteAsync();
        Task SaveAsync();
    }
}
