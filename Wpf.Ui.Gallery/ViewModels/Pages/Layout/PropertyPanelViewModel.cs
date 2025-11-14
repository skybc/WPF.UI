// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Layout;

public partial class PropertyPanelViewModel : ViewModel
{
    public SampleObject SampleData { get; } = new SampleObject();
}

/// <summary>
/// Sample object to demonstrate PropertyPanel functionality
/// </summary>
public class SampleObject : INotifyPropertyChanged
{
    private string _name = "Sample Item";
    private string _description = "This is a sample description";
    private bool _isEnabled = true;
    private int _count = 10;
    private double _percentage = 75.5;
    private SampleEnum _category = SampleEnum.Medium;
    private DateTime _createdDate = DateTime.Now;
    private string _password = "secret123";
    private double _sliderValue = 50;

    [PropertyPanel(DisplayName = "Item Name", Description = "The name of the item", Order = 1)]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    [PropertyPanel(DisplayName = "Description", Description = "A detailed description of the item", Order = 2, Editor = PropertyEditorKind.TextBox)]
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    [PropertyPanel(DisplayName = "Is Enabled", Description = "Whether the item is enabled or not", Order = 3)]
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            OnPropertyChanged();
        }
    }

    [PropertyPanel(DisplayName = "Count", Description = "Number of items", Order = 4)]
    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            OnPropertyChanged();
        }
    }

    [PropertyPanel(DisplayName = "Percentage", Description = "Percentage value", Order = 5)]
    public double Percentage
    {
        get => _percentage;
        set
        {
            _percentage = value;
            OnPropertyChanged();
        }
    }

    [PropertyPanel(DisplayName = "Category", Description = "Item category", Order = 6)]
    public SampleEnum Category
    {
        get => _category;
        set
        {
            _category = value;
            OnPropertyChanged();
        }
    }

    [PropertyPanel(DisplayName = "Created Date", Description = "When the item was created", Order = 7)]
    public DateTime CreatedDate
    {
        get => _createdDate;
        set
        {
            _createdDate = value;
            OnPropertyChanged();
        }
    }

    [PropertyPanel(DisplayName = "Password", Description = "A password field example", Order = 8, EditorElementType = typeof(PasswordBox), EditorValuePath = "Password")]
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    [PropertyPanel(DisplayName = "Slider Value", Description = "Demonstrates slider editor", Order = 9, Editor = PropertyEditorKind.Slider, Min = 0, Max = 100, Step = 5)]
    public double SliderValue
    {
        get => _sliderValue;
        set
        {
            _sliderValue = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public enum SampleEnum
{
    Low,
    Medium,
    High,
    Critical
}