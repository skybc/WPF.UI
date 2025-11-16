// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Globalization;
using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Converters;

/// <summary>
/// 数字加法转换器 - 将一个数字加上指定的值
/// Numeric Addition Converter - Adds a value to a number
/// </summary>
public class AddOneConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return "0";

        if (!int.TryParse(value.ToString(), out int intValue))
            return value.ToString();

        // 默认加 1
        int addValue = 1;
        
        // 如果提供了参数，使用参数值
        if (parameter != null && int.TryParse(parameter.ToString(), out int paramValue))
        {
            addValue = paramValue == 0 ? 1 : paramValue;
        }

        return (intValue + addValue).ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
