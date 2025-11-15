// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

/// <summary>
/// A dialog window for selecting and configuring font settings.
/// </summary>
public partial class FontSettingsDialog : Window, INotifyPropertyChanged
{
    private FontSettings _fontSettings = new();
    private ObservableCollection<FontFamily> _availableFontFamilies = new();
    private ObservableCollection<FontWeight> _availableFontWeights = new();

    /// <summary>
    /// Event raised when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets or sets the font settings being edited.
    /// </summary>
    public FontSettings FontSettings
    {
        get => _fontSettings;
        set
        {
            if (_fontSettings != value)
            {
                _fontSettings = value ?? new FontSettings();
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the collection of available font families.
    /// </summary>
    public ObservableCollection<FontFamily> AvailableFontFamilies
    {
        get => _availableFontFamilies;
        set
        {
            if (_availableFontFamilies != value)
            {
                _availableFontFamilies = value ?? new ObservableCollection<FontFamily>();
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the collection of available font weights.
    /// </summary>
    public ObservableCollection<FontWeight> AvailableFontWeights
    {
        get => _availableFontWeights;
        set
        {
            if (_availableFontWeights != value)
            {
                _availableFontWeights = value ?? new ObservableCollection<FontWeight>();
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current style is Normal.
    /// </summary>
    public bool IsNormalStyle
    {
        get => FontSettings.FontStyle == FontStyles.Normal;
        set
        {
            if (value && FontSettings.FontStyle != FontStyles.Normal)
            {
                FontSettings.FontStyle = FontStyles.Normal;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current style is Italic.
    /// </summary>
    public bool IsItalicStyle
    {
        get => FontSettings.FontStyle == FontStyles.Italic;
        set
        {
            if (value && FontSettings.FontStyle != FontStyles.Italic)
            {
                FontSettings.FontStyle = FontStyles.Italic;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontSettingsDialog"/> class.
    /// </summary>
    public FontSettingsDialog()
    {
        InitializeComponent();
        DataContext = this;

        // Initialize available font families
        var fontFamilies = Fonts.SystemFontFamilies
            .OrderBy(f => f.Source)
            .ToList();
        foreach (var fontFamily in fontFamilies)
        {
            AvailableFontFamilies.Add(fontFamily);
        }

        // Initialize available font weights
        var fontWeights = new[]
        {
            FontWeights.Thin,
            FontWeights.ExtraLight,
            FontWeights.Light,
            FontWeights.Normal,
            FontWeights.Medium,
            FontWeights.SemiBold,
            FontWeights.Bold,
            FontWeights.ExtraBold,
            FontWeights.Black
        };
        foreach (var fontWeight in fontWeights)
        {
            AvailableFontWeights.Add(fontWeight);
        }
    }

    /// <summary>
    /// Called when the OK button is clicked.
    /// </summary>
    private void OnOkClick(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    /// <summary>
    /// Called when the Cancel button is clicked.
    /// </summary>
    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
