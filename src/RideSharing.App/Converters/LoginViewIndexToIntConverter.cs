using System;
using System.Globalization;
using System.Windows.Data;
using RideSharing.Common.Enums;

namespace RideSharing.App.Converters;

public class LoginViewIndexToIntConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return (int)LoginViewIndex.SelectUser;

        var temp = value as LoginViewIndex?;
        return (int)(temp ?? LoginViewIndex.SelectUser);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Enum.TryParse(value.ToString(), out LoginViewIndex result) ? result : LoginViewIndex.SelectUser;
    }
}
