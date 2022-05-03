using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RideSharing.App.Extensions;
using RideSharing.App.Services;
using RideSharing.App.Services.MessageDialog;
using RideSharing.App.Settings;
using RideSharing.App.ViewModels;
using RideSharing.App.Views;
using RideSharing.BL;
using RideSharing.DAL;
using RideSharing.DAL.Factories;

namespace RideSharing.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly IHost _host;

        public App()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(context.Configuration, services);
                }).Build();
        }

        private void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
        {
            builder.AddJsonFile(@"AppSettings.json", false, false);
        }

        private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddBLServices();
            services.Configure<DALSettings>(configuration.GetSection("RideSharing:DAL"));
            services.AddSingleton<IDbContextFactory<RideSharingDbContext>>(provider =>
            {
                var dalSettings = provider.GetRequiredService<IOptions<DALSettings>>().Value;
                return new SqlServerDbContextFactory(dalSettings.ConnectionString!,
                    dalSettings.SkipMigrationAndSeedDemoData);
            });

            services.AddSingleton<LoginWindow>();
            services.AddSingleton<MainWindow>();

            services.AddSingleton<IMessageDialogService, MessageDialogService>();
            services.AddSingleton<IMediator, Mediator>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<IDashboardViewModel, DashboardViewModel>();
            services.AddSingleton<IFindRideViewModel, FindRideViewModel>();
            services.AddSingleton<IRideDetailViewModel, RideDetailViewModel>();
            services.AddSingleton<IShareRideViewModel, ShareRideViewModel>();
            services.AddSingleton<IRideManagementViewModel, RideManagementViewModel>();
            services.AddSingleton<IUserDetailViewModel, UserDetailViewModel>();
            services.AddSingleton<IVehicleDetailViewModel, VehicleDetailViewModel>();
            services.AddSingleton<IVehicleListViewModel, VehicleListViewModel>();

            services.AddFactory<IDashboardViewModel, DashboardViewModel>();
            services.AddFactory<IFindRideViewModel, FindRideViewModel>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var dbContextFactory = _host.Services.GetRequiredService<IDbContextFactory<RideSharingDbContext>>();
            var dalSettings = _host.Services.GetRequiredService<IOptions<DALSettings>>().Value;

            await using (var dbx = await dbContextFactory.CreateDbContextAsync())
            {
                if (dalSettings.SkipMigrationAndSeedDemoData)
                {
                    await dbx.Database.EnsureDeletedAsync();
                    await dbx.Database.EnsureCreatedAsync();
                }
                else
                {
                    await dbx.Database.MigrateAsync();
                }
            }

            var loginViewModel = _host.Services.GetRequiredService<LoginViewModel>();
            var mainViewModel = _host.Services.GetRequiredService<MainViewModel>();
            var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();

            // Login
            loginViewModel.OnLogin += (_, _) =>
            {
                loginWindow.Hide();
                mainWindow.Show();
            };

            // Logout
            mainViewModel.OnLogout += (_, _) =>
            {
                mainWindow.Hide();
                loginWindow.Show();
            };

            // Handle closing from login window
            loginWindow.Closed += OnClosed;
            mainWindow.Closed += OnClosed;

            loginWindow.Show();
            base.OnStartup(e);
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            OnExit(default);
            Current.Shutdown();
        }

        protected override async void OnExit(ExitEventArgs? e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}
