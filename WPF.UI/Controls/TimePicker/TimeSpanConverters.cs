// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

using System;
using System.Globalization;
using System.Windows.Data;

/// <summary>
/// Converts a TimeSpan to a formatted hour string (HH).
/// </summary>
[ValueConversion(typeof(TimeSpan), typeof(string))]
public class TimeSpanToHourConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan)
        {
            return timeSpan.Hours.ToString("D2");
        }
        return "00";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a TimeSpan to a formatted minute string (mm).
/// </summary>
[ValueConversion(typeof(TimeSpan), typeof(string))]
public class TimeSpanToMinuteConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan)
        {
            return timeSpan.Minutes.ToString("D2");
        }
        return "00";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a TimeSpan to a formatted AM/PM string.
/// </summary>
[ValueConversion(typeof(TimeSpan), typeof(string))]
public class TimeSpanToAMPMConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan)
        {
            return timeSpan.Hours >= 12 ? "PM" : "AM";
        }
        return "AM";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
