using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using System.Windows.Input;
using GoogleApi.Entities.Maps.Common;
using MaterialDesignThemes.Wpf;
using RideSharing.App.Messages;
using RideSharing.App.Services;
using RideSharing.App.Services.Dialogs;
using RideSharing.App.Wrappers;
using RideSharing.BL;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.Common.Enums;
using AsyncRelayCommand = RideSharing.App.Commands.AsyncRelayCommand;

namespace RideSharing.App.ViewModels
{
    public class ShareRideViewModel : ViewModelBase, IShareRideViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly VehicleFacade _vehicleFacade;
        private readonly ReservationFacade _reservationFacade;
        private readonly IMediator _mediator;
        private readonly ISnackbarMessageQueue _messageQueue;

        public ShareRideViewModel(
            RideFacade rideFacade,
            VehicleFacade vehicleFacade,
            ReservationFacade reservationFacade,
            IMediator mediator,
            ISnackbarMessageQueue messageQueue) : base(mediator)
        {
            _rideFacade = rideFacade;
            _vehicleFacade = vehicleFacade;
            _reservationFacade = reservationFacade;
            _mediator = mediator;
            _messageQueue = messageQueue;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            UpdateRouteCommand = new AsyncRelayCommand(UpdateRoute);
            ClearCommand = new AsyncRelayCommand(ClearView);

            mediator.Register<AddedMessage<VehicleWrapper>>(async _ => await LoadAsync());
            mediator.Register<UpdateMessage<VehicleWrapper>>(async _ => await LoadAsync());
            mediator.Register<DeleteMessage<VehicleWrapper>>(async _ => await LoadAsync());
        }

        

        public RideWrapper? DetailModel { get; set; }
        public List<VehicleListModel>? Vehicles { get; set; }

        private VehicleListModel? _selectedVehicle;
        public VehicleListModel? SelectedVehicle
        {
            get { return _selectedVehicle; }
            set
            {
                _selectedVehicle = value;
                if (DetailModel is not null)
                {
                    DetailModel.SharedSeats = 1;
                }
            }
        }


        public ICommand SaveCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand UpdateRouteCommand { get; }

        public override async void UserLoggedIn(LoginMessage<UserWrapper> obj)
        {
            base.UserLoggedIn(obj);

            await LoadAsync();
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                ArrTime = ArrDate = Combine(DepDate, DepTime) + Duration;
            }
        }

        private DateTime _depTime;
        public DateTime DepTime
        {
            get { return _depTime; }
            set
            {
                _depTime = value;
                ArrTime = ArrDate = Combine(DepDate, DepTime) + Duration;
            }
        }

        private DateTime _depDate;
        public DateTime DepDate
        {
            get { return _depDate; }
            set
            {
                _depDate = value;
                ArrTime = ArrDate = Combine(DepDate, DepTime) + Duration;
            }
        }

        public DateTime ArrTime { get; set; }

        public DateTime ArrDate { get; set; }

        public async Task ClearView()
        {
            await LoadAsync();
            SelectedVehicle = null;
        }

        public async Task LoadAsync()
        {
            if (LoggedUser is null)
                return;

            MapEnabled = false;
            DetailModel = new(RideDetailModel.Empty);
            Vehicles = await _vehicleFacade.GetByOwnerAsync(LoggedUser.Id);
            ArrDate = DepDate = ArrTime = DepTime = DateTime.Now;
            Duration = TimeSpan.Zero;
            RefreshMap();
        }

        public bool MapEnabled { get; set; }

        public bool CanSave() => DetailModel is not null && DetailModel.Distance > 0 && Combine(ArrDate, ArrTime) > Combine(DepDate, DepTime) && Combine(ArrDate, ArrTime) > DateTime.Now &&
                                 Combine(DepDate, DepTime) > DateTime.Now &&  SelectedVehicle is not null && (Vehicles?.Any(x => x.Id == SelectedVehicle.Id) ?? false);
        
        public async Task SaveAsync()
        {
            if (LoggedUser is null || DetailModel is null)
                return;

            // Check for conflicting rides/reservations
            if (await _reservationFacade.HasConflictingRide(LoggedUser.Id, DetailModel.Departure, DetailModel.Arrival))
            {
                await DialogHost.Show(new MessageDialog("Conficting Ride or Reservation",
                    "You are already signed up for another ride at the entered date and time.", DialogType.OK));
                return;
            }

            try
            {
                DetailModel.VehicleId = SelectedVehicle!.Id;
                DetailModel.Arrival = Combine(ArrDate, ArrTime);
                DetailModel.Departure = Combine(DepDate, DepTime);
                await _rideFacade.SaveAsync(DetailModel);
                _mediator.Send(new AddedMessage<RideWrapper>());
                _mediator.Send(new SwitchTabMessage(ViewIndex.Dashboard));
                _messageQueue.Enqueue("Ride has been successfully created");

                await ClearView();
            }
            catch
            {
                await DialogHost.Show(new MessageDialog("Creating Failed", "Failed to create the ride.", DialogType.OK));
            }
        }

        public DateTime Today { get; set; } = DateTime.Now;
        
        private async Task UpdateRoute()
        {
            if (DetailModel?.FromName is null || DetailModel?.ToName is null)
                return;

            var routeInfo = await BusinessLogic.GetRouteInfoAsync(DetailModel.FromName, DetailModel.ToName);
            if (!routeInfo.Item1)
                return;

            DetailModel.Distance = routeInfo.Item2!.Value;
            Duration = routeInfo.Item3!.Value;
        }

        private void RefreshMap()
        {
            MapEnabled = true;
            MapEnabled = false;
        }

        private DateTime Combine(DateTime date, DateTime time)
        {
            return date.Date.Add(time.TimeOfDay);
        }

    }


}
