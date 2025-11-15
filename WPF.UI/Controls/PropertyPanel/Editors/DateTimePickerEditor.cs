// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor for date and time picker properties.
/// </summary>
internal class DateTimePickerEditor : PropertyEditorBase
{
    public override PropertyEditorKind EditorKind => PropertyEditorKind.DateTimeSelect;

    public override bool CanHandle(PropertyItem item) =>
        item.Attribute.Editor == PropertyEditorKind.DateTimeSelect || 
        (item.UnderlyingType == typeof(DateTime));

    public override FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
    {
        var stackPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Spacing = 8,
        };

        // Date picker
        var datePicker = new DatePicker
        {
            MinWidth = 120,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        // Time picker (using TextBox as fallback if TimePicker is not available)
        var timePicker = new TextBox
        {
            MinWidth = 80,
            HorizontalAlignment = HorizontalAlignment.Left,
            Padding = new Thickness(8, 4, 8, 4),
        };

        var binding = CreateValueBinding(item, UpdateSourceTrigger.LostFocus);

        // Bind date part
        datePicker.SetBinding(DatePicker.SelectedDateProperty, new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            Converter = new DateTimeToDateConverter(),
        });

        // Bind time part
        timePicker.SetBinding(TextBox.TextProperty, new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
            Converter = new DateTimeToTimeConverter(),
            StringFormat = "HH:mm:ss",
        });

        stackPanel.Children.Add(datePicker);
        stackPanel.Children.Add(timePicker);

        SetCommonProperties(stackPanel, item);

        return stackPanel;
    }

    /// <summary>
    /// Converter for DateTime to Date only (for DatePicker binding).
    /// </summary>
    private class DateTimeToDateConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                return dt.Date;
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                // Preserve the time part if the source was already DateTime
                return dt;
            }
            return null;
        }
    }

    /// <summary>
    /// Converter for DateTime to Time string (for TextBox binding).
    /// </summary>
    private class DateTimeToTimeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                return dt.ToString("HH:mm:ss", culture);
            }
            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string timeStr && DateTime.TryParseExact(timeStr, "HH:mm:ss", culture, System.Globalization.DateTimeStyles.None, out var time))
            {
                return time;
            }
            return null;
        }
    }
}
