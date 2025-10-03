// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// A divider control that displays a horizontal or vertical line with optional text.
/// Can be used to visually separate content in forms, panels, or other containers.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:Divider /&gt;
/// &lt;ui:Divider Content="Section Title" /&gt;
/// &lt;ui:Divider Content="Left Aligned" ContentAlignment="Left" /&gt;
/// &lt;ui:Divider Orientation="Vertical" Height="100" /&gt;
/// </code>
/// </example>
public class Divider : ContentControl
{
    /// <summary>Identifies the <see cref="Orientation"/> dependency property.</summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(Divider),
        new PropertyMetadata(Orientation.Horizontal)
    );

    /// <summary>Identifies the <see cref="ContentAlignment"/> dependency property.</summary>
    public static readonly DependencyProperty ContentAlignmentProperty = DependencyProperty.Register(
        nameof(ContentAlignment),
        typeof(HorizontalAlignment),
        typeof(Divider),
        new PropertyMetadata(HorizontalAlignment.Center)
    );

    /// <summary>Identifies the <see cref="LineThickness"/> dependency property.</summary>
    public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register(
        nameof(LineThickness),
        typeof(double),
        typeof(Divider),
        new PropertyMetadata(1.0)
    );

    /// <summary>Identifies the <see cref="LineStroke"/> dependency property.</summary>
    public static readonly DependencyProperty LineStrokeProperty = DependencyProperty.Register(
        nameof(LineStroke),
        typeof(System.Windows.Media.Brush),
        typeof(Divider),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="Spacing"/> dependency property.</summary>
    public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
        nameof(Spacing),
        typeof(double),
        typeof(Divider),
        new PropertyMetadata(12.0)
    );

    /// <summary>
    /// Gets or sets the orientation of the divider (Horizontal or Vertical).
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the alignment of the content text (Left, Center, or Right).
    /// Only applies when Orientation is Horizontal.
    /// </summary>
    public HorizontalAlignment ContentAlignment
    {
        get => (HorizontalAlignment)GetValue(ContentAlignmentProperty);
        set => SetValue(ContentAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the thickness of the divider line.
    /// </summary>
    public double LineThickness
    {
        get => (double)GetValue(LineThicknessProperty);
        set => SetValue(LineThicknessProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush used to draw the divider line.
    /// If not set, uses the default theme color.
    /// </summary>
    public System.Windows.Media.Brush LineStroke
    {
        get => (System.Windows.Media.Brush)GetValue(LineStrokeProperty);
        set => SetValue(LineStrokeProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between the content text and the divider lines.
    /// </summary>
    public double Spacing
    {
        get => (double)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }
}
