// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Converters;

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

/// <summary>
/// Converts between Color and Hex string representation.
/// </summary>
public class ColorToHexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Color color)
        {
            if (color.A == 255)
            {
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        return "#FFFFFF";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string hex)
        {
            hex = hex.TrimStart('#');

            try
            {
                if (hex.Length == 6)
                {
                    return Color.FromRgb(
                        System.Convert.ToByte(hex.Substring(0, 2), 16),
                        System.Convert.ToByte(hex.Substring(2, 2), 16),
                        System.Convert.ToByte(hex.Substring(4, 2), 16));
                }
                else if (hex.Length == 8)
                {
                    return Color.FromArgb(
                        System.Convert.ToByte(hex.Substring(0, 2), 16),
                        System.Convert.ToByte(hex.Substring(2, 2), 16),
                        System.Convert.ToByte(hex.Substring(4, 2), 16),
                        System.Convert.ToByte(hex.Substring(6, 2), 16));
                }
            }
            catch
            {
                // Invalid hex string, return white
            }
        }

        return Colors.White;
    }
}
