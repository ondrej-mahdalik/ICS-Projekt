using System;
using System.Globalization;
using System.Windows.Data;

namespace RideSharing.App.Converters;

public class TimespanToStringConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var timeSpan = (TimeSpan)value;

        if (timeSpan > TimeSpan.FromDays(1) && timeSpan.Hours != 0)
            return $"{timeSpan.Days} day{(timeSpan.Days > 1 ? 's' : string.Empty)} {timeSpan.Hours} hour{(timeSpan.Hours > 1 ? 's' : string.Empty)}";
        if (timeSpan > TimeSpan.FromDays(1))
            return $"{timeSpan.Days} day{(timeSpan.Days > 1 ? 's' : string.Empty)}";
        if (timeSpan > TimeSpan.FromHours(1) && timeSpan.Minutes != 0)
            return $"{timeSpan.Hours} hour{(timeSpan.Hours > 1 ? 's' : string.Empty)} {timeSpan.Minutes} minute{(timeSpan.Minutes > 1 ? 's' : string.Empty)}";
        if (timeSpan > TimeSpan.FromHours(1))
            return $"{timeSpan.Hours} hour{(timeSpan.Hours > 1 ? 's' : string.Empty)}";
        if (timeSpan > TimeSpan.FromMinutes(1))
            return $"{timeSpan.Minutes} minute{(timeSpan.Minutes > 1 ? 's' : string.Empty)}";

        return "< 1 minute";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
