// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

/// <summary>
/// A reusable font picker control that displays a font preview with a button to open the font settings dialog.
/// </summary>
public class FontPickerControl : Control
{
    /// <summary>
    /// Identifies the <see cref="FontSettings"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FontSettingsProperty = DependencyProperty.Register(
        nameof(FontSettings),
        typeof(FontSettings),
        typeof(FontPickerControl),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault),
        ValidateFontSettings);

    /// <summary>
    /// Identifies the <see cref="PreviewText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PreviewTextProperty = DependencyProperty.Register(
        nameof(PreviewText),
        typeof(string),
        typeof(FontPickerControl),
        new PropertyMetadata("Font Preview Text"));

    /// <summary>
    /// Gets or sets the font settings.
    /// </summary>
    public FontSettings FontSettings
    {
        get => (FontSettings)GetValue(FontSettingsProperty) ?? new FontSettings();
        set => SetValue(FontSettingsProperty, value);
    }

    /// <summary>
    /// Gets or sets the preview text displayed in the TextBlock.
    /// </summary>
    public string PreviewText
    {
        get => (string)GetValue(PreviewTextProperty);
        set => SetValue(PreviewTextProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontPickerControl"/> class.
    /// </summary>
    static FontPickerControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FontPickerControl), new FrameworkPropertyMetadata(typeof(FontPickerControl)));
    }

    /// <summary>
    /// Validates the FontSettings value.
    /// </summary>
    private static bool ValidateFontSettings(object value)
    {
        return value == null || value is FontSettings;
    }

    /// <summary>
    /// Called when the template is applied to retrieve template parts.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        try
        {
            // Unwire previous button if it exists
            if (GetTemplateChild("OpenButton") is System.Windows.Controls.Button oldButton)
            {
                oldButton.Click -= OnOpenFontDialogClick;
            }

            // Find and wire up the button
            var button = GetTemplateChild("OpenButton");
            if (button == null)
            {
                Debug.WriteLine("WARNING: OpenButton template child not found in FontPickerControl");
                return;
            }

            if (button is System.Windows.Controls.Button btn)
            {
                btn.Click += OnOpenFontDialogClick;
            }
            else
            {
                Debug.WriteLine($"WARNING: OpenButton is of type {button.GetType().Name}, not Button");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ERROR in FontPickerControl.OnApplyTemplate: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles the button click event.
    /// </summary>
    private void OnOpenFontDialogClick(object sender, RoutedEventArgs e)
    {
        OnOpenFontDialog(sender, e);
    }

    /// <summary>
    /// Called when the open font dialog button is clicked.
    /// </summary>
    private void OnOpenFontDialog(object sender, RoutedEventArgs e)
    {
        var dialog = new FontSettingsDialog
        {
            Owner = Window.GetWindow(this),
            FontSettings = FontSettings.Clone()
        };

        if (dialog.ShowDialog() == true)
        {
            FontSettings = dialog.FontSettings;
        }
    }
}
