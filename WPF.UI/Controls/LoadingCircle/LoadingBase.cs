using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Wpf.Ui.Controls;

/// <summary>
/// Provides shared logic for loading visuals that use animated dots.
/// </summary>
public abstract class LoadingBase : ContentControl
{
    protected Storyboard? Storyboard;

    public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register(
        nameof(IsRunning),
        typeof(bool),
        typeof(LoadingBase),
        new PropertyMetadata(true, OnIsRunningChanged));

    public static readonly DependencyProperty DotCountProperty = DependencyProperty.Register(
        nameof(DotCount),
        typeof(int),
        typeof(LoadingBase),
        new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DotIntervalProperty = DependencyProperty.Register(
        nameof(DotInterval),
        typeof(double),
        typeof(LoadingBase),
        new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DotBorderBrushProperty = DependencyProperty.Register(
        nameof(DotBorderBrush),
        typeof(Brush),
        typeof(LoadingBase),
        new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DotBorderThicknessProperty = DependencyProperty.Register(
        nameof(DotBorderThickness),
        typeof(double),
        typeof(LoadingBase),
        new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DotDiameterProperty = DependencyProperty.Register(
        nameof(DotDiameter),
        typeof(double),
        typeof(LoadingBase),
        new FrameworkPropertyMetadata(6.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DotSpeedProperty = DependencyProperty.Register(
        nameof(DotSpeed),
        typeof(double),
        typeof(LoadingBase),
        new FrameworkPropertyMetadata(4.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DotDelayTimeProperty = DependencyProperty.Register(
        nameof(DotDelayTime),
        typeof(double),
        typeof(LoadingBase),
        new FrameworkPropertyMetadata(80.0, FrameworkPropertyMetadataOptions.AffectsRender));

    protected readonly Canvas PrivateCanvas = new()
    {
        ClipToBounds = true
    };

    protected LoadingBase()
    {
        Content = PrivateCanvas;
    }

    public bool IsRunning
    {
        get => (bool)GetValue(IsRunningProperty);
        set => SetValue(IsRunningProperty, value);
    }

    public int DotCount
    {
        get => (int)GetValue(DotCountProperty);
        set => SetValue(DotCountProperty, value);
    }

    public double DotInterval
    {
        get => (double)GetValue(DotIntervalProperty);
        set => SetValue(DotIntervalProperty, value);
    }

    public Brush? DotBorderBrush
    {
        get => (Brush?)GetValue(DotBorderBrushProperty);
        set => SetValue(DotBorderBrushProperty, value);
    }

    public double DotBorderThickness
    {
        get => (double)GetValue(DotBorderThicknessProperty);
        set => SetValue(DotBorderThicknessProperty, value);
    }

    public double DotDiameter
    {
        get => (double)GetValue(DotDiameterProperty);
        set => SetValue(DotDiameterProperty, value);
    }

    public double DotSpeed
    {
        get => (double)GetValue(DotSpeedProperty);
        set => SetValue(DotSpeedProperty, value);
    }

    public double DotDelayTime
    {
        get => (double)GetValue(DotDelayTimeProperty);
        set => SetValue(DotDelayTimeProperty, value);
    }

    protected abstract void UpdateDots();

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        UpdateDots();
    }

    protected virtual Ellipse CreateEllipse(int index)
    {
        var ellipse = new Ellipse();
        ellipse.SetBinding(WidthProperty, new Binding(DotDiameterProperty.Name) { Source = this });
        ellipse.SetBinding(HeightProperty, new Binding(DotDiameterProperty.Name) { Source = this });
        ellipse.SetBinding(Shape.FillProperty, new Binding(ForegroundProperty.Name) { Source = this });
        ellipse.SetBinding(Shape.StrokeThicknessProperty, new Binding(DotBorderThicknessProperty.Name) { Source = this });
        ellipse.SetBinding(Shape.StrokeProperty, new Binding(DotBorderBrushProperty.Name) { Source = this });
        return ellipse;
    }

    private static void OnIsRunningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not LoadingBase control)
        {
            return;
        }

        if (control.Storyboard is null)
        {
            return;
        }

        if ((bool)e.NewValue)
        {
            control.Storyboard.Resume();
        }
        else
        {
            control.Storyboard.Pause();
        }
    }
}