// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor for checkbox and toggle switch properties.
/// </summary>
internal class CheckBoxEditor : PropertyEditorBase
{
    public override PropertyEditorKind EditorKind => PropertyEditorKind.CheckBox;

    public override bool CanHandle(PropertyItem item) => 
        item.Attribute.Editor == PropertyEditorKind.CheckBox || item.Attribute.Editor == PropertyEditorKind.ToggleSwitch;

    public override FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
    {
        var checkBox = new CheckBox
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        var binding = CreateValueBinding(item);
        checkBox.SetBinding(ToggleButton.IsCheckedProperty, binding);
        SetCommonProperties(checkBox, item);

        return checkBox;
    }
}
