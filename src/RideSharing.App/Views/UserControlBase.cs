using System.Windows;
using System.Windows.Controls;
using RideSharing.App.ViewModels;
using RideSharing.App.ViewModels.Interfaces;

namespace RideSharing.App.Views;

public abstract class UserControlBase : UserControl
{
    protected UserControlBase()
    {
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        //if (DataContext is IListViewModel viewModel)
        //    await viewModel.LoadAsync();
        if (DataContext is LoginViewModel loginViewModel)
            await loginViewModel.LoadAsync();
    }
}
