// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Wpf.Ui.Controls;

/// <summary>
/// Base class for property editors providing common functionality.
/// </summary>
public abstract class PropertyEditorBase : IPropertyEditor
{
    /// <summary>
    /// Gets the editor kind this editor handles.
    /// </summary>
    public abstract PropertyEditorKind EditorKind { get; }

    /// <summary>
    /// Creates an editor control for the given property item.
    /// </summary>
    public abstract FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel);

    /// <summary>
    /// Determines if this editor can handle the given property item.
    /// Default implementation returns true if the editor kind matches.
    /// </summary>
    public virtual bool CanHandle(PropertyItem item) => item.Attribute.Editor == EditorKind;

    /// <summary>
    /// Sets common properties on the editor element.
    /// </summary>
    protected virtual void SetCommonProperties(FrameworkElement element, PropertyItem item)
    {
        element.IsEnabled = !item.IsReadOnly;

        if (!string.IsNullOrEmpty(item.Description))
        {
            element.ToolTip = item.Description;
        }
    }

    /// <summary>
    /// Creates a binding for a dependency property.
    /// </summary>
    protected virtual Binding CreateValueBinding(PropertyItem item, UpdateSourceTrigger trigger = UpdateSourceTrigger.PropertyChanged)
    {
        return new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = trigger,
        };
    }

    /// <summary>
    /// Binds a text property to the property item's current value.
    /// </summary>
    protected virtual void BindTextProperty(TextBox textBox, PropertyItem item)
    {
        var binding = CreateValueBinding(item);
        textBox.SetBinding(TextBox.TextProperty, binding);
    }

    /// <summary>
    /// Finds the parent element of the specified type in the visual tree.
    /// </summary>
    protected T FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        DependencyObject parent = System.Windows.Media.VisualTreeHelper.GetParent(child);

        while (parent != null)
        {
            if (parent is T typedParent)
            {
                return typedParent;
            }

            parent = System.Windows.Media.VisualTreeHelper.GetParent(parent);
        }

        return null;
    }
}
