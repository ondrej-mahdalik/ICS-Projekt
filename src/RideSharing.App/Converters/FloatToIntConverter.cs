using System;
using System.Globalization;
using System.Windows.Data;

namespace RideSharing.App.Converters;

public class FloatToIntConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null || value.Equals(float.NaN))
            return 0;

        return (int)(float)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
