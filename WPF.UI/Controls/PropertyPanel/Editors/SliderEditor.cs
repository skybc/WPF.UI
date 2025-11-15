// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor for slider properties.
/// </summary>
internal class SliderEditor : PropertyEditorBase
{
    public override PropertyEditorKind EditorKind => PropertyEditorKind.Slider;

    public override FrameworkElement CreateEditor(PropertyItem item, PropertyPanel panel)
    {
        var stackPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var slider = new Slider
        {
            MinWidth = 120,
            IsSnapToTickEnabled = true,
            Minimum = item.Min,
            Maximum = item.Max,
            TickFrequency = item.Step,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var sliderBinding = CreateValueBinding(item);
        slider.SetBinding(RangeBase.ValueProperty, sliderBinding);

        var textBlock = new TextBlock
        {
            Margin = new Thickness(8, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
        };

        var textBinding = new System.Windows.Data.Binding(nameof(PropertyItem.CurrentValue))
        {
            Source = item,
        };

        textBlock.SetBinding(TextBlock.TextProperty, textBinding);

        stackPanel.Children.Add(slider);
        stackPanel.Children.Add(textBlock);

        SetCommonProperties(stackPanel, item);

        return stackPanel;
    }
}
