// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor for password box properties.
/// </summary>
internal class PasswordEditor : PropertyEditorBase
{
    public override PropertyEditorKind EditorKind => PropertyEditorKind.Password;

    public override FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
    {
        var passwordBox = new PasswordBox
        {
            MinWidth = 120,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var binding = CreateValueBinding(item);
        passwordBox.SetBinding(PasswordBox.PasswordProperty, binding);
        SetCommonProperties(passwordBox, item);

        return passwordBox;
    }
}
