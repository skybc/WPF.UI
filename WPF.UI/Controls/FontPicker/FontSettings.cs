// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

using System.Windows;
using System.Windows.Media;

/// <summary>
/// Represents a collection of font-related settings that can be bound to font preview and selection controls.
/// </summary>
public class FontSettings : DependencyObject
{
    /// <summary>
    /// Identifies the <see cref="FontFamily"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(
        nameof(FontFamily),
        typeof(FontFamily),
        typeof(FontSettings),
        new FrameworkPropertyMetadata(new FontFamily("Segoe UI"), FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="FontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
        nameof(FontSize),
        typeof(double),
        typeof(FontSettings),
        new FrameworkPropertyMetadata(14.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="FontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register(
        nameof(FontWeight),
        typeof(FontWeight),
        typeof(FontSettings),
        new FrameworkPropertyMetadata(FontWeights.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="FontStyle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register(
        nameof(FontStyle),
        typeof(FontStyle),
        typeof(FontSettings),
        new FrameworkPropertyMetadata(FontStyles.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="Foreground"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
        nameof(Foreground),
        typeof(Brush),
        typeof(FontSettings),
        new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Gets or sets the font family.
    /// </summary>
    public FontFamily FontFamily
    {
        get => (FontFamily)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size in device-independent units (1/96th of an inch).
    /// </summary>
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the font weight.
    /// </summary>
    public FontWeight FontWeight
    {
        get => (FontWeight)GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the font style (Normal or Italic).
    /// </summary>
    public FontStyle FontStyle
    {
        get => (FontStyle)GetValue(FontStyleProperty);
        set => SetValue(FontStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the text foreground color.
    /// </summary>
    public Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="FontSettings"/> class.
    /// </summary>
    public FontSettings()
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="FontSettings"/> class with initial values.
    /// </summary>
    /// <param name="fontFamily">The initial font family.</param>
    /// <param name="fontSize">The initial font size.</param>
    /// <param name="fontWeight">The initial font weight.</param>
    /// <param name="fontStyle">The initial font style.</param>
    public FontSettings(FontFamily fontFamily, double fontSize, FontWeight fontWeight, FontStyle fontStyle)
    {
        FontFamily = fontFamily;
        FontSize = fontSize;
        FontWeight = fontWeight;
        FontStyle = fontStyle;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="FontSettings"/> class with initial values including foreground color.
    /// </summary>
    /// <param name="fontFamily">The initial font family.</param>
    /// <param name="fontSize">The initial font size.</param>
    /// <param name="fontWeight">The initial font weight.</param>
    /// <param name="fontStyle">The initial font style.</param>
    /// <param name="foreground">The initial foreground color.</param>
    public FontSettings(FontFamily fontFamily, double fontSize, FontWeight fontWeight, FontStyle fontStyle, Brush foreground)
    {
        FontFamily = fontFamily;
        FontSize = fontSize;
        FontWeight = fontWeight;
        FontStyle = fontStyle;
        Foreground = foreground;
    }

    /// <summary>
    /// Creates a copy of the current font settings.
    /// </summary>
    /// <returns>A new <see cref="FontSettings"/> instance with the same values.</returns>
    public FontSettings Clone()
    {
        return new FontSettings(FontFamily, FontSize, FontWeight, FontStyle, Foreground);
    }
}
