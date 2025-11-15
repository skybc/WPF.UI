// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

/// <summary>
/// Converts a <see cref="FontSettings"/> object to its <see cref="FontFamily"/> property.
/// </summary>
[ValueConversion(typeof(FontSettings), typeof(FontFamily))]
public class FontFamilyExtractorConverter : IValueConverter
{
    public object? Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        return (value as FontSettings)?.FontFamily ?? new FontFamily("Segoe UI");
    }

    public object ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a <see cref="FontSettings"/> object to its <see cref="FontSize"/> property.
/// </summary>
[ValueConversion(typeof(FontSettings), typeof(double))]
public class FontSizeExtractorConverter : IValueConverter
{
    public object? Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        return (value as FontSettings)?.FontSize ?? 14.0;
    }

    public object ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a <see cref="FontSettings"/> object to its <see cref="FontWeight"/> property.
/// </summary>
[ValueConversion(typeof(FontSettings), typeof(FontWeight))]
public class FontWeightExtractorConverter : IValueConverter
{
    public object? Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        return (value as FontSettings)?.FontWeight ?? FontWeights.Normal;
    }

    public object ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a <see cref="FontStyle"/> object to its <see cref="FontStyle"/> property.
/// </summary>
[ValueConversion(typeof(FontSettings), typeof(FontStyle))]
public class FontStyleExtractorConverter : IValueConverter
{
    public object? Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        return (value as FontSettings)?.FontStyle ?? FontStyles.Normal;
    }

    public object ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a <see cref="Brush"/> to a <see cref="Color"/>.
/// </summary>
[ValueConversion(typeof(Brush), typeof(Color))]
public class BrushToColorConverter : IValueConverter
{
    public object? Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush solidBrush)
        {
            return solidBrush.Color;
        }
        return Colors.Black;
    }

    public object ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Color color)
        {
            return new SolidColorBrush(color);
        }
        return new SolidColorBrush(Colors.Black);
    }
}

/// <summary>
/// Converts a <see cref="Color"/> to a <see cref="Brush"/>.
/// </summary>
[ValueConversion(typeof(Color), typeof(Brush))]
public class ColorToBrushConverter : IValueConverter
{
    public object? Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Color color)
        {
            return new SolidColorBrush(color);
        }
        return new SolidColorBrush(Colors.Black);
    }

    public object ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush solidBrush)
        {
            return solidBrush.Color;
        }
        return Colors.Black;
    }
}

/// <summary>
/// Converts a <see cref="FontSettings"/> object to its <see cref="Foreground"/> property.
/// </summary>
[ValueConversion(typeof(FontSettings), typeof(Brush))]
public class ForegroundExtractorConverter : IValueConverter
{
    public object? Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        return (value as FontSettings)?.Foreground ?? new SolidColorBrush(Colors.Black);
    }

    public object ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
