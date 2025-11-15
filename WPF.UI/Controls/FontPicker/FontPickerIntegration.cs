// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// A custom property editor for FontSettings that can be used with PropertyPanel.
/// This editor allows editing FontSettings directly within a PropertyPanel.
/// </summary>
//public class FontPickerPropertyEditor : PropertyEditorBase
//{
//    public override PropertyEditorKind EditorKind => PropertyEditorKind.FontPicker;

//    /// <inheritdoc/>
//    public override UIElement CreateEditor(PropertyItem property)
//    {
//        if (property.Value is not FontSettings fontSettings)
//        {
//            fontSettings = new FontSettings();
//        }

//        var control = new FontPickerControl
//        {
//            FontSettings = fontSettings,
//            PreviewText = property.DisplayName ?? "Font Preview",
//            HorizontalAlignment = HorizontalAlignment.Stretch,
//            Height = 36
//        };

//        // Bind the control back to the property
//        var binding = new System.Windows.Data.Binding("FontSettings")
//        {
//            Source = control,
//            Mode = System.Windows.Data.BindingMode.TwoWay,
//            UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged
//        };

//        BindingOperations.SetBinding(property, PropertyItem.ValueProperty, binding);

//        return control;
//    }

//    public override FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
//    {
//        throw new NotImplementedException();
//    }
////}

///// <summary>
///// Provides extension methods for integrating FontPickerControl with PropertyPanel.
///// </summary>
//public static class FontPickerIntegration
//{
//    /// <summary>
//    /// Registers the FontPickerControl editor with the property editor registry.
//    /// This should be called during application initialization.
//    /// </summary>
//    public static void RegisterFontPickerEditor()
//    {
//        PropertyEditorRegistry.RegisterEditor(
//            typeof(FontSettings),
//            () => new FontPickerPropertyEditor()
//        );
//    }

//    /// <summary>
//    /// Applies font settings to a TextBlock or other control with font properties.
//    /// </summary>
//    /// <param name="element">The element to apply font settings to.</param>
//    /// <param name="settings">The font settings to apply.</param>
//    public static void ApplyFontSettings(FrameworkElement element, FontSettings settings)
//    {
//        if (element is TextBlock textBlock)
//        {
//            textBlock.FontFamily = settings.FontFamily;
//            textBlock.FontSize = settings.FontSize;
//            textBlock.FontWeight = settings.FontWeight;
//            textBlock.FontStyle = settings.FontStyle;
//        }
//        else if (element is TextBox textBox)
//        {
//            textBox.FontFamily = settings.FontFamily;
//            textBox.FontSize = settings.FontSize;
//            textBox.FontWeight = settings.FontWeight;
//            textBox.FontStyle = settings.FontStyle;
//        }
//        else if (element is Control control)
//        {
//            control.FontFamily = settings.FontFamily;
//            control.FontSize = settings.FontSize;
//            control.FontWeight = settings.FontWeight;
//            control.FontStyle = settings.FontStyle;
//        }
//    }

//    /// <summary>
//    /// Extracts font settings from a TextBlock or other control.
//    /// </summary>
//    /// <param name="element">The element to extract font settings from.</param>
//    /// <returns>A new FontSettings instance with the element's font properties.</returns>
//    public static FontSettings ExtractFontSettings(FrameworkElement element)
//    {
//        var fontFamily = GetFontFamily(element) ?? new System.Windows.Media.FontFamily("Segoe UI");
//        var fontSize = GetFontSize(element) ?? 14.0;
//        var fontWeight = GetFontWeight(element) ?? System.Windows.FontWeights.Normal;
//        var fontStyle = GetFontStyle(element) ?? System.Windows.FontStyles.Normal;

//        return new FontSettings(fontFamily, fontSize, fontWeight, fontStyle);
//    }

//    private static System.Windows.Media.FontFamily? GetFontFamily(FrameworkElement element)
//    {
//        return element switch
//        {
//            TextBlock tb => tb.FontFamily,
//            TextBox tb => tb.FontFamily,
//            Control c => c.FontFamily,
//            _ => null
//        };
//    }

//    private static double? GetFontSize(FrameworkElement element)
//    {
//        return element switch
//        {
//            TextBlock tb => tb.FontSize,
//            TextBox tb => tb.FontSize,
//            Control c => c.FontSize,
//            _ => null
//        };
//    }

//    private static System.Windows.FontWeight? GetFontWeight(FrameworkElement element)
//    {
//        return element switch
//        {
//            TextBlock tb => tb.FontWeight,
//            TextBox tb => tb.FontWeight,
//            Control c => c.FontWeight,
//            _ => null
//        };
//    }

//    private static System.Windows.FontStyle? GetFontStyle(FrameworkElement element)
//    {
//        return element switch
//        {
//            TextBlock tb => tb.FontStyle,
//            TextBox tb => tb.FontStyle,
//            Control c => c.FontStyle,
//            _ => null
//        };
//    }
//}
