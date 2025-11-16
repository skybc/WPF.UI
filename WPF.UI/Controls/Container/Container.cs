// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// A layout container control similar to Element Plus &lt;el-container&gt;, containing Header, Main, and Footer areas.
/// </summary>
/// <remarks>
/// The Container control provides a three-row layout structure:
/// - Row 0: Header (Auto height)
/// - Row 1: Main content (fills remaining space with ScrollViewer)
/// - Row 2: Footer (Auto height)
/// </remarks>
public class Container : System.Windows.Controls.Control
{
    /// <summary>Identifies the <see cref="Header"/> dependency property.</summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(Container),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="HeaderTemplate"/> dependency property.</summary>
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
        nameof(HeaderTemplate),
        typeof(DataTemplate),
        typeof(Container),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="Main"/> dependency property.</summary>
    public static readonly DependencyProperty MainProperty = DependencyProperty.Register(
        nameof(Main),
        typeof(object),
        typeof(Container),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="MainTemplate"/> dependency property.</summary>
    public static readonly DependencyProperty MainTemplateProperty = DependencyProperty.Register(
        nameof(MainTemplate),
        typeof(DataTemplate),
        typeof(Container),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="Footer"/> dependency property.</summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
        nameof(Footer),
        typeof(object),
        typeof(Container),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="FooterTemplate"/> dependency property.</summary>
    public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register(
        nameof(FooterTemplate),
        typeof(DataTemplate),
        typeof(Container),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="IsHeaderVisible"/> dependency property.</summary>
    public static readonly DependencyProperty IsHeaderVisibleProperty = DependencyProperty.Register(
        nameof(IsHeaderVisible),
        typeof(bool),
        typeof(Container),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="IsFooterVisible"/> dependency property.</summary>
    public static readonly DependencyProperty IsFooterVisibleProperty = DependencyProperty.Register(
        nameof(IsFooterVisible),
        typeof(bool),
        typeof(Container),
        new PropertyMetadata(true)
    );

    /// <summary>
    /// Gets or sets the header content.
    /// </summary>
    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the header template.
    /// </summary>
    public DataTemplate? HeaderTemplate
    {
        get => (DataTemplate?)GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the main content.
    /// </summary>
    public object? Main
    {
        get => GetValue(MainProperty);
        set => SetValue(MainProperty, value);
    }

    /// <summary>
    /// Gets or sets the main content template.
    /// </summary>
    public DataTemplate? MainTemplate
    {
        get => (DataTemplate?)GetValue(MainTemplateProperty);
        set => SetValue(MainTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the footer content.
    /// </summary>
    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    /// <summary>
    /// Gets or sets the footer template.
    /// </summary>
    public DataTemplate? FooterTemplate
    {
        get => (DataTemplate?)GetValue(FooterTemplateProperty);
        set => SetValue(FooterTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the header is visible.
    /// </summary>
    public bool IsHeaderVisible
    {
        get => (bool)GetValue(IsHeaderVisibleProperty);
        set => SetValue(IsHeaderVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the footer is visible.
    /// </summary>
    public bool IsFooterVisible
    {
        get => (bool)GetValue(IsFooterVisibleProperty);
        set => SetValue(IsFooterVisibleProperty, value);
    }
}
