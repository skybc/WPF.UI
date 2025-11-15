// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

/// <summary>
/// Represents a control that lets a user pick a color using a color spectrum, color wheel, and preset colors.
/// </summary>
[TemplatePart(Name = PartToggleButton, Type = typeof(ToggleButton))]
[TemplatePart(Name = PartPopup, Type = typeof(Popup))]
[TemplatePart(Name = PartColorBorder, Type = typeof(Border))]
public class ColorPicker : Control
{
    private const string PartToggleButton = "PART_ToggleButton";
    private const string PartPopup = "PART_Popup";
    private const string PartColorBorder = "PART_ColorBorder";

    private ToggleButton? toggleButton;
    private Popup? popup;
    private Border? colorBorder;
    private bool isUpdatingColor;

    /// <summary>Identifies the <see cref="Value"/> dependency property.</summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(Color),
        typeof(ColorPicker),
        new FrameworkPropertyMetadata(
            Colors.White,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnValueChanged));

    /// <summary>Identifies the <see cref="IsDropDownOpen"/> dependency property.</summary>
    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
        nameof(IsDropDownOpen),
        typeof(bool),
        typeof(ColorPicker),
        new PropertyMetadata(false, OnIsDropDownOpenChanged));

    /// <summary>Identifies the <see cref="PresetColors"/> dependency property.</summary>
    public static readonly DependencyProperty PresetColorsProperty = DependencyProperty.Register(
        nameof(PresetColors),
        typeof(ObservableCollection<Color>),
        typeof(ColorPicker),
        new PropertyMetadata(null));

    /// <summary>Identifies the <see cref="Hue"/> dependency property.</summary>
    public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
        nameof(Hue),
        typeof(double),
        typeof(ColorPicker),
        new PropertyMetadata(0.0, OnHsvChanged));

    /// <summary>Identifies the <see cref="Saturation"/> dependency property.</summary>
    public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
        nameof(Saturation),
        typeof(double),
        typeof(ColorPicker),
        new PropertyMetadata(0.0, OnHsvChanged));

    /// <summary>Identifies the <see cref="Brightness"/> dependency property.</summary>
    public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register(
        nameof(Brightness),
        typeof(double),
        typeof(ColorPicker),
        new PropertyMetadata(1.0, OnHsvChanged));

    /// <summary>Identifies the <see cref="Red"/> dependency property.</summary>
    public static readonly DependencyProperty RedProperty = DependencyProperty.Register(
        nameof(Red),
        typeof(byte),
        typeof(ColorPicker),
        new PropertyMetadata((byte)255, OnRgbChanged));

    /// <summary>Identifies the <see cref="Green"/> dependency property.</summary>
    public static readonly DependencyProperty GreenProperty = DependencyProperty.Register(
        nameof(Green),
        typeof(byte),
        typeof(ColorPicker),
        new PropertyMetadata((byte)255, OnRgbChanged));

    /// <summary>Identifies the <see cref="Blue"/> dependency property.</summary>
    public static readonly DependencyProperty BlueProperty = DependencyProperty.Register(
        nameof(Blue),
        typeof(byte),
        typeof(ColorPicker),
        new PropertyMetadata((byte)255, OnRgbChanged));

    /// <summary>Identifies the <see cref="Alpha"/> dependency property.</summary>
    public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register(
        nameof(Alpha),
        typeof(byte),
        typeof(ColorPicker),
        new PropertyMetadata((byte)255, OnRgbChanged));

    /// <summary>
    /// Gets or sets the currently selected color value.
    /// </summary>
    [Bindable(true)]
    [Category("Common")]
    public Color Value
    {
        get => (Color)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the drop-down is open.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public bool IsDropDownOpen
    {
        get => (bool)GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets the collection of preset colors.
    /// </summary>
    public ObservableCollection<Color> PresetColors
    {
        get => (ObservableCollection<Color>)GetValue(PresetColorsProperty);
        set => SetValue(PresetColorsProperty, value);
    }

    /// <summary>
    /// Gets or sets the hue value (0-360).
    /// </summary>
    public double Hue
    {
        get => (double)GetValue(HueProperty);
        set => SetValue(HueProperty, value);
    }

    /// <summary>
    /// Gets or sets the saturation value (0-1).
    /// </summary>
    public double Saturation
    {
        get => (double)GetValue(SaturationProperty);
        set => SetValue(SaturationProperty, value);
    }

    /// <summary>
    /// Gets or sets the brightness/value (0-1).
    /// </summary>
    public double Brightness
    {
        get => (double)GetValue(BrightnessProperty);
        set => SetValue(BrightnessProperty, value);
    }

    /// <summary>
    /// Gets or sets the red component (0-255).
    /// </summary>
    public byte Red
    {
        get => (byte)GetValue(RedProperty);
        set => SetValue(RedProperty, value);
    }

    /// <summary>
    /// Gets or sets the green component (0-255).
    /// </summary>
    public byte Green
    {
        get => (byte)GetValue(GreenProperty);
        set => SetValue(GreenProperty, value);
    }

    /// <summary>
    /// Gets or sets the blue component (0-255).
    /// </summary>
    public byte Blue
    {
        get => (byte)GetValue(BlueProperty);
        set => SetValue(BlueProperty, value);
    }

    /// <summary>
    /// Gets or sets the alpha component (0-255).
    /// </summary>
    public byte Alpha
    {
        get => (byte)GetValue(AlphaProperty);
        set => SetValue(AlphaProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorPicker"/> class.
    /// </summary>
    public ColorPicker()
    {
        PresetColors = CreatePresetColors();
        Loaded += OnLoaded;
        AddHandler(Button.ClickEvent, new RoutedEventHandler(OnPresetColorButtonClick));
    }

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (toggleButton != null)
        {
            toggleButton.Click -= OnToggleButtonClick;
        }

        if (colorBorder != null)
        {
            colorBorder.MouseLeftButtonDown -= OnColorBorderMouseLeftButtonDown;
        }

        toggleButton = GetTemplateChild(PartToggleButton) as ToggleButton;
        popup = GetTemplateChild(PartPopup) as Popup;
        colorBorder = GetTemplateChild(PartColorBorder) as Border;

        if (toggleButton != null)
        {
            toggleButton.Click += OnToggleButtonClick;
        }

        if (colorBorder != null)
        {
            colorBorder.MouseLeftButtonDown += OnColorBorderMouseLeftButtonDown;
        }

        if (popup != null)
        {
            popup.Opened += OnPopupOpened;
            popup.Closed += OnPopupClosed;
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        UpdateRgbFromColor(Value);
        UpdateHsvFromColor(Value);
    }

    private void OnToggleButtonClick(object sender, RoutedEventArgs e)
    {
        SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);
    }

    private void OnColorBorderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);
        e.Handled = true;
    }

    private void OnPopupOpened(object? sender, System.EventArgs e)
    {
        // Popup opened logic
    }

    private void OnPopupClosed(object? sender, System.EventArgs e)
    {
        if (toggleButton != null)
        {
            toggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, false);
        }
    }

    private void OnPresetColorButtonClick(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is System.Windows.Controls.Button button && button.Tag is Color color)
        {
            SetCurrentValue(ValueProperty, color);
            SetCurrentValue(IsDropDownOpenProperty, false);
        }
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorPicker picker && !picker.isUpdatingColor)
        {
            picker.isUpdatingColor = true;
            picker.UpdateRgbFromColor((Color)e.NewValue);
            picker.UpdateHsvFromColor((Color)e.NewValue);
            picker.isUpdatingColor = false;
        }
    }

    private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorPicker picker && picker.popup != null)
        {
            picker.popup.SetCurrentValue(Popup.IsOpenProperty, (bool)e.NewValue);
        }
    }

    private static void OnHsvChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorPicker picker && !picker.isUpdatingColor)
        {
            picker.isUpdatingColor = true;
            picker.UpdateColorFromHsv();
            picker.isUpdatingColor = false;
        }
    }

    private static void OnRgbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorPicker picker && !picker.isUpdatingColor)
        {
            picker.isUpdatingColor = true;
            picker.UpdateColorFromRgb();
            picker.isUpdatingColor = false;
        }
    }

    private void UpdateColorFromHsv()
    {
        var color = ColorHelper.HsvToRgb(Hue, Saturation, Brightness, Alpha / 255.0);
        SetCurrentValue(ValueProperty, color);
        UpdateRgbFromColor(color);
    }

    private void UpdateColorFromRgb()
    {
        var color = Color.FromArgb(Alpha, Red, Green, Blue);
        SetCurrentValue(ValueProperty, color);
        UpdateHsvFromColor(color);
    }

    private void UpdateRgbFromColor(Color color)
    {
        SetCurrentValue(RedProperty, color.R);
        SetCurrentValue(GreenProperty, color.G);
        SetCurrentValue(BlueProperty, color.B);
        SetCurrentValue(AlphaProperty, color.A);
    }

    private void UpdateHsvFromColor(Color color)
    {
        var hsv = ColorHelper.RgbToHsv(color);
        SetCurrentValue(HueProperty, hsv.Hue);
        SetCurrentValue(SaturationProperty, hsv.Saturation);
        SetCurrentValue(BrightnessProperty, hsv.Value);
    }

    private ObservableCollection<Color> CreatePresetColors()
    {
        // Using colors from Palette.xaml
        return new ObservableCollection<Color>
        {
            Color.FromRgb(0xF4, 0x43, 0x36), // PaletteRedColor
            Color.FromRgb(0xE9, 0x1E, 0x63), // PalettePinkColor
            Color.FromRgb(0x9C, 0x27, 0xB0), // PalettePurpleColor
            Color.FromRgb(0x67, 0x3A, 0xB7), // PaletteDeepPurpleColor
            Color.FromRgb(0x3F, 0x51, 0xB5), // PaletteIndigoColor
            Color.FromRgb(0x21, 0x96, 0xF3), // PaletteBlueColor
            Color.FromRgb(0x03, 0xA9, 0xF4), // PaletteLightBlueColor
            Color.FromRgb(0x00, 0xBC, 0xD4), // PaletteCyanColor
            Color.FromRgb(0x00, 0x96, 0x88), // PaletteTealColor
            Color.FromRgb(0x4C, 0xAF, 0x50), // PaletteGreenColor
            Color.FromRgb(0x8B, 0xC3, 0x4A), // PaletteLightGreenColor
            Color.FromRgb(0xCD, 0xDC, 0x39), // PaletteLimeColor
            Color.FromRgb(0xFF, 0xEB, 0x3B), // PaletteYellowColor
            Color.FromRgb(0xFF, 0xC1, 0x07), // PaletteAmberColor
            Color.FromRgb(0xFF, 0x98, 0x00), // PaletteOrangeColor
            Color.FromRgb(0xFF, 0x57, 0x22), // PaletteDeepOrangeColor
            Color.FromRgb(0x79, 0x55, 0x48), // PaletteBrownColor
            Color.FromRgb(0x9E, 0x9E, 0x9E), // PaletteGreyColor
        };
    }
}
