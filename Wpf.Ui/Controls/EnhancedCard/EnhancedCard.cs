// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Input;

namespace Wpf.Ui.Controls;

/// <summary>
/// Enhanced Card control with Header, Content, Footer slots, elevation shadow, and optional click support.
/// </summary>
public class EnhancedCard : System.Windows.Controls.ContentControl
{
    /// <summary>Identifies the <see cref="Header"/> dependency property.</summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(EnhancedCard),
        new PropertyMetadata(null, OnHeaderChanged)
    );

    /// <summary>Identifies the <see cref="Footer"/> dependency property.</summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
        nameof(Footer),
        typeof(object),
        typeof(EnhancedCard),
        new PropertyMetadata(null, OnFooterChanged)
    );

    /// <summary>Identifies the <see cref="HasHeader"/> dependency property.</summary>
    public static readonly DependencyProperty HasHeaderProperty = DependencyProperty.Register(
        nameof(HasHeader),
        typeof(bool),
        typeof(EnhancedCard),
        new PropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="HasFooter"/> dependency property.</summary>
    public static readonly DependencyProperty HasFooterProperty = DependencyProperty.Register(
        nameof(HasFooter),
        typeof(bool),
        typeof(EnhancedCard),
        new PropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="CornerRadius"/> dependency property.</summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(EnhancedCard),
        new PropertyMetadata(new CornerRadius(4))
    );

    /// <summary>Identifies the <see cref="Elevation"/> dependency property.</summary>
    public static readonly DependencyProperty ElevationProperty = DependencyProperty.Register(
        nameof(Elevation),
        typeof(int),
        typeof(EnhancedCard),
        new PropertyMetadata(2, OnElevationChanged, CoerceElevation)
    );

    /// <summary>Identifies the <see cref="Command"/> dependency property.</summary>
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(ICommand),
        typeof(EnhancedCard),
        new PropertyMetadata(null, OnCommandChanged)
    );

    /// <summary>Identifies the <see cref="CommandParameter"/> dependency property.</summary>
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        nameof(CommandParameter),
        typeof(object),
        typeof(EnhancedCard),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="IsClickable"/> dependency property.</summary>
    public static readonly DependencyProperty IsClickableProperty = DependencyProperty.Register(
        nameof(IsClickable),
        typeof(bool),
        typeof(EnhancedCard),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// Gets or sets the header content displayed at the top of the card.
    /// </summary>
    [Bindable(true)]
    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the footer content displayed at the bottom of the card.
    /// </summary>
    [Bindable(true)]
    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="EnhancedCard"/> has a <see cref="Header"/>.
    /// </summary>
    public bool HasHeader
    {
        get => (bool)GetValue(HasHeaderProperty);
        internal set => SetValue(HasHeaderProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="EnhancedCard"/> has a <see cref="Footer"/>.
    /// </summary>
    public bool HasFooter
    {
        get => (bool)GetValue(HasFooterProperty);
        internal set => SetValue(HasFooterProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius of the card.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the elevation level (shadow depth) of the card. 
    /// Valid range: 0-5, where 0 is no shadow and 5 is maximum shadow.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public int Elevation
    {
        get => (int)GetValue(ElevationProperty);
        set => SetValue(ElevationProperty, value);
    }

    /// <summary>
    /// Gets or sets the command to invoke when the card is clicked.
    /// </summary>
    [Bindable(true)]
    [Category("Action")]
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the parameter to pass to the <see cref="Command"/>.
    /// </summary>
    [Bindable(true)]
    [Category("Action")]
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the card is clickable (shows hover effects).
    /// </summary>
    [Bindable(true)]
    [Category("Behavior")]
    public bool IsClickable
    {
        get => (bool)GetValue(IsClickableProperty);
        set => SetValue(IsClickableProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnhancedCard"/> class.
    /// </summary>
    public EnhancedCard()
    {
        MouseLeftButtonDown += OnMouseLeftButtonDown;
    }

    private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not EnhancedCard control)
        {
            return;
        }

        control.SetValue(HasHeaderProperty, control.Header != null);
    }

    private static void OnFooterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not EnhancedCard control)
        {
            return;
        }

        control.SetValue(HasFooterProperty, control.Footer != null);
    }

    private static void OnElevationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Elevation changes are handled by style triggers
    }

    private static object CoerceElevation(DependencyObject d, object baseValue)
    {
        // Clamp elevation to valid range 0-5
        if (baseValue is int elevation)
        {
            return Math.Clamp(elevation, 0, 5);
        }

        return 2; // Default
    }

    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not EnhancedCard control)
        {
            return;
        }

        // Automatically set IsClickable if Command is set
        if (e.NewValue is ICommand)
        {
            control.IsClickable = true;
        }
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (Command is not null && Command.CanExecute(CommandParameter))
        {
            Command.Execute(CommandParameter);
            e.Handled = true;
        }
    }
}
