using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RideSharing.App.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not null && parameter.ToString() ==  "true") // Hidden when true
        {
            if (value is null)
                return Visibility.Visible;

            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }
        else // Visible when false
        {
            if (value is null)
                return Visibility.Collapsed;

            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
