// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable

using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a control that displays geometry as a DrawingImage.
/// The geometry is parsed from a string and can be scaled using FontSize and colored using Foreground.
/// </summary>
public class GeometryView : Control
{
    /// <summary>缓存的 Foreground 值，用于检测继承属性变化</summary>
    private Brush? _cachedForeground;

    /// <summary>Identifies the <see cref="GeometryPath"/> dependency property.</summary>
    public static readonly DependencyProperty GeometryPathProperty = DependencyProperty.Register(
        nameof(GeometryPath),
        typeof(Geometry),
        typeof(GeometryView),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnGeometryPathChanged
        )
    );

    /// <summary>Identifies the <see cref="Geometry"/> dependency property.</summary>
    public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register(
        nameof(Geometry),
        typeof(string),
        typeof(GeometryView),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnGeometryChanged
        )
    );

    /// <summary>
    /// Gets or sets the geometry object that will be displayed directly.
    /// Prefer this property when the source data is already a <see cref="Geometry"/> instance.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public Geometry? GeometryPath
    {
        get => (Geometry?)GetValue(GeometryPathProperty);
        set => SetValue(GeometryPathProperty, value);
    }

    /// <summary>
    /// Gets or sets the geometry string that will be parsed and displayed.
    /// The string should be in a format that can be parsed by <see cref="System.Windows.Media.Geometry.Parse"/>.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public string? Geometry
    {
        get => (string?)GetValue(GeometryProperty);
        set => SetValue(GeometryProperty, value);
    }



    /// <summary>
    /// Static constructor to set default style key.
    /// </summary>
    static GeometryView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(GeometryView),
            new FrameworkPropertyMetadata(typeof(GeometryView))
        );
        ForegroundProperty.OverrideMetadata(
            typeof(GeometryView),
            new FrameworkPropertyMetadata(
                Brushes.Black,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
                OnForegroundChanged
            )
        );
        FontSizeProperty.OverrideMetadata(
            typeof(GeometryView),
            new FrameworkPropertyMetadata(
                14.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
                OnFontSizeChanged
            )
        );
        // 监听 IsEnabled 属性变化
        IsEnabledProperty.OverrideMetadata(
            typeof(GeometryView),
            new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnIsEnabledChanged
            )
        );
    }

    /// <summary>
    /// Handles changes to the Geometry property.
    /// </summary>
    private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GeometryView geometryView)
        {
            geometryView.UpdateDrawingImage();
        }
    }

    /// <summary>
    /// Handles changes to the GeometryPath property.
    /// </summary>
    private static void OnGeometryPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GeometryView geometryView)
        {
            geometryView.UpdateDrawingImage();
        }
    }

    /// <summary>
    /// Handles changes to the Foreground property.
    /// </summary>
    private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GeometryView geometryView)
        {
            geometryView.UpdateDrawingImage();
        }
    }

    /// <summary>
    /// Handles changes to the FontSize property.
    /// </summary>
    private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GeometryView geometryView)
        {
            geometryView.UpdateDrawingImage();
        }
    }

    /// <summary>
    /// Handles changes to the IsEnabled property.
    /// </summary>
    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GeometryView geometryView)
        {
            geometryView.UpdateDrawingImage();
        }
    }

    public GeometryView()
    {
        _cachedForeground = Brushes.Black;
    }

    /// <summary>
    /// 重写 OnRender 以检测继承的 Foreground 属性变化。
    /// 当父控件的 Foreground 改变时，继承值会改变，但可能不会触发 PropertyChanged 回调。
    /// </summary>
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        // 检测继承的 Foreground 属性是否改变
        var currentForeground = Foreground;
        if (!BrushesEqual(_cachedForeground, currentForeground))
        {
            _cachedForeground = currentForeground?.CloneCurrentValue() ?? Brushes.Black;
            UpdateDrawingImage();
        }
    }

    /// <summary>
    /// 比较两个 Brush 是否相等。
    /// </summary>
    private static bool BrushesEqual(Brush? brush1, Brush? brush2)
    {
        if (ReferenceEquals(brush1, brush2))
            return true;

        if (brush1 is null || brush2 is null)
            return false;

        if (brush1 is SolidColorBrush scb1 && brush2 is SolidColorBrush scb2)
            return scb1.Color == scb2.Color;

        return brush1.Equals(brush2);
    }

    /// <summary>
    /// Updates the DrawingImage based on the current Geometry, FontSize, and Foreground.
    /// </summary>
    private void UpdateDrawingImage()
    {
        var geometry = GeometryPath;
        if (geometry is null && string.IsNullOrWhiteSpace(Geometry))
        {
            Background = null;
            return;
        }

        try
        {
            if (geometry is null)
            {
                // Parse the geometry string only when a Geometry object is not supplied.
                geometry = System.Windows.Media.Geometry.Parse(Geometry);
            }

            // Create DrawingImage
            var drawingImage = new DrawingImage();
            var drawingGroup = new DrawingGroup();

            // 根据 IsEnabled 状态调整前景色亮度
            Brush brush = Foreground ?? Brushes.Black;
            if (!IsEnabled && brush is SolidColorBrush solidBrush)
            {
                // 禁用状态：降低亮度，使用灰色
                var color = solidBrush.Color;
                // 计算灰度值并降低亮度到 60%
                var disabledColor = Color.FromArgb(
                    color.A,
                    (byte)(color.R * 0.6),
                    (byte)(color.G * 0.6),
                    (byte)(color.B * 0.6)
                );
                brush = new SolidColorBrush(disabledColor);
            }
            else if (!IsEnabled && !(brush is SolidColorBrush))
            {
                // 对于非 SolidColorBrush，使用灰色
                brush = new SolidColorBrush(Color.FromArgb(255, (byte)(211 * 0.6), (byte)(211 * 0.6), (byte)(211 * 0.6)));
            }

            // Create GeometryDrawing
            var geometryDrawing = new GeometryDrawing
            {
                Brush = brush,
                Geometry = geometry
            };


            drawingGroup.Children.Add(geometryDrawing);

            // Scale the drawing based on FontSize
            // Adjust the scale factor according to your needs
            double scaleFactor = FontSize / 24.0; // 24 is the base size
            var scaleTransform = new ScaleTransform(scaleFactor, scaleFactor);
            drawingGroup.Transform = scaleTransform;

            drawingImage.Drawing = drawingGroup;
            Background = new ImageBrush(drawingImage)
            {
                Stretch = Stretch.Uniform,
                AlignmentX = AlignmentX.Center,
                AlignmentY = AlignmentY.Center
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to parse geometry: {ex.Message}");
            Background = null;
        }
    }
}
