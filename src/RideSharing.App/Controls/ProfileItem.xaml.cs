using System;
using System.Windows;
using System.Windows.Input;

namespace RideSharing.App.Controls
{
    /// <summary>
    /// Interakční logika pro ProfileItem.xaml
    /// </summary>
    public partial class ProfileItem
    {
        public Guid ProfileId
        {
            get => (Guid)GetValue(ProfileIdProperty);
            set => SetValue(ProfileIdProperty, value);
        }

        public string ProfileName
        {
            get => (string)GetValue(ProfileNameProperty);
            set => SetValue(ProfileNameProperty, value);
        }

        public string OwnedVehicles
        {
            get => (string)GetValue(OwnedVehiclesProperty);
            set => SetValue(OwnedVehiclesProperty, value);
        }

        public string UpcomingRides
        {
            get => (string)GetValue(UpcomingRidesProperty);
            set => SetValue(UpcomingRidesProperty, value);
        }

        public string ImageUrl
        {
            get => (string)GetValue(ImageUrlProperty);
            set => SetValue(ImageUrlProperty, value);
        }

        public ICommand LoginCommand
        {
            get => (ICommand)GetValue(LoginCommandProperty);
            set => SetValue(LoginCommandProperty, value);
        }

        public ICommand DeleteCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            set => SetValue(DeleteCommandProperty, value);
        }

        public static readonly DependencyProperty ProfileIdProperty = DependencyProperty.Register(
            nameof(ProfileId), typeof(Guid), typeof(ProfileItem), new PropertyMetadata(default));

        public static readonly DependencyProperty ProfileNameProperty = DependencyProperty.Register(
            nameof(ProfileName), typeof(string), typeof(ProfileItem), new PropertyMetadata(default));

        public static readonly DependencyProperty OwnedVehiclesProperty = DependencyProperty.Register(
            nameof(OwnedVehicles), typeof(string), typeof(ProfileItem), new PropertyMetadata(default));

        public static readonly DependencyProperty UpcomingRidesProperty = DependencyProperty.Register(
            nameof(UpcomingRides), typeof(string), typeof(ProfileItem), new PropertyMetadata(default));

        public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register(
            nameof(ImageUrl), typeof(string), typeof(ProfileItem), new PropertyMetadata(default));

        public static readonly DependencyProperty LoginCommandProperty = DependencyProperty.Register(
            nameof(LoginCommand), typeof(ICommand), typeof(ProfileItem), new PropertyMetadata(default));

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(
            nameof(DeleteCommand), typeof(ICommand), typeof(ProfileItem), new PropertyMetadata(default));

        public ProfileItem()
        {
            InitializeComponent();
        }
    }
}
