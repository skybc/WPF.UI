using System;

namespace Wpf.Ui.Controls;

/// <summary>
/// Attribute for ComboBox editor configuration. Used with PropertyPanelAttribute
/// when Editor is set to ComboBox.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class PropertyComboBoxAttribute : Attribute
{
    /// <summary>
    /// DataContext-relative binding path used as ItemsSource for ComboBox.
    /// </summary>
    public string? ItemsSourcePath { get; set; }

    /// <summary>
    /// Optional DisplayMemberPath for ComboBox.
    /// Specifies which property of the items to display in the ComboBox.
    /// </summary>
    public string? DisplayMemberPath { get; set; }

    /// <summary>
    /// Optional SelectedValuePath for ComboBox.
    /// Specifies which property of the selected item to use as the value.
    /// </summary>
    public string? SelectedValuePath { get; set; }

    /// <summary>
    /// Initializes a new instance of the PropertyComboBoxAttribute class.
    /// </summary>
    public PropertyComboBoxAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the PropertyComboBoxAttribute class with ItemsSourcePath.
    /// </summary>
    /// <param name="itemsSourcePath">DataContext-relative binding path for ItemsSource.</param>
    public PropertyComboBoxAttribute(string? itemsSourcePath)
    {
        ItemsSourcePath = itemsSourcePath;
    }

    /// <summary>
    /// Initializes a new instance of the PropertyComboBoxAttribute class with all parameters.
    /// </summary>
    /// <param name="itemsSourcePath">DataContext-relative binding path for ItemsSource.</param>
    /// <param name="displayMemberPath">Property to display in the ComboBox.</param>
    /// <param name="selectedValuePath">Property to use as the selected value.</param>
    public PropertyComboBoxAttribute(string? itemsSourcePath, string? displayMemberPath, string? selectedValuePath)
    {
        ItemsSourcePath = itemsSourcePath;
        DisplayMemberPath = displayMemberPath;
        SelectedValuePath = selectedValuePath;
    }
}
