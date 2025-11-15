// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Data;

namespace Wpf.Ui.Controls;

/// <summary>
/// Factory that creates editor controls for PropertyItem instances.
/// Uses a registry of specialized editor implementations.
/// </summary>
internal static class PropertyEditorFactory
{
    /// <summary>
    /// Initializes the factory with default editors.
    /// Should be called once at application startup.
    /// </summary>
    public static void Initialize()
    {
        PropertyEditorRegistry.Initialize();
    }

    /// <summary>
    /// Registers a custom property editor.
    /// </summary>
    /// <param name="editor">The custom editor to register.</param>
    public static void RegisterEditor(IPropertyEditor editor)
    {
        PropertyEditorRegistry.RegisterEditor(editor);
    }

    /// <summary>
    /// Creates an appropriate editor control for the given PropertyItem.
    /// </summary>
    /// <param name="item">The property item to create an editor for.</param>
    /// <param name="panel">The parent PropertyPanel for resource lookup.</param>
    /// <returns>A FrameworkElement that can edit the property value.</returns>
    public static FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        // Ensure registry is initialized
        Initialize();

        var attr = item.Attribute;

        // Custom element type requested
        if (attr.Editor == PropertyEditorKind.Custom || attr.EditorElementType != null)
        {
            var elementType = attr.EditorElementType;
            if (elementType != null && typeof(FrameworkElement).IsAssignableFrom(elementType))
            {
                try
                {
                    var element = Activator.CreateInstance(elementType) as FrameworkElement;
                    if (element != null)
                    {
                        BindEditorProperty(element, attr.EditorValuePath, item);
                        return element;
                    }
                }
                catch
                {
                    // Fall through to registry
                }
            }
        }

        // Get editor from registry
        var editor = PropertyEditorRegistry.GetEditor(item);
        return editor.CreateEditor(item, panel);
    }

    /// <summary>
    /// Binds a custom editor property to the item's current value.
    /// </summary>
    private static void BindEditorProperty(FrameworkElement element, string propertyPath, PropertyItem item)
    {
        var field = element.GetType().GetField($"{propertyPath}Property",
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy);

        if (field != null && field.GetValue(null) is DependencyProperty dp)
        {
            var binding = new Binding(nameof(PropertyItem.CurrentValue))
            {
                Source = item,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };

            // Add converter if one is specified
            var converter = item.Converter;
            if (converter != null)
            {
                binding.Converter = converter;
            }

            element.SetBinding(dp, binding);
        }
    }
}

