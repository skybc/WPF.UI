// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Converters;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

/// <summary>
/// Takes the first N items from a collection.
/// </summary>
public class TakeItemsConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets the number of items to take.
    /// </summary>
    public int Count { get; set; } = 10;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable enumerable)
        {
            return enumerable.Cast<object>().Take(Count).ToList();
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Skips N items and takes M items from a collection.
/// </summary>
public class SkipTakeItemsConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets the number of items to skip.
    /// </summary>
    public int Skip { get; set; } = 0;

    /// <summary>
    /// Gets or sets the number of items to take.
    /// </summary>
    public int Count { get; set; } = 10;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable enumerable)
        {
            return enumerable.Cast<object>().Skip(Skip).Take(Count).ToList();
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
