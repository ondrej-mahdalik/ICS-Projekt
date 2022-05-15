using System;
using System.Globalization;
using System.Windows.Data;

namespace RideSharing.App.Converters;

public class IntSubtractConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return 0;

        return (UInt16) value - 1;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
