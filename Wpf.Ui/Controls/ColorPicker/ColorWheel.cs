// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

/// <summary>
/// Interactive HSV color wheel used by <see cref="ColorPicker"/>.
/// </summary>
public class ColorWheel : FrameworkElement
{
    private const double ValueStartAngle = -90.0;

    /// <summary>Identifies the <see cref="Hue"/> dependency property.</summary>
    public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
        nameof(Hue),
        typeof(double),
        typeof(ColorWheel),
        new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHueChanged, CoerceHue));

    /// <summary>Identifies the <see cref="Saturation"/> dependency property.</summary>
    public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
        nameof(Saturation),
        typeof(double),
        typeof(ColorWheel),
        new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnWheelCoordinateChanged, CoerceNormalized));

    /// <summary>Identifies the <see cref="Value"/> dependency property.</summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(double),
        typeof(ColorWheel),
        new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, CoerceNormalized));

    private bool isDraggingHueSaturation;
    private bool isDraggingValue;
    private DrawingGroup? cachedColorWheelDrawing;
    private Size cachedSize;

    static ColorWheel()
    {
        FocusableProperty.OverrideMetadata(typeof(ColorWheel), new FrameworkPropertyMetadata(true));
        SnapsToDevicePixelsProperty.OverrideMetadata(typeof(ColorWheel), new FrameworkPropertyMetadata(true));
    }

    /// <summary>
    /// Gets or sets the hue component (0-360).
    /// </summary>
    public double Hue
    {
        get => (double)this.GetValue(HueProperty);
        set => this.SetValue(HueProperty, value);
    }

    /// <summary>
    /// Gets or sets the saturation component (0-1).
    /// </summary>
    public double Saturation
    {
        get => (double)this.GetValue(SaturationProperty);
        set => this.SetValue(SaturationProperty, value);
    }

    /// <summary>
    /// Gets or sets the value/brightness component (0-1).
    /// </summary>
    public double Value
    {
        get => (double)this.GetValue(ValueProperty);
        set => this.SetValue(ValueProperty, value);
    }

    /// <inheritdoc />
    protected override Size MeasureOverride(Size availableSize)
    {
        var desired = 200d;

        if (!double.IsInfinity(availableSize.Width) && !double.IsInfinity(availableSize.Height))
        {
            desired = Math.Min(Math.Min(availableSize.Width, availableSize.Height), desired);
        }

        return new Size(desired, desired);
    }

    /// <inheritdoc />
    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        this.cachedColorWheelDrawing = null;
        this.InvalidateVisual();
    }

    /// <inheritdoc />
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (this.ActualWidth <= 0 || this.ActualHeight <= 0)
        {
            return;
        }

        var renderRect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
        var currentSize = new Size(this.ActualWidth, this.ActualHeight);
        
        // 如果尺寸变化或缓存不存在，重新创建内圈色轮缓存
        if (this.cachedColorWheelDrawing == null || this.cachedSize != currentSize)
        {
            this.cachedColorWheelDrawing = this.CreateColorWheelDrawing();
            this.cachedSize = currentSize;
        }
        
        // 绘制缓存的内圈色轮
        if (this.cachedColorWheelDrawing != null)
        {
            drawingContext.DrawDrawing(this.cachedColorWheelDrawing);
        }
        
        // 绘制外圈亮度环（根据当前 Hue 动态变化）
        this.DrawValueRing(drawingContext, renderRect);
        
        // 绘制指针
        this.DrawPointers(drawingContext, renderRect);
    }

    /// <inheritdoc />
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        this.Focus();
        this.CaptureMouse();

        var position = e.GetPosition(this);
        this.DetermineDragMode(position);
        this.UpdateComponentsFromPoint(position);
    }

    /// <inheritdoc />
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (this.isDraggingHueSaturation || this.isDraggingValue)
        {
            var position = e.GetPosition(this);
            this.UpdateComponentsFromPoint(position);
        }
    }

    /// <inheritdoc />
    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);

        this.isDraggingHueSaturation = false;
        this.isDraggingValue = false;
        this.ReleaseMouseCapture();
    }

    private static void OnHueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorWheel wheel)
        {
            wheel.InvalidateVisual();
        }
    }

    private static void OnWheelCoordinateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorWheel wheel)
        {
            wheel.InvalidateVisual();
        }
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorWheel wheel)
        {
            wheel.InvalidateVisual();
        }
    }

    private static object CoerceHue(DependencyObject d, object baseValue)
    {
        var hue = (double)baseValue;
        hue %= 360d;

        if (hue < 0d)
        {
            hue += 360d;
        }

        return hue;
    }

    private static object CoerceNormalized(DependencyObject d, object baseValue)
    {
        return Math.Clamp((double)baseValue, 0d, 1d);
    }

    private void DetermineDragMode(Point position)
    {
        var layout = this.GetLayoutMetrics();
        var dx = position.X - layout.Center.X;
        var dy = position.Y - layout.Center.Y;
        var distance = Math.Sqrt((dx * dx) + (dy * dy));

        if (distance <= layout.InnerRadius)
        {
            this.isDraggingHueSaturation = true;
            this.isDraggingValue = false;
        }
        else if (distance <= layout.OuterRadius)
        {
            this.isDraggingValue = true;
            this.isDraggingHueSaturation = false;
        }
        else
        {
            this.isDraggingHueSaturation = false;
            this.isDraggingValue = false;
        }
    }

    private void UpdateComponentsFromPoint(Point position)
    {
        var layout = this.GetLayoutMetrics();
        var dx = position.X - layout.Center.X;
        var dy = layout.Center.Y - position.Y; // invert Y for trigonometry
        var distance = Math.Sqrt((dx * dx) + (dy * dy));

        if (this.isDraggingHueSaturation && layout.InnerRadius > 0)
        {
            var angle = Math.Atan2(dy, dx);
            var hue = NormalizeAngle((angle * 180d / Math.PI));
            var saturation = Math.Clamp(distance / layout.InnerRadius, 0d, 1d);

            this.SetCurrentValue(HueProperty, hue);
            this.SetCurrentValue(SaturationProperty, saturation);
        }
        else if (this.isDraggingValue)
        {
            var angle = Math.Atan2(dy, dx);
            var normalizedAngle = NormalizeAngle((angle * 180d / Math.PI));
            var t = AngleToValue(normalizedAngle);
            
            // 根据外圈位置反向计算 HSV 值
            // 外圈显示：白色(t=0) → 纯色(t=0.25) → 黑色(t=0.5) → 纯色(t=0.75) → 白色(t=1)
            // 需要将这个映射转换为 HSV 的 Saturation 和 Value
            double saturation, value;
            
            if (t <= 0.25d)
            {
                // 白色到纯色：饱和度从0到1，亮度保持1
                var blend = t * 4d;
                saturation = blend;
                value = 1d;
            }
            else if (t <= 0.5d)
            {
                // 纯色到黑色：饱和度保持1，亮度从1到0
                var blend = (t - 0.25d) * 4d;
                saturation = 1d;
                value = 1d - blend;
            }
            else if (t <= 0.75d)
            {
                // 黑色到纯色：饱和度保持1，亮度从0到1
                var blend = (t - 0.5d) * 4d;
                saturation = 1d;
                value = blend;
            }
            else
            {
                // 纯色到白色：饱和度从1到0，亮度保持1
                var blend = (t - 0.75d) * 4d;
                saturation = 1d - blend;
                value = 1d;
            }
            
            this.SetCurrentValue(SaturationProperty, saturation);
            this.SetCurrentValue(ValueProperty, value);
        }
    }

    /// <summary>
    /// 创建内圈色轮的 Drawing（可缓存）.
    /// </summary>
    private DrawingGroup CreateColorWheelDrawing()
    {
        var drawingGroup = new DrawingGroup();
        
        using (var drawingContext = drawingGroup.Open())
        {
            var layout = this.GetLayoutMetrics();
            var center = layout.Center;
            var radius = layout.InnerRadius;

            if (radius <= 0)
            {
                return drawingGroup;
            }

            // 使用分段绘制的方式来创建色轮
            const int segments = 360; // 色相分段数
            const int rings = 50; // 径向分段数

            for (int i = 0; i < segments; i++)
            {
                double startAngle = i * 360.0 / segments;
                double endAngle = (i + 1) * 360.0 / segments;
                double midAngle = (startAngle + endAngle) / 2.0;

                for (int j = 0; j < rings; j++)
                {
                    double innerRatio = (double)j / rings;
                    double outerRatio = (double)(j + 1) / rings;
                    double midRatio = (innerRatio + outerRatio) / 2.0;

                    // 计算该区域的颜色
                    var color = ColorHelper.HsvToRgb(midAngle, midRatio, 1.0);
                    var brush = new SolidColorBrush(color);
                    brush.Freeze();

                    // 创建扇形几何
                    var geometry = CreateWedgeGeometry(center, radius * innerRatio, radius * outerRatio, startAngle, endAngle);
                    drawingContext.DrawGeometry(brush, null, geometry);
                }
            }
        }
        
        drawingGroup.Freeze();
        return drawingGroup;
    }

    /// <summary>
    /// 绘制外圈亮度环
    /// </summary>
    private void DrawValueRing(DrawingContext drawingContext, Rect renderRect)
    {
        var layout = this.GetLayoutMetrics();
        var center = layout.Center;
        var innerRadius = layout.OuterRadius - layout.RingThickness;
        var outerRadius = layout.OuterRadius;

        if (innerRadius <= 0)
        {
            return;
        }

        // 绘制外圈的渐变环
        const int segments = 360;

        for (int i = 0; i < segments; i++)
        {
            double startAngle = i * 360.0 / segments;
            double endAngle = (i + 1) * 360.0 / segments;
            double midAngle = (startAngle + endAngle) / 2.0;

            // 计算 t 值
            var normalizedAngle = NormalizeAngle(midAngle);
            var t = AngleToValue(normalizedAngle);
            
            // 获取该位置的颜色
            var color = this.GetValueRingColor(t);
            var brush = new SolidColorBrush(color);
            brush.Freeze();

            // 创建扇形几何
            var geometry = CreateWedgeGeometry(center, innerRadius, outerRadius, startAngle, endAngle);
            drawingContext.DrawGeometry(brush, null, geometry);
        }
    }

    /// <summary>
    /// 创建扇形几何形状
    /// </summary>
    private static StreamGeometry CreateWedgeGeometry(Point center, double innerRadius, double outerRadius, double startAngle, double endAngle)
    {
        var geometry = new StreamGeometry();
        using (var context = geometry.Open())
        {
            // 转换角度为弧度，并调整坐标系（WPF 中 Y 轴向下）
            var startRad = (startAngle - 90) * Math.PI / 180.0;
            var endRad = (endAngle - 90) * Math.PI / 180.0;

            // 外弧起点
            var outerStart = new Point(
                center.X + (outerRadius * Math.Cos(startRad)),
                center.Y + (outerRadius * Math.Sin(startRad)));

            // 外弧终点
            var outerEnd = new Point(
                center.X + (outerRadius * Math.Cos(endRad)),
                center.Y + (outerRadius * Math.Sin(endRad)));

            // 内弧终点
            var innerEnd = new Point(
                center.X + (innerRadius * Math.Cos(endRad)),
                center.Y + (innerRadius * Math.Sin(endRad)));

            // 内弧起点
            var innerStart = new Point(
                center.X + (innerRadius * Math.Cos(startRad)),
                center.Y + (innerRadius * Math.Sin(startRad)));

            context.BeginFigure(outerStart, true, true);
            
            // 绘制外弧
            var isLargeArc = (endAngle - startAngle) > 180;
            context.ArcTo(outerEnd, new Size(outerRadius, outerRadius), 0, isLargeArc, SweepDirection.Clockwise, true, true);
            
            // 连接到内弧
            context.LineTo(innerEnd, true, true);
            
            // 绘制内弧（反向）
            context.ArcTo(innerStart, new Size(innerRadius, innerRadius), 0, isLargeArc, SweepDirection.Counterclockwise, true, true);
            
            // 闭合路径
            context.LineTo(outerStart, true, true);
        }
        
        geometry.Freeze();
        return geometry;
    }

    private Color GetValueRingColor(double t)
    {
        t = Math.Clamp(t, 0d, 1d);

        // 获取当前选择的纯色（饱和度=1，亮度=1）
        var baseColor = ColorHelper.HsvToRgb(this.Hue, 1d, 1d);

        // 底部(t=0, -90度)为白色，顶部(t=0.5, 90度)为黑色，再回到底部(t=1, 270度)为白色
        // 0 -> 0.25: 白色到选择的颜色
        // 0.25 -> 0.5: 选择的颜色到黑色
        // 0.5 -> 0.75: 黑色到选择的颜色
        // 0.75 -> 1: 选择的颜色到白色
        if (t <= 0.25d)
        {
            // 从白色渐变到选择的颜色
            var blend = t * 4d; // 0 -> 1
            return LerpColor(Colors.White, baseColor, blend);
        }
        else if (t <= 0.5d)
        {
            // 从选择的颜色渐变到黑色
            var blend = (t - 0.25d) * 4d; // 0 -> 1
            return LerpColor(baseColor, Colors.Black, blend);
        }
        else if (t <= 0.75d)
        {
            // 从黑色渐变到选择的颜色
            var blend = (t - 0.5d) * 4d; // 0 -> 1
            return LerpColor(Colors.Black, baseColor, blend);
        }
        else
        {
            // 从选择的颜色渐变到白色
            var blend = (t - 0.75d) * 4d; // 0 -> 1
            return LerpColor(baseColor, Colors.White, blend);
        }
    }

    private void DrawPointers(DrawingContext drawingContext, Rect renderRect)
    {
        var layout = this.GetLayoutMetrics();

        var hueRadians = this.Hue * Math.PI / 180d;
        var saturationRadius = layout.InnerRadius * this.Saturation;

        var innerPoint = new Point(
            layout.Center.X + (Math.Cos(hueRadians) * saturationRadius),
            layout.Center.Y - (Math.Sin(hueRadians) * saturationRadius));

        // 根据当前 Saturation 和 Value 计算外圈的 t 值
        var t = SaturationValueToRingPosition(this.Saturation, this.Value);
        var valueAngle = ValueToAngle(t) * Math.PI / 180d;

        // 外圈指针应该在外圈的中心位置（考虑10px间隙）
        var ringRadius = layout.OuterRadius - (layout.RingThickness / 2d);
        var outerPoint = new Point(
            layout.Center.X + (Math.Cos(valueAngle) * ringRadius),
            layout.Center.Y - (Math.Sin(valueAngle) * ringRadius));

        var pointerBrush = Brushes.White;
        var pointerStroke = new Pen(Brushes.Black, layout.PointerStrokeThickness);

        drawingContext.DrawEllipse(pointerBrush, pointerStroke, innerPoint, layout.InnerPointerRadius, layout.InnerPointerRadius);
        drawingContext.DrawEllipse(pointerBrush, pointerStroke, outerPoint, layout.OuterPointerRadius, layout.OuterPointerRadius);
    }

    private LayoutMetrics GetLayoutMetrics()
    {
        return this.GetLayoutMetrics(this.ActualWidth, this.ActualHeight);
    }

    private LayoutMetrics GetLayoutMetrics(double width, double height)
    {
        var radius = Math.Max(0d, Math.Min(width, height) / 2d);
        var ringThickness = 10d; // 固定外圈宽度为10px
        var gapSize = 10d; // 内圆和外圈之间的间隙为10px
        var outerRadius = Math.Max(0d, radius - 1d);
        var innerRadius = Math.Max(0d, outerRadius - ringThickness - gapSize);

        return new LayoutMetrics
        {
            Center = new Point(width / 2d, height / 2d),
            InnerRadius = innerRadius,
            OuterRadius = outerRadius,
            RingThickness = ringThickness,
            InnerPointerRadius = Math.Max(4d, ringThickness * 0.35d),
            OuterPointerRadius = Math.Max(4d, ringThickness * 0.45d),
            PointerStrokeThickness = Math.Max(1d, ringThickness * 0.08d)
        };
    }

    private static double NormalizeAngle(double angle)
    {
        angle %= 360d;
        if (angle < 0d)
        {
            angle += 360d;
        }

        return angle;
    }

    private static double AngleToValue(double angle)
    {
        var adjusted = NormalizeAngle(angle - ValueStartAngle);
        return adjusted / 360d;
    }

    private static double ValueToAngle(double value)
    {
        value = Math.Clamp(value, 0d, 1d);
        return ValueStartAngle + (value * 360d);
    }

    /// <summary>
    /// 将外圈的 Saturation 和 Value 转换为环上的位置参数 t (0-1).
    /// </summary>
    private static double SaturationValueToRingPosition(double saturation, double value)
    {
        saturation = Math.Clamp(saturation, 0d, 1d);
        value = Math.Clamp(value, 0d, 1d);

        // 外圈显示：白色(t=0) → 纯色(t=0.25) → 黑色(t=0.5) → 纯色(t=0.75) → 白色(t=1)
        // 根据 HSV 颜色模型：
        // - 白色: V=1, S=0
        // - 纯色: V=1, S=1
        // - 黑色: V=0, S=任意值(通常为1)

        // 使用阈值来判断是在 Value 主导区域还是 Saturation 主导区域
        const double threshold = 0.8d;

        if (value > threshold)
        {
            // Value 较高，在白色-纯色区域
            if (saturation <= 0.5d)
            {
                // 白色(sat=0, t=0) 到 纯色(sat=1, t=0.25)
                return saturation * 0.25d;
            }
            else
            {
                // 纯色(sat=1, t=0.75) 到 白色(sat=0, t=1)
                return 0.75d + ((1d - saturation) * 0.25d);
            }
        }
        else
        {
            // Value 较低，在纯色-黑色区域
            if (value >= 0.5d)
            {
                // 纯色(val=1, t=0.25) 到 黑色(val=0, t=0.5)
                return 0.25d + ((1d - value) * 0.25d);
            }
            else
            {
                // 黑色(val=0, t=0.5) 到 纯色(val=1, t=0.75)
                return 0.5d + (value * 0.25d);
            }
        }
    }

    private static Color LerpColor(Color from, Color to, double t)
    {
        t = Math.Clamp(t, 0d, 1d);
        return Color.FromArgb(
            (byte)Math.Round(from.A + ((to.A - from.A) * t)),
            (byte)Math.Round(from.R + ((to.R - from.R) * t)),
            (byte)Math.Round(from.G + ((to.G - from.G) * t)),
            (byte)Math.Round(from.B + ((to.B - from.B) * t)));
    }

    private readonly struct LayoutMetrics
    {
        public Point Center { get; init; }

        public double InnerRadius { get; init; }

        public double OuterRadius { get; init; }

        public double RingThickness { get; init; }

        public double InnerPointerRadius { get; init; }

        public double OuterPointerRadius { get; init; }

        public double PointerStrokeThickness { get; init; }
    }
}
