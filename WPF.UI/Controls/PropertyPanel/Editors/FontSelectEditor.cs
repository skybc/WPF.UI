// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor for font picker properties.
/// </summary>
internal class FontSelectEditor : PropertyEditorBase
{
    public override PropertyEditorKind EditorKind => PropertyEditorKind.FontPicker;

    public override FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
    {
        var fontPicker = new FontPickerControl
        {
            MinWidth = 200,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            PreviewText = "Aa",
        };

        var binding = CreateValueBinding(item);
        fontPicker.SetBinding(FontPickerControl.FontSettingsProperty, binding);
        SetCommonProperties(fontPicker, item);

        return fontPicker;
    }

    /// <summary>
    /// Determines if this editor can handle the given property item.
    /// Handles FontSettings type or explicit FontPicker kind.
    /// </summary>
    public override bool CanHandle(PropertyItem item)
    {
        // Check explicit editor kind
        if (item.Attribute.Editor == PropertyEditorKind.FontPicker)
        {
            return true;
        }

        // Check if property type is FontSettings
        return item.PropertyType == typeof(FontSettings) || 
               (item.PropertyType.IsGenericType && 
                item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                item.PropertyType.GetGenericArguments()[0] == typeof(FontSettings));
    }
}
