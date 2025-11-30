using System;

namespace Wpf.Ui.Controls;

/// <summary>
/// Marks a property to be displayed/edited in PropertyPanel.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class PropertyPanelAttribute : Attribute
{
    /// <summary>
    /// Binding path for visibility. If not null/empty, binds to this path and converts to Visibility.
    /// Example: "IsAdvancedMode" will bind to the source object's IsAdvancedMode property.
    /// </summary>
    public string? IsVisible { get; set; }

    /// <summary>
    /// Optional display name shown in the panel. If null, the property name is used.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Optional description (tooltip) for the property editor.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Optional group name for future grouping scenarios.
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// Order for sorting. Lower comes first.
    /// </summary>
    public int Order { get; set; } = 0;

    /// <summary>
    /// Preferred editor kind. Defaults to <see cref="PropertyEditorKind.Auto"/>.
    /// </summary>
    public PropertyEditorKind Editor { get; set; } = PropertyEditorKind.Auto;

    /// <summary>
    /// If provided, the <see cref="System.Windows.DataTemplate"/> with this key will be used for rendering the editor.
    /// </summary>
    public object? TemplateResourceKey { get; set; }

    /// <summary>
    /// Use a custom element type as editor. It must be a <see cref="System.Windows.FrameworkElement"/>.
    /// When provided, the panel will generate a template hosting this control and bind <see cref="EditorValuePath"/> to the property.
    /// </summary>
    public Type? EditorElementType { get; set; }

    /// <summary>
    /// The path on the editor element to bind the property value to (for <see cref="EditorElementType"/>). Defaults to "Value".
    /// Typical paths: "Text" (TextBox), "SelectedItem" (ComboBox), "IsChecked" (CheckBox), "Value" (Slider).
    /// </summary>
    public string EditorValuePath { get; set; } = "Value";

    /// <summary>
    /// Slider/number range: Minimum. Only applies when editor is numeric or Slider.
    /// </summary>
    public double Min { get; set; } = 0d;

    /// <summary>
    /// Slider/number range: Maximum. Only applies when editor is numeric or Slider.
    /// </summary>
    public double Max { get; set; } = 100d;

    /// <summary>
    /// Slider step (TickFrequency). For integer properties this will snap to ticks.
    /// </summary>
    public double Step { get; set; } = 1d;

    /// <summary>
    /// When editor is ComboBox and property is not an enum,
    /// ItemsSource can be provided via resource key or binding path. Use this for custom scenarios.
    /// </summary>
    /// <remarks>
    /// Deprecated: Use <see cref="PropertyComboBoxAttribute"/> instead for ComboBox-specific configuration.
    /// </remarks>
    public object? ItemsSourceResourceKey { get; set; }

     

    /// <summary>
    /// Width for the DisplayName label. When 0, uses the PropertyPanel's DisplayNameWidth. Defaults to 0.
    /// </summary>
    public double DisplayNameWidth { get; set; } = 0d;

    /// <summary>
    /// Optional IValueConverter type for the binding. The type must implement <see cref="System.Windows.Data.IValueConverter"/>.
    /// This is used when binding a custom editor element to the property value.
    /// Only applies when <see cref="EditorElementType"/> is specified.
    /// </summary>
    public Type? Converter { get; set; }
}
