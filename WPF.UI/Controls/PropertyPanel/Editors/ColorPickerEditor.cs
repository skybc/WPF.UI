// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor for color picker properties.
/// </summary>
internal class ColorPickerEditor : PropertyEditorBase
{
    public override PropertyEditorKind EditorKind => PropertyEditorKind.ColorPicker;

    public override FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
    {
        var colorPicker = new ColorPicker
        {
            MinWidth = 120,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var binding = CreateValueBinding(item);
        colorPicker.SetBinding(ColorPicker.ValueProperty, binding);
        SetCommonProperties(colorPicker, item);

        return colorPicker;
    }
}
