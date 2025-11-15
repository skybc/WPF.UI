// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// StyleSelector for ButtonGroup items. Selects appropriate style based on button type and position.
/// </summary>
public class ButtonGroupItemStyleSelector : StyleSelector
{
    private static readonly Dictionary<string, Style?> StyleDict = new();

    /// <summary>
    /// Selects the appropriate style for a ButtonGroup item.
    /// </summary>
    public override Style? SelectStyle(object item, DependencyObject container)
    {
        if (container is not ButtonGroup buttonGroup || item is not ButtonBase buttonBase)
            return null;

        var count = GetVisibleButtonsCount(buttonGroup);

        return buttonBase switch
        {
            Button => GetButtonStyle(count, buttonGroup, buttonBase),
            ToggleButton => GetToggleButtonStyle(count, buttonGroup, buttonBase),
            _ => null
        };
    }

    /// <summary>
    /// Gets the count of visible buttons in the group.
    /// </summary>
    private static int GetVisibleButtonsCount(ButtonGroup buttonGroup)
    {
        return buttonGroup.Items.OfType<ButtonBase>().Count(button => button.IsVisible);
    }

    /// <summary>
    /// Gets the appropriate style for a Button.
    /// </summary>
    private static Style? GetButtonStyle(int count, ButtonGroup buttonGroup, ButtonBase button)
    {
        var index = buttonGroup.Items.IndexOf(button);
        var resourceKey = GetStyleResourceKey(count, index, buttonGroup.Orientation, "Button");

        return TryGetStyle(resourceKey);
    }

    /// <summary>
    /// Gets the appropriate style for a ToggleButton.
    /// </summary>
    private static Style? GetToggleButtonStyle(int count, ButtonGroup buttonGroup, ButtonBase button)
    {
        var index = buttonGroup.Items.IndexOf(button);
        var resourceKey = GetStyleResourceKey(count, index, buttonGroup.Orientation, "ToggleButton");

        return TryGetStyle(resourceKey);
    }

    /// <summary>
    /// Gets the resource key for the appropriate style.
    /// </summary>
    private static string GetStyleResourceKey(int count, int index, Orientation orientation, string buttonType)
    {
        if (count == 1)
            return $"ButtonGroupItem{buttonType}Single";

        return orientation == Orientation.Horizontal
            ? index == 0
                ? $"ButtonGroupItem{buttonType}HorizontalFirst"
                : index == count - 1
                    ? $"ButtonGroupItem{buttonType}HorizontalLast"
                    : $"ButtonGroupItem{buttonType}Default"
            : index == 0
                ? $"ButtonGroupItem{buttonType}VerticalFirst"
                : index == count - 1
                    ? $"ButtonGroupItem{buttonType}VerticalLast"
                    : $"ButtonGroupItem{buttonType}Default";
    }

    /// <summary>
    /// Tries to get a style from the dynamic resources.
    /// </summary>
    private static Style? TryGetStyle(string resourceKey)
    {
        if (StyleDict.TryGetValue(resourceKey, out var cachedStyle))
            return cachedStyle;

        // Try to get from application resources
        if (Application.Current?.Resources[resourceKey] is Style style)
        {
            StyleDict[resourceKey] = style;
            return style;
        }

        return null;
    }
}
