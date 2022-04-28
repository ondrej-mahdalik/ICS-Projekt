using System;
using System.Windows;

namespace RideSharing.App.Controls
{
    /// <summary>
    /// Interakční logika pro FromToBlock.xaml
    /// </summary>
    public partial class FromToBlock
    {
        public string FromName
        {
            get => (string)GetValue(FromNameProperty);
            set => SetValue(FromNameProperty, value);
        }

        public string ToName
        {
            get => (string)GetValue(ToNameProperty);
            set => SetValue(ToNameProperty, value);
        }

        public DateTime Departure
        {
            get => (DateTime)GetValue(DepartureProperty);
            set => SetValue(DepartureProperty, value);
        }

        public DateTime Arrival
        {
            get => (DateTime)GetValue(ArrivalProperty);
            set => SetValue(ArrivalProperty, value);
        }

        public static readonly DependencyProperty FromNameProperty = DependencyProperty.Register(
            nameof(FromName), typeof(string), typeof(FromToBlock), new PropertyMetadata(default));

        public static readonly DependencyProperty ToNameProperty = DependencyProperty.Register(
            nameof(ToName), typeof(string), typeof(FromToBlock), new PropertyMetadata(default));

        public static readonly DependencyProperty DepartureProperty = DependencyProperty.Register(
            nameof(Departure), typeof(DateTime), typeof(FromToBlock), new PropertyMetadata(default));

        public static readonly DependencyProperty ArrivalProperty = DependencyProperty.Register(
            nameof(Arrival), typeof(DateTime), typeof(FromToBlock), new PropertyMetadata(default));

        public FromToBlock()
        {
            InitializeComponent();
        }
    }
}
