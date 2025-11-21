// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Data;

namespace Wpf.Ui.Converters;

public class EnumToBoolConverter<TEnum> : IValueConverter
    where TEnum : Enum
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not TEnum valueEnum)
        {
            throw new ArgumentException($"{nameof(value)} is not type: {typeof(TEnum)}");
        }

        if (parameter is not TEnum parameterEnum)
        {
            throw new ArgumentException($"{nameof(parameter)} is not type: {typeof(TEnum)}");
        }

        return EqualityComparer<TEnum>.Default.Equals(valueEnum, parameterEnum);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class EnumToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null || parameter is null)
        {
            return false;
        }

        if (!value.GetType().IsEnum)
        {
            throw new ArgumentException($"{nameof(value)} is not an enum type");
        }

        if (!parameter.GetType().IsEnum)
        {
            throw new ArgumentException($"{nameof(parameter)} is not an enum type");
        }

        if (value.GetType() != parameter.GetType())
        {
            throw new ArgumentException($"{nameof(value)} and {nameof(parameter)} must be the same enum type");
        }

        return value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool boolValue || !boolValue || parameter is null)
        {
            return Binding.DoNothing;
        }

        return parameter;
    }
}