// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

/// <summary>
/// A color wheel control with a circular hue ring and triangular saturation/value selector.
/// </summary>
public class ColorWheel : Control
{
    private const double RingThickness = 20.0;
    private const double ThumbRadius = 6.0;

    private WriteableBitmap? wheelBitmap;
    private bool isMouseDown;
    private bool isDraggingHue;
    private bool isDraggingSV;

    /// <summary>Identifies the <see cref="Hue"/> dependency property.</summary>
    public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
        nameof(Hue),
        typeof(double),
        typeof(ColorWheel),
        new FrameworkPropertyMetadata(
            0.0,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnHueChanged,
            CoerceHue));

    /// <summary>Identifies the <see cref="Saturation"/> dependency property.</summary>
    public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
        nameof(Saturation),
        typeof(double),
        typeof(ColorWheel),
        new FrameworkPropertyMetadata(
            1.0,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnSaturationValueChanged,
            CoerceSaturationValue));

    /// <summary>Identifies the <see cref="Value"/> dependency property.</summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(double),
        typeof(ColorWheel),
        new FrameworkPropertyMetadata(
            1.0,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnSaturationValueChanged,
            CoerceSaturationValue));

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
    /// Gets or sets the value/brightness (0-1).
    /// </summary>
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    static ColorWheel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorWheel), new FrameworkPropertyMetadata(typeof(ColorWheel)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorWheel"/> class.
    /// </summary>
    public ColorWheel()
    {
        SizeChanged += OnSizeChanged;
        Loaded += OnLoaded;
    }

    private static object CoerceHue(DependencyObject d, object baseValue)
    {
        var value = (double)baseValue;
        value = value % 360;
        if (value < 0)
        {
            value += 360;
        }

        return value;
    }

    private static object CoerceSaturationValue(DependencyObject d, object baseValue)
    {
        var value = (double)baseValue;
        return Math.Max(0, Math.Min(1, value));
    }

    private static void OnHueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorWheel wheel)
        {
            wheel.InvalidateVisual();
        }
    }

    private static void OnSaturationValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorWheel wheel)
        {
            wheel.InvalidateVisual();
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        GenerateWheelBitmap();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        GenerateWheelBitmap();
        InvalidateVisual();
    }

    private void GenerateWheelBitmap()
    {
        var size = Math.Min(ActualWidth, ActualHeight);
        if (size <= 0)
        {
            return;
        }

        var pixelWidth = (int)size;
        var pixelHeight = (int)size;

        wheelBitmap = new WriteableBitmap(pixelWidth, pixelHeight, 96, 96, PixelFormats.Bgra32, null);

        var center = size / 2;
        var outerRadius = center;
        var innerRadius = center - RingThickness;

        wheelBitmap.Lock();

        unsafe
        {
            var pBackBuffer = (int*)wheelBitmap.BackBuffer;
            var stride = wheelBitmap.BackBufferStride / 4;

            for (var y = 0; y < pixelHeight; y++)
            {
                for (var x = 0; x < pixelWidth; x++)
                {
                    var dx = x - center;
                    var dy = y - center;
                    var distance = Math.Sqrt(dx * dx + dy * dy);

                    if (distance >= innerRadius && distance <= outerRadius)
                    {
                        var angle = Math.Atan2(dy, dx) * 180 / Math.PI;
                        if (angle < 0)
                        {
                            angle += 360;
                        }

                        var color = ColorHelper.HsvToRgb(angle, 1.0, 1.0, 1.0);
                        var pixelColor = (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
                        pBackBuffer[y * stride + x] = pixelColor;
                    }
                }
            }
        }

        wheelBitmap.AddDirtyRect(new Int32Rect(0, 0, pixelWidth, pixelHeight));
        wheelBitmap.Unlock();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        var size = Math.Min(ActualWidth, ActualHeight);
        if (size <= 0)
        {
            return;
        }

        var center = new Point(ActualWidth / 2, ActualHeight / 2);
        var radius = size / 2;

        // Draw color wheel
        if (wheelBitmap != null)
        {
            drawingContext.DrawImage(wheelBitmap, new Rect(0, 0, size, size));
        }

        // Draw triangle
        DrawTriangle(drawingContext, center, radius - RingThickness);

        // Draw hue thumb
        DrawHueThumb(drawingContext, center, radius - RingThickness / 2);

        // Draw SV thumb
        DrawSVThumb(drawingContext, center, radius - RingThickness);
    }

    private void DrawTriangle(DrawingContext dc, Point center, double radius)
    {
        var hueRad = Hue * Math.PI / 180;

        // Triangle vertices: p1=colored (top), p2=white (bottom-left), p3=black (bottom-right)
        var p1 = new Point(
            center.X + radius * Math.Cos(hueRad),
            center.Y + radius * Math.Sin(hueRad));

        var p2 = new Point(
            center.X + radius * Math.Cos(hueRad + 2 * Math.PI / 3),
            center.Y + radius * Math.Sin(hueRad + 2 * Math.PI / 3));

        var p3 = new Point(
            center.X + radius * Math.Cos(hueRad + 4 * Math.PI / 3),
            center.Y + radius * Math.Sin(hueRad + 4 * Math.PI / 3));

        // Create gradient mesh for triangle
        var streamGeometry = new StreamGeometry();
        using (var ctx = streamGeometry.Open())
        {
            ctx.BeginFigure(p1, true, true);
            ctx.LineTo(p2, true, false);
            ctx.LineTo(p3, true, false);
        }

        streamGeometry.Freeze();

        // Draw the triangle using a mesh of smaller triangles for proper gradient
        var baseColor = ColorHelper.HsvToRgb(Hue, 1.0, 1.0, 1.0);
        
        // Draw multiple gradient segments to simulate 2D gradient
        int steps = 20;
        for (int i = 0; i < steps; i++)
        {
            double t = (double)i / steps;
            double tNext = (double)(i + 1) / steps;
            
            // Interpolate along the edge from white(p2) to colored(p1)
            var edgeStart = new Point(
                p2.X + (p1.X - p2.X) * t,
                p2.Y + (p1.Y - p2.Y) * t);
            var edgeEnd = new Point(
                p2.X + (p1.X - p2.X) * tNext,
                p2.Y + (p1.Y - p2.Y) * tNext);
            
            // Colors at these points
            var colorStart = ColorHelper.HsvToRgb(Hue, t, 1.0, 1.0);
            var colorEnd = ColorHelper.HsvToRgb(Hue, tNext, 1.0, 1.0);
            
            // Draw gradient from edge to black corner (p3)
            var gradBrush = new LinearGradientBrush()
            {
                StartPoint = edgeStart,
                EndPoint = p3,
                GradientStops = new GradientStopCollection
                {
                    new GradientStop(colorStart, 0.0),
                    new GradientStop(Colors.Black, 1.0)
                }
            };
            
            var segmentGeometry = new StreamGeometry();
            using (var ctx = segmentGeometry.Open())
            {
                ctx.BeginFigure(edgeStart, true, true);
                ctx.LineTo(edgeEnd, true, false);
                ctx.LineTo(p3, true, false);
            }
            segmentGeometry.Freeze();
            
            dc.DrawGeometry(gradBrush, null, segmentGeometry);
        }

        // Draw triangle outline
        dc.DrawGeometry(null, new Pen(new SolidColorBrush(Color.FromArgb(128, 128, 128, 128)), 1), streamGeometry);
    }

    private void DrawHueThumb(DrawingContext dc, Point center, double radius)
    {
        var hueRad = Hue * Math.PI / 180;
        var thumbPos = new Point(
            center.X + radius * Math.Cos(hueRad),
            center.Y + radius * Math.Sin(hueRad));

        dc.DrawEllipse(Brushes.White, new Pen(Brushes.Gray, 2), thumbPos, ThumbRadius, ThumbRadius);
    }

    private void DrawSVThumb(DrawingContext dc, Point center, double radius)
    {
        var hueRad = Hue * Math.PI / 180;

        // p1=colored (top), p2=white (bottom-left), p3=black (bottom-right)
        var p1 = new Point(
            center.X + radius * Math.Cos(hueRad),
            center.Y + radius * Math.Sin(hueRad));

        var p2 = new Point(
            center.X + radius * Math.Cos(hueRad + 2 * Math.PI / 3),
            center.Y + radius * Math.Sin(hueRad + 2 * Math.PI / 3));

        var p3 = new Point(
            center.X + radius * Math.Cos(hueRad + 4 * Math.PI / 3),
            center.Y + radius * Math.Sin(hueRad + 4 * Math.PI / 3));

        // Calculate position: interpolate from white(p2) towards colored(p1) by Saturation,
        // then towards black(p3) by (1-Value)
        var whiteToColored = new Point(
            p2.X + (p1.X - p2.X) * Saturation,
            p2.Y + (p1.Y - p2.Y) * Saturation);

        var thumbPos = new Point(
            whiteToColored.X + (p3.X - whiteToColored.X) * (1 - Value),
            whiteToColored.Y + (p3.Y - whiteToColored.Y) * (1 - Value));

        dc.DrawEllipse(Brushes.White, new Pen(Brushes.Gray, 2), thumbPos, ThumbRadius, ThumbRadius);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        isMouseDown = true;
        CaptureMouse();

        var pos = e.GetPosition(this);
        UpdateColorFromPosition(pos);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (isMouseDown)
        {
            var pos = e.GetPosition(this);
            UpdateColorFromPosition(pos);
        }
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);
        isMouseDown = false;
        isDraggingHue = false;
        isDraggingSV = false;
        ReleaseMouseCapture();
    }

    private void UpdateColorFromPosition(Point pos)
    {
        var size = Math.Min(ActualWidth, ActualHeight);
        var center = new Point(ActualWidth / 2, ActualHeight / 2);
        var radius = size / 2;
        var innerRadius = radius - RingThickness;

        var dx = pos.X - center.X;
        var dy = pos.Y - center.Y;
        var distance = Math.Sqrt(dx * dx + dy * dy);

        // Check if clicking on hue ring
        if (distance >= innerRadius && distance <= radius)
        {
            isDraggingHue = true;
            isDraggingSV = false;

            var angle = Math.Atan2(dy, dx) * 180 / Math.PI;
            if (angle < 0)
            {
                angle += 360;
            }

            SetCurrentValue(HueProperty, angle);
        }
        // Check if clicking inside triangle
        else if (distance < innerRadius)
        {
            isDraggingSV = true;
            isDraggingHue = false;

            UpdateSaturationValueFromPosition(pos, center, innerRadius);
        }
    }

    private void UpdateSaturationValueFromPosition(Point pos, Point center, double radius)
    {
        var hueRad = Hue * Math.PI / 180;

        // p1=colored (top), p2=white (bottom-left), p3=black (bottom-right)
        var p1 = new Point(
            center.X + radius * Math.Cos(hueRad),
            center.Y + radius * Math.Sin(hueRad));

        var p2 = new Point(
            center.X + radius * Math.Cos(hueRad + 2 * Math.PI / 3),
            center.Y + radius * Math.Sin(hueRad + 2 * Math.PI / 3));

        var p3 = new Point(
            center.X + radius * Math.Cos(hueRad + 4 * Math.PI / 3),
            center.Y + radius * Math.Sin(hueRad + 4 * Math.PI / 3));

        // Calculate saturation and value from position
        // Project point onto the triangle and calculate distances
        var (s, v) = CalculateSVFromPosition(pos, p1, p2, p3);

        s = Math.Max(0, Math.Min(1, s));
        v = Math.Max(0, Math.Min(1, v));

        SetCurrentValue(SaturationProperty, s);
        SetCurrentValue(ValueProperty, v);
    }

    private (double saturation, double value) CalculateSVFromPosition(Point p, Point colored, Point white, Point black)
    {
        // Use barycentric coordinates with correct vertex mapping
        // colored=p1 (top), white=p2 (bottom-left), black=p3 (bottom-right)
        
        // Vector from white to colored
        var v0x = colored.X - white.X;
        var v0y = colored.Y - white.Y;
        
        // Vector from white to black
        var v1x = black.X - white.X;
        var v1y = black.Y - white.Y;
        
        // Vector from white to point
        var v2x = p.X - white.X;
        var v2y = p.Y - white.Y;

        var dot00 = v0x * v0x + v0y * v0y;
        var dot01 = v0x * v1x + v0y * v1y;
        var dot02 = v0x * v2x + v0y * v2y;
        var dot11 = v1x * v1x + v1y * v1y;
        var dot12 = v1x * v2x + v1y * v2y;

        var denom = dot00 * dot11 - dot01 * dot01;
        if (Math.Abs(denom) < 0.0001)
        {
            return (0, 0);
        }

        var invDenom = 1 / denom;
        var u = (dot11 * dot02 - dot01 * dot12) * invDenom; // weight for colored vertex
        var v = (dot00 * dot12 - dot01 * dot02) * invDenom; // weight for black vertex

        // u represents saturation (distance from white towards colored)
        // v represents darkness (distance from white-colored edge towards black)
        // value = 1 means full brightness (no black), value = 0 means black
        var saturation = Math.Max(0, Math.Min(1, u));
        var value = Math.Max(0, Math.Min(1, 1 - v));

        return (saturation, value);
    }
}
