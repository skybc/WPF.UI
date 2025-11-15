// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor for date picker properties.
/// </summary>
internal class DatePickerEditor : PropertyEditorBase
{
    public override PropertyEditorKind EditorKind => PropertyEditorKind.DatePicker;

    public override FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
    {
        var datePicker = new DatePicker
        {
            MinWidth = 120,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var binding = CreateValueBinding(item);
        datePicker.SetBinding(DatePicker.SelectedDateProperty, binding);
        SetCommonProperties(datePicker, item);

        return datePicker;
    }
}
