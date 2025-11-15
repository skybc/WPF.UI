// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls;

/// <summary>
/// Interface for property editors that create UI controls for editing property values.
/// </summary>
public interface IPropertyEditor
{
    /// <summary>
    /// Creates an editor control for the given property item.
    /// </summary>
    /// <param name="item">The property item to create an editor for.</param>
    /// <param name="panel">The parent PropertyPanel for resource lookup.</param>
    /// <returns>A FrameworkElement that can edit the property value.</returns>
    FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel);

    /// <summary>
    /// Gets the editor kind this editor handles.
    /// </summary>
    PropertyEditorKind EditorKind { get; }

    /// <summary>
    /// Determines if this editor can handle the given property item.
    /// </summary>
    /// <param name="item">The property item to check.</param>
    /// <returns>True if this editor can handle the property; otherwise false.</returns>
    bool CanHandle(PropertyItem item);
}
