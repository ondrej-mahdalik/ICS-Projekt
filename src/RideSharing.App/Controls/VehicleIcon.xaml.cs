using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using RideSharing.Common.Enums;

namespace RideSharing.App.Controls
{
    /// <summary>
    /// Interakční logika pro FromToBlock.xaml
    /// </summary>
    public partial class VehicleIcon : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public VehicleType VehicleType
        {
            get => (VehicleType)GetValue(VehicleTypeProperty);
            set => SetValue(VehicleTypeProperty, value);
        }

        public static readonly DependencyProperty VehicleTypeProperty = DependencyProperty.Register(
            nameof(VehicleType), typeof(string), typeof(VehicleIcon), new PropertyMetadata(default));

        public VehicleIcon()
        {
            InitializeComponent();
            PropertyChanged += VehicleIcon_PropertyChanged;
        }

        private void VehicleIcon_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            CanvasBicycle.Visibility = Visibility.Hidden;
            CanvasMotorcycle.Visibility = Visibility.Hidden;
            CanvasCar.Visibility = Visibility.Hidden;
            CanvasVan.Visibility = Visibility.Hidden;
            CanvasMinibus.Visibility = Visibility.Hidden;
            CanvasBus.Visibility = Visibility.Hidden;

            switch (VehicleType)
            {
                case VehicleType.Bicycle:
                    CanvasBicycle.Visibility = Visibility.Visible;
                    break;
                case VehicleType.Motorcycle:
                    CanvasMotorcycle.Visibility = Visibility.Visible;
                    break;
                case VehicleType.Van:
                    CanvasVan.Visibility = Visibility.Visible;
                    break;
                case VehicleType.Minibus:
                    CanvasMinibus.Visibility = Visibility.Visible;
                    break;
                case VehicleType.Bus:
                    CanvasBus.Visibility = Visibility.Visible;
                    break;
                case VehicleType.Car:
                default:
                    CanvasCar.Visibility = Visibility.Visible;
                    break;

            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
