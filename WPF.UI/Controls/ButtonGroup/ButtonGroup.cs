// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// ButtonGroup is a control that allows grouping buttons or toggle buttons together.
/// </summary>
/// <remarks>
/// The <see cref="ButtonGroup"/> control inherits from <see cref="ItemsControl"/> and displays
/// a group of buttons with styled connections between them. It supports both horizontal and vertical orientation.
/// </remarks>
public class ButtonGroup : ItemsControl
{
    /// <summary>Identifies the <see cref="Orientation"/> dependency property.</summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(ButtonGroup),
        new PropertyMetadata(Orientation.Horizontal)
    );

    /// <summary>Identifies the <see cref="ItemContainerStyleSelector"/> dependency property.</summary>
    public static readonly DependencyProperty ItemContainerStyleSelectorProperty = DependencyProperty.Register(
        nameof(ItemContainerStyleSelector),
        typeof(StyleSelector),
        typeof(ButtonGroup),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// Gets or sets the orientation of the button group (Horizontal or Vertical).
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the style selector used to select the style for each button in the group.
    /// </summary>
    public new StyleSelector ItemContainerStyleSelector
    {
        get => (StyleSelector)GetValue(ItemContainerStyleSelectorProperty);
        set => SetValue(ItemContainerStyleSelectorProperty, value);
    }

    /// <summary>
    /// Determines whether the specified item is its own container override.
    /// </summary>
    protected override bool IsItemItsOwnContainerOverride(object item) =>
        item is Button or ToggleButton;

    /// <summary>
    /// Called when the collection of items changes.
    /// </summary>
    protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);
        UpdateItemStyles();
    }

    /// <summary>
    /// Called when the control is rendered.
    /// </summary>
    protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        UpdateItemStyles();
    }

    /// <summary>
    /// Updates the styles of all items in the group.
    /// </summary>
    private void UpdateItemStyles()
    {
        if (ItemContainerStyleSelector == null)
            return;

        var count = Items.Count;
        for (var i = 0; i < count; i++)
        {
            if (Items[i] is ButtonBase item)
            {
                var style = ItemContainerStyleSelector.SelectStyle(item, this);
                if (style != null)
                {
                    item.Style = style;
                }
            }
        }
    }
}
