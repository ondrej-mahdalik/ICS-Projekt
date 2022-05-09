using System;
using System.Globalization;
using System.Windows.Data;
using RideSharing.Common.Enums;

namespace RideSharing.App.Converters;

public class VehicleTypeToIntConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return (int)VehicleType.Car;

        var temp = value as VehicleType?;
        return (int)(temp ?? VehicleType.Car);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Enum.TryParse(value.ToString(), out VehicleType result) ? result : VehicleType.Car;
    }
}
