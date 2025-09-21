// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents the base class for an icon UI element.
/// </summary>
[TypeConverter(typeof(IconElementConverter))]
public abstract class IconElement : FrameworkElement
{
    static IconElement()
    {
        FocusableProperty.OverrideMetadata(typeof(IconElement), new FrameworkPropertyMetadata(false));
        KeyboardNavigation.IsTabStopProperty.OverrideMetadata(
            typeof(IconElement),
            new FrameworkPropertyMetadata(false)
        );
    }

    /// <summary>Identifies the <see cref="Foreground"/> dependency property.</summary>
    public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(
        typeof(IconElement),
        new FrameworkPropertyMetadata(
            SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits,
            static (d, args) => ((IconElement)d).OnForegroundChanged(args)
        )
    );

    /// <inheritdoc cref="Control.Foreground"/>
    [Bindable(true)]
    [Category("Appearance")]
    public Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    // Keep a reference to the last effective (non-disabled) foreground so we can restore it
    private Brush _effectiveForeground = Brushes.Transparent;
    private bool _hasEffectiveForeground;

    protected override int VisualChildrenCount => 1;

    private Grid? _layoutRoot;

    protected abstract UIElement InitializeChildren();

    protected virtual void OnForegroundChanged(DependencyPropertyChangedEventArgs args) { }
    protected virtual void OnIsEnabledChangedCore(DependencyPropertyChangedEventArgs e) { }

    public IconElement()
    {
        IsEnabledChanged += IconElement_IsEnabledChanged;
    }

    private void IconElement_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        // When becoming disabled, swap to a disabled foreground brush; when enabled, restore.
        if (!IsEnabled)
        {
            if (!_hasEffectiveForeground)
            {
                _effectiveForeground = Foreground;
                _hasEffectiveForeground = true;
            }

            // Use SystemColors.GrayTextBrush for disabled state
            SetCurrentValue(ForegroundProperty, SystemColors.GrayTextBrush);
        }
        else
        {
            if (_hasEffectiveForeground)
            {
                SetCurrentValue(ForegroundProperty, _effectiveForeground);
                _hasEffectiveForeground = false;
            }
        }

        OnIsEnabledChangedCore(e);
    }

    private void EnsureLayoutRoot()
    {
        if (_layoutRoot != null)
        {
            return;
        }

        _layoutRoot = new Grid { Background = Brushes.Transparent, SnapsToDevicePixels = true };

        _ = _layoutRoot.Children.Add(InitializeChildren());

        AddVisualChild(_layoutRoot);
    }

    protected override Visual GetVisualChild(int index)
    {
        if (index != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "IconElement should have only 1 child");
        }

        EnsureLayoutRoot();
        return _layoutRoot!;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        EnsureLayoutRoot();

        _layoutRoot!.Measure(availableSize);
        return _layoutRoot.DesiredSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        EnsureLayoutRoot();

        _layoutRoot!.Arrange(new Rect(default, finalSize));
        return finalSize;
    }

    /// <summary>
    /// Coerces the value of an Icon dependency property, allowing the use of either IconElement or IconSourceElement.
    /// </summary>
    /// <param name="_">The dependency object (unused).</param>
    /// <param name="baseValue">The value to be coerced.</param>
    /// <returns>An IconElement, either directly or derived from an IconSourceElement.</returns>
    public static object? Coerce(DependencyObject _, object? baseValue)
    {
        return baseValue switch
        {
            IconSourceElement iconSourceElement => iconSourceElement.CreateIconElement(),
            IconElement or null => baseValue,
            _ => throw new ArgumentException(
                message: $"Expected either '{typeof(IconSourceElement)}' or '{typeof(IconElement)}' but got '{baseValue.GetType()}'.",
                paramName: nameof(baseValue)
            ),
        };
    }
}
