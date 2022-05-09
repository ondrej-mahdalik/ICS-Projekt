using System;
using System.Globalization;
using System.Windows.Data;
using RideSharing.Common.Enums;

namespace RideSharing.App.Converters;

public class ViewIndexToIntConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return (int)ViewIndex.Dashboard;

        var temp = value as ViewIndex?;
        return (int)(temp ?? ViewIndex.Dashboard);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Enum.TryParse(value.ToString(), out ViewIndex result) ? result : ViewIndex.Dashboard;
    }
}
