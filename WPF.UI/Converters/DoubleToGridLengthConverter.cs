using System;
using System.Globalization;
using System.Windows;

namespace Wpf.Ui.Converters;

/// <summary>
/// Converts a double value to a GridLength value.
/// If the double is 0, returns GridLength.Auto. Otherwise, returns GridLength with fixed width.
/// </summary>
public sealed class DoubleToGridLengthConverter : System.Windows.Data.IValueConverter
{
    /// <summary>
    /// Converts a double value to GridLength.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double d)
        {
            // If width is 0, use Auto
            if (d == 0d)
            {
                return GridLength.Auto;
            }

            return new GridLength(d);
        }

        return GridLength.Auto;
    }

    /// <summary>
    /// Not implemented. This converter is not meant for two-way binding.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
