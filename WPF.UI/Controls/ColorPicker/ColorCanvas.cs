// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

/// <summary>
/// A 2D color canvas for selecting Saturation and Value (brightness) with a fixed Hue.
/// </summary>
public class ColorCanvas : Control
{
    private Ellipse? thumbEllipse;
    private bool isDragging;

    /// <summary>Identifies the <see cref="Hue"/> dependency property.</summary>
    public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
        nameof(Hue),
        typeof(double),
        typeof(ColorCanvas),
        new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender, OnHueChanged));

    /// <summary>Identifies the <see cref="Saturation"/> dependency property.</summary>
    public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
        nameof(Saturation),
        typeof(double),
        typeof(ColorCanvas),
        new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender, OnSaturationValueChanged, CoerceNormalized));

    /// <summary>Identifies the <see cref="Value"/> dependency property.</summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(double),
        typeof(ColorCanvas),
        new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender, OnSaturationValueChanged, CoerceNormalized));

    static ColorCanvas()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorCanvas), new FrameworkPropertyMetadata(typeof(ColorCanvas)));
    }

    /// <summary>
    /// Gets or sets the hue component (0-360).
    /// </summary>
    public double Hue
    {
        get => (double)GetValue(HueProperty);
        set => SetValue(HueProperty, value);
    }

    /// <summary>
    /// Gets or sets the saturation component (0-1).
    /// </summary>
    public double Saturation
    {
        get => (double)GetValue(SaturationProperty);
        set => SetValue(SaturationProperty, value);
    }

    /// <summary>
    /// Gets or sets the value/brightness component (0-1).
    /// </summary>
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        thumbEllipse = GetTemplateChild("PART_Thumb") as Ellipse;
        UpdateThumbPosition();
    }

    /// <inheritdoc />
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (ActualWidth <= 0 || ActualHeight <= 0)
        {
            return;
        }

        var rect = new Rect(0, 0, ActualWidth, ActualHeight);

        // Create gradient brushes for the canvas
        // Horizontal: White to Pure Color (based on Hue)
        var pureColor = ColorHelper.HsvToRgb(Hue, 1, 1);
        var horizontalGradient = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 0)
        };
        horizontalGradient.GradientStops.Add(new GradientStop(Colors.White, 0));
        horizontalGradient.GradientStops.Add(new GradientStop(pureColor, 1));

        drawingContext.DrawRectangle(horizontalGradient, null, rect);

        // Vertical: Transparent to Black
        var verticalGradient = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1)
        };
        verticalGradient.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0));
        verticalGradient.GradientStops.Add(new GradientStop(Colors.Black, 1));

        drawingContext.DrawRectangle(verticalGradient, null, rect);
    }

    /// <inheritdoc />
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        
        if (CaptureMouse())
        {
            isDragging = true;
            Focus();
            UpdateColorFromPoint(e.GetPosition(this));
            e.Handled = true;
        }
    }

    /// <inheritdoc />
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (isDragging)
        {
            UpdateColorFromPoint(e.GetPosition(this));
            e.Handled = true;
        }
    }

    /// <inheritdoc />
    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);

        if (isDragging)
        {
            isDragging = false;
            ReleaseMouseCapture();
            e.Handled = true;
        }
    }

    private static void OnHueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorCanvas canvas)
        {
            canvas.InvalidateVisual();
        }
    }

    private static void OnSaturationValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorCanvas canvas)
        {
            canvas.UpdateThumbPosition();
        }
    }

    private static object CoerceNormalized(DependencyObject d, object baseValue)
    {
        return Math.Clamp((double)baseValue, 0d, 1d);
    }

    private void UpdateColorFromPoint(Point position)
    {
        var saturation = Math.Clamp(position.X / ActualWidth, 0, 1);
        var value = Math.Clamp(1 - (position.Y / ActualHeight), 0, 1);

        SetCurrentValue(SaturationProperty, saturation);
        SetCurrentValue(ValueProperty, value);
    }

    private void UpdateThumbPosition()
    {
        if (thumbEllipse == null || ActualWidth <= 0 || ActualHeight <= 0)
        {
            return;
        }

        var x = Saturation * ActualWidth;
        var y = (1 - Value) * ActualHeight;

        Canvas.SetLeft(thumbEllipse, x - thumbEllipse.Width / 2);
        Canvas.SetTop(thumbEllipse, y - thumbEllipse.Height / 2);
    }
}
