// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor for file selection properties.
/// </summary>
internal class FileSelectEditor : PropertyEditorBase
{
    public override PropertyEditorKind EditorKind => PropertyEditorKind.FileSelect;

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
            var dialog = new OpenFileDialog();

            var fileAttr = item.FileAttribute;
            if (fileAttr != null && !string.IsNullOrWhiteSpace(fileAttr.Extension))
            {
                var extensions = fileAttr.Extension.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (extensions.Length > 0)
                {
                    var filterParts = new List<string>();
                    foreach (var ext in extensions)
                    {
                        var trimmedExt = ext.TrimStart('.');
                        filterParts.Add($"{trimmedExt} files (*.{trimmedExt})|*.{trimmedExt}");
                    }
                    dialog.Filter = string.Join("|", filterParts) + "|All files (*.*)|*.*";
                }
            }
            else
            {
                dialog.Filter = "All files (*.*)|*.*";
            }

            if (dialog.ShowDialog() == true)
            {
                item.CurrentValue = dialog.FileName;
            }
        };

        stackPanel.Children.Add(textBox);
        stackPanel.Children.Add(button);

        SetCommonProperties(stackPanel, item);

        return stackPanel;
    }
}
