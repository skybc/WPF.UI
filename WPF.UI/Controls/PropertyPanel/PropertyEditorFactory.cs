// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Wpf.Ui.Controls;

/// <summary>
/// Factory that creates editor controls for PropertyItem instances.
/// Replaces DataTemplate-based approach with direct control instantiation.
/// </summary>
internal static class PropertyEditorFactory
{
    /// <summary>
    /// Creates an appropriate editor control for the given PropertyItem.
    /// </summary>
    /// <param name="item">The property item to create an editor for.</param>
    /// <param name="panel">The parent PropertyPanel for resource lookup.</param>
    /// <returns>A FrameworkElement that can edit the property value.</returns>
    public static FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
    {
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
                        SetCommonProperties(element, item);
                        return element;
                    }
                }
                catch
                {
                    // Fall through to default
                }
            }
        }

        // Explicit editor kind
        switch (attr.Editor)
        {
            case PropertyEditorKind.TextBox:
                return CreateTextBoxEditor(item);

            case PropertyEditorKind.Password:
                return CreatePasswordEditor(item);

            case PropertyEditorKind.CheckBox:
            case PropertyEditorKind.ToggleSwitch:
                return CreateCheckBoxEditor(item);

            case PropertyEditorKind.ComboBox:
                if (!item.IsEnum)
                {
                    return CreateComboBoxEditor(item, attr);
                }

                return CreateEnumComboBoxEditor(item);

            case PropertyEditorKind.Slider:
                return CreateSliderEditor(item);

            case PropertyEditorKind.DatePicker:
                return CreateDatePickerEditor(item);

            case PropertyEditorKind.ColorPicker:
                return CreateColorPickerEditor(item);
        }

        // Auto by type
        var t = item.UnderlyingType;
        if (t == typeof(string))
        {
            return CreateTextBoxEditor(item);
        }

        if (t == typeof(bool))
        {
            return CreateCheckBoxEditor(item);
        }

        if (t.IsEnum)
        {
            return CreateEnumComboBoxEditor(item);
        }

        if (t == typeof(DateTime))
        {
            return CreateDatePickerEditor(item);
        }

        if (t == typeof(Color))
        {
            return CreateColorPickerEditor(item);
        }

        if (IsNumeric(t))
        {
            // prefer slider if attribute specifies range
            if (item.Min != 0 || item.Max != 100 || attr.Editor == PropertyEditorKind.Slider)
            {
                return CreateSliderEditor(item);
            }

            return CreateNumberTextBoxEditor(item);
        }

        // Fallback to Text
        return CreateTextBoxEditor(item);
    }

    private static bool IsNumeric(Type t)
    {
        t = Nullable.GetUnderlyingType(t) ?? t;
        return t == typeof(byte) || t == typeof(sbyte) ||
               t == typeof(short) || t == typeof(ushort) ||
               t == typeof(int) || t == typeof(uint) ||
               t == typeof(long) || t == typeof(ulong) ||
               t == typeof(float) || t == typeof(double) || t == typeof(decimal);
    }

    private static TextBox CreateTextBoxEditor(PropertyItem item)
    {
        var textBox = new TextBox
        {
            MinWidth = 120,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var binding = new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };

        textBox.SetBinding(TextBox.TextProperty, binding);
        SetCommonProperties(textBox, item);

        return textBox;
    }

    private static PasswordBox CreatePasswordEditor(PropertyItem item)
    {
        var passwordBox = new PasswordBox
        {
            MinWidth = 120,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var binding = new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };

        passwordBox.SetBinding(PasswordBox.PasswordProperty, binding);
        SetCommonProperties(passwordBox, item);

        return passwordBox;
    }

    private static CheckBox CreateCheckBoxEditor(PropertyItem item)
    {
        var checkBox = new CheckBox
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        var binding = new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };

        checkBox.SetBinding(ToggleButton.IsCheckedProperty, binding);
        SetCommonProperties(checkBox, item);

        return checkBox;
    }

    private static ComboBox CreateEnumComboBoxEditor(PropertyItem item)
    {
        var comboBox = new ComboBox
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            MinWidth = 120,
        };

        comboBox.ItemsSource = item.EnumValues;

        var binding = new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };

        comboBox.SetBinding(Selector.SelectedItemProperty, binding);
        SetCommonProperties(comboBox, item);

        return comboBox;
    }

    private static ComboBox CreateComboBoxEditor(PropertyItem item, PropertyPanelAttribute attr)
    {
        var comboBox = new ComboBox
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            MinWidth = 120,
        };

        // Set ItemsSource if provided
        if (!string.IsNullOrWhiteSpace(attr.ItemsSourcePath))
        {
            var itemsBinding = new Binding(attr.ItemsSourcePath);
            comboBox.SetBinding(ItemsControl.ItemsSourceProperty, itemsBinding);
        }

        // Set DisplayMemberPath if provided
        if (!string.IsNullOrWhiteSpace(attr.DisplayMemberPath))
        {
            comboBox.DisplayMemberPath = attr.DisplayMemberPath;
        }

        // Set SelectedValuePath if provided
        if (!string.IsNullOrWhiteSpace(attr.SelectedValuePath))
        {
            comboBox.SelectedValuePath = attr.SelectedValuePath;
        }

        var binding = new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };

        comboBox.SetBinding(Selector.SelectedItemProperty, binding);
        SetCommonProperties(comboBox, item);

        return comboBox;
    }

    private static FrameworkElement CreateSliderEditor(PropertyItem item)
    {
        var stackPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var slider = new Slider
        {
            MinWidth = 120,
            IsSnapToTickEnabled = true,
            Minimum = item.Min,
            Maximum = item.Max,
            TickFrequency = item.Step,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var sliderBinding = new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };

        slider.SetBinding(RangeBase.ValueProperty, sliderBinding);

        var textBlock = new TextBlock
        {
            Margin = new Thickness(8, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
        };

        var textBinding = new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
        };

        textBlock.SetBinding(TextBlock.TextProperty, textBinding);

        stackPanel.Children.Add(slider);
        stackPanel.Children.Add(textBlock);

        SetCommonProperties(stackPanel, item);

        return stackPanel;
    }

    private static TextBox CreateNumberTextBoxEditor(PropertyItem item)
    {
        var textBox = new TextBox
        {
            MinWidth = 120,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var binding = new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            StringFormat = "{0:g}",
        };

        binding.ValidationRules.Add(new ExceptionValidationRule());

        textBox.SetBinding(TextBox.TextProperty, binding);
        SetCommonProperties(textBox, item);

        return textBox;
    }

    private static DatePicker CreateDatePickerEditor(PropertyItem item)
    {
        var datePicker = new DatePicker
        {
            MinWidth = 120,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var binding = new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };

        datePicker.SetBinding(DatePicker.SelectedDateProperty, binding);
        SetCommonProperties(datePicker, item);

        return datePicker;
    }

    private static ColorPicker CreateColorPickerEditor(PropertyItem item)
    {
        var colorPicker = new ColorPicker
        {
            MinWidth = 120,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var binding = new Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };

        colorPicker.SetBinding(ColorPicker.ValueProperty, binding);
        SetCommonProperties(colorPicker, item);

        return colorPicker;
    }

    private static void SetCommonProperties(FrameworkElement element, PropertyItem item)
    {
        // Set IsEnabled based on whether the property is read-only
        element.IsEnabled = !item.IsReadOnly;

        if (!string.IsNullOrEmpty(item.Description))
        {
            element.ToolTip = item.Description;
        }
    }

    private static void BindEditorProperty(FrameworkElement element, string propertyPath, PropertyItem item)
    {
        // Find the dependency property by name
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

            element.SetBinding(dp, binding);
        }
    }
}
