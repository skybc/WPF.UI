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
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnForegroundChanged
            )
        );
        FontSizeProperty.OverrideMetadata(
            typeof(GeometryView),
            new FrameworkPropertyMetadata(
                14.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                OnFontSizeChanged
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

    public GeometryView()
    {
        
    }

    /// <summary>
    /// Updates the DrawingImage based on the current Geometry, FontSize, and Foreground.
    /// </summary>
    private void UpdateDrawingImage()
    {
        if (string.IsNullOrEmpty(Geometry))
        {
            Background = null;
            return;
        }

        try
        {        
            // Parse the geometry string
            var geometry = System.Windows.Media.Geometry.Parse(Geometry);

            // Create DrawingImage
            var drawingImage = new DrawingImage();
            var drawingGroup = new DrawingGroup();

            // Create GeometryDrawing
            var geometryDrawing = new GeometryDrawing
            {
                Brush = Foreground ?? Brushes.Black,
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
