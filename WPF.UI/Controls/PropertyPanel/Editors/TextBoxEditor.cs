// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor for text box properties.
/// </summary>
internal class TextBoxEditor : PropertyEditorBase
{
    public override PropertyEditorKind EditorKind => PropertyEditorKind.TextBox;

    public override FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
    {
        var textBox = new TextBox
        {
            MinWidth = 120,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        BindTextProperty(textBox, item);
        SetCommonProperties(textBox, item);

        return textBox;
    }
}
