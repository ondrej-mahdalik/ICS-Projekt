using System;
using System.CodeDom;
using System.Windows;
using System.Windows.Input;
using RideSharing.Common.Enums;

namespace RideSharing.App.Controls;
/// <summary>
/// Interakční logika pro FindRideItem.xaml
/// </summary>
public partial class FindRideItem
{
    public string UserName
    {
        get => (string)GetValue(UserNameProperty);
        set => SetValue(UserNameProperty, value);
    }

    public string UserSurname
    {
        get => (string)GetValue(UserSurnameProperty);
        set => SetValue(UserSurnameProperty, value);
    }

    public string Rating
    {
        get => (string)GetValue(RatingProperty);
        set => SetValue(RatingProperty, value);
    }

    public string ReviewCount
    {
        get => (string)GetValue(ReviewCountProperty);
        set => SetValue(ReviewCountProperty, value);
    }

    public string From
    {
        get => (string)GetValue(FromProperty);
        set => SetValue(FromProperty, value);
    }

    public string To
    {
        get => (string)GetValue(ToProperty);
        set => SetValue(ToProperty, value);
    }

    public string Departure
    {
        get => (string)GetValue(DepartureProperty);
        set => SetValue(DepartureProperty, value);
    }

    public string Arrival
    {
        get => (string)GetValue(ArrivalProperty);
        set => SetValue(ArrivalProperty, value);
    }

    public string Distance
    {
        get => (string)GetValue(DistanceProperty);
        set => SetValue(DistanceProperty, value);
    }

    public string Occupied
    {
        get => (string)GetValue(OccupiedProperty);
        set => SetValue(OccupiedProperty, value);
    }

    public VehicleType VehicleType
    {
        get => (VehicleType)GetValue(VehicleTypeProperty);
        set => SetValue(VehicleTypeProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly DependencyProperty UserNameProperty = DependencyProperty.Register(
        nameof(UserName), typeof(string), typeof(FindRideItem), new PropertyMetadata(default));

    public static readonly DependencyProperty UserSurnameProperty = DependencyProperty.Register(
        nameof(UserSurname), typeof(string), typeof(FindRideItem), new PropertyMetadata(default));

    public static readonly DependencyProperty RatingProperty = DependencyProperty.Register(
        nameof(Rating), typeof(string), typeof(FindRideItem), new PropertyMetadata(default));

    public static readonly DependencyProperty ReviewCountProperty = DependencyProperty.Register(
        nameof(ReviewCount), typeof(string), typeof(FindRideItem), new PropertyMetadata(default));

    public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
        nameof(From), typeof(string), typeof(FindRideItem), new PropertyMetadata(default));

    public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
        nameof(To), typeof(string), typeof(FindRideItem), new PropertyMetadata(default));

    public static readonly DependencyProperty DepartureProperty = DependencyProperty.Register(
        nameof(Departure), typeof(string), typeof(FindRideItem), new PropertyMetadata(default));

    public static readonly DependencyProperty ArrivalProperty = DependencyProperty.Register(
        nameof(Arrival), typeof(string), typeof(FindRideItem), new PropertyMetadata(default));

    public static readonly DependencyProperty DistanceProperty = DependencyProperty.Register(
        nameof(Distance), typeof(string), typeof(FindRideItem), new PropertyMetadata(default));

    public static readonly DependencyProperty OccupiedProperty = DependencyProperty.Register(
        nameof(Occupied), typeof(string), typeof(FindRideItem), new PropertyMetadata(default));

    public static readonly DependencyProperty VehicleTypeProperty = DependencyProperty.Register(
        nameof(VehicleType), typeof(string), typeof(FindRideItem), new PropertyMetadata(default));

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command), typeof(ICommand), typeof(FindRideItem), new PropertyMetadata(default));

    public FindRideItem()
    {
        InitializeComponent();
    }
}
