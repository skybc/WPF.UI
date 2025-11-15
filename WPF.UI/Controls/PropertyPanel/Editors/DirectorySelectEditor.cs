// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor for directory selection properties.
/// </summary>
internal class DirectorySelectEditor : PropertyEditorBase
{
    public override PropertyEditorKind EditorKind => PropertyEditorKind.DirectorySelect;

    public override FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
    {
        var stackPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var textBox = new TextBox
        {
            MinWidth = 100,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Margin = new Thickness(0, 0, 8, 0),
        };

        BindTextProperty(textBox, item);

        var button = new Button
        {
            Content = "浏览...",
            MinWidth = 80,
            Height = 32,
        };

        button.Click += (s, e) =>
        {
            var dialog = new OpenFolderDialog
            {
                Title = "选择目录",
            };

            if (dialog.ShowDialog() == true)
            {
                item.CurrentValue = dialog.FolderName;
            }
        };

        stackPanel.Children.Add(textBox);
        stackPanel.Children.Add(button);

        SetCommonProperties(stackPanel, item);

        return stackPanel;
    }
}
