// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor for enum and combo box properties.
/// </summary>
internal class ComboBoxEditor : PropertyEditorBase
{
    public override PropertyEditorKind EditorKind => PropertyEditorKind.ComboBox;

    public override bool CanHandle(PropertyItem item) =>
        item.Attribute.Editor == PropertyEditorKind.ComboBox || item.IsEnum;

    public override FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
    {
        var comboBox = new ComboBox
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            MinWidth = 120,
        };

        // For enum types
        var comboAttr = item.ComboBoxAttribute;
        if (item.IsEnum)
        {
            comboBox.ItemsSource = item.EnumValues;
        }
        else
        {
            // For non-enum combo boxes, try to get ComboBoxAttribute first
            if (comboAttr != null)
            {
                if (!string.IsNullOrWhiteSpace(comboAttr.ItemsSourcePath))
                {
                    var itemsBinding = new Binding(comboAttr.ItemsSourcePath);
                    comboBox.SetBinding(ItemsControl.ItemsSourceProperty, itemsBinding);
                }

                if (!string.IsNullOrWhiteSpace(comboAttr.DisplayMemberPath))
                {
                    comboBox.DisplayMemberPath = comboAttr.DisplayMemberPath;
                }

                if (!string.IsNullOrWhiteSpace(comboAttr.SelectedValuePath))
                {
                    comboBox.SelectedValuePath = comboAttr.SelectedValuePath;
                }
            }
             
        }

        var binding = CreateValueBinding(item);

        if (!string.IsNullOrWhiteSpace(item.ComboBoxAttribute?.SelectedValuePath))
        {
            comboBox.SetBinding(ComboBox.SelectedValueProperty, binding);
        }
        else
        {
            comboBox.SetBinding(ComboBox.SelectedItemProperty, binding);
        }
        SetCommonProperties(comboBox, item);

        return comboBox;
    }
}
