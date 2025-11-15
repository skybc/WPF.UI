// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Layout;

public partial class PropertyPanelViewModel : ViewModel
{
    public SampleObject SampleData { get; set; } = new SampleObject();
    public SampleObject SampleData2 { get; set; } = new SampleObject
    {
        Name = "Another Item",
        Description = "Different description",
        IsEnabled = false,
        Count = 25,
        Percentage = 33.3,
        Category = SampleEnum.High,
        SliderValue = 75,
        ThemeColor = System.Windows.Media.Colors.Green
    };
}

/// <summary>
/// Sample object to demonstrate PropertyPanel functionality
/// </summary>
public partial class SampleObject : ViewModel
{
    [PropertyPanel(DisplayName = "Item Name", Description = "The name of the item", Order = 1, GroupName = "Basic")]
    public string Name { get; set; } = "Sample Item";

    [PropertyPanel(DisplayName = "Description", Description = "A detailed description of the item", Order = 2, Editor = PropertyEditorKind.TextBox, GroupName = "Basic")]
    public string Description { get; set; } = "This is a sample description";

    [PropertyPanel(DisplayName = "Is Enabled", Description = "Whether the item is enabled or not", Order = 3, GroupName = "Settings")]
    public bool IsEnabled { get; set; } = true;

    [PropertyPanel(DisplayName = "Count", Description = "Number of items", Order = 4, GroupName = "Settings")]
    public int Count { get; set; } = 10;

    [PropertyPanel(DisplayName = "Percentage", Description = "Percentage value", Order = 5, GroupName = "Settings")]
    public double Percentage { get; set; } = 75.5;

    [PropertyPanel(DisplayName = "Category", Description = "Item category", Order = 6, GroupName = "Settings")]
    public SampleEnum Category { get; set; } = SampleEnum.Medium;

    [PropertyPanel(DisplayName = "Created Date", Description = "When the item was created", Order = 7, GroupName = "Timestamps")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [PropertyPanel(DisplayName = "Password", Description = "A password field example", Order = 8, Editor = PropertyEditorKind.Password, GroupName = "Security")]
    public string Password { get; set; } = "secret123";

    [PropertyPanel(DisplayName = "Slider Value", Description = "Demonstrates slider editor", Order = 9, Editor = PropertyEditorKind.Slider, Min = 0, Max = 100, Step = 5, GroupName = "Advanced")]
    public double SliderValue { get; set; } = 50;

    [PropertyPanel(DisplayName = "Theme Color", Description = "Choose a theme color", Order = 10, Editor = PropertyEditorKind.ColorPicker, GroupName = "Appearance")]
    public System.Windows.Media.Color ThemeColor { get; set; } = System.Windows.Media.Colors.Blue;
}

public enum SampleEnum
{
    Low,
    Medium,
    High,
    Critical
}