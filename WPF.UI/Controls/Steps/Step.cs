// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// 步骤项控件 - Steps 容器中的单个步骤项
/// Step Item Control - Individual step item in Steps container
/// </summary>
public class Step : ContentControl
{
    /// <summary>Identifies the <see cref="Title"/> dependency property.</summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(Step),
        new PropertyMetadata(string.Empty));

    /// <summary>Identifies the <see cref="Description"/> dependency property.</summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(Step),
        new PropertyMetadata(string.Empty));

    /// <summary>Identifies the <see cref="Status"/> dependency property.</summary>
    public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
        nameof(Status),
        typeof(StepStatus),
        typeof(Step),
        new PropertyMetadata(StepStatus.Wait));

    /// <summary>Identifies the <see cref="Index"/> dependency property.</summary>
    public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(
        nameof(Index),
        typeof(int),
        typeof(Step),
        new PropertyMetadata(0));

    /// <summary>Identifies the <see cref="IsClickable"/> dependency property.</summary>
    public static readonly DependencyProperty IsClickableProperty = DependencyProperty.Register(
        nameof(IsClickable),
        typeof(bool),
        typeof(Step),
        new PropertyMetadata(true));

    /// <summary>Identifies the <see cref="Clicked"/> routed event.</summary>
    public static readonly RoutedEvent ClickedEvent = EventManager.RegisterRoutedEvent(
        nameof(Clicked),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(Step));

    /// <summary>
    /// 步骤标题
    /// Step title
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// 步骤描述
    /// Step description
    /// </summary>
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    /// <summary>
    /// 步骤状态
    /// Step status
    /// </summary>
    public StepStatus Status
    {
        get => (StepStatus)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    /// <summary>
    /// 步骤索引（从 0 开始）
    /// Step index (starting from 0)
    /// </summary>
    public int Index
    {
        get => (int)GetValue(IndexProperty);
        set => SetValue(IndexProperty, value);
    }

    /// <summary>
    /// 是否可点击（只有在 Wait 或 Finish 状态时，且在 Process 步骤之前的步骤才可点击）
    /// Whether the step is clickable
    /// </summary>
    public bool IsClickable
    {
        get => (bool)GetValue(IsClickableProperty);
        set => SetValue(IsClickableProperty, value);
    }

    /// <summary>
    /// 步骤被点击事件
    /// Step clicked event
    /// </summary>
    public event RoutedEventHandler Clicked
    {
        add => AddHandler(ClickedEvent, value);
        remove => RemoveHandler(ClickedEvent, value);
    }

    static Step()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Step), new FrameworkPropertyMetadata(typeof(Step)));
    }

    public Step()
    {
        AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnMouseLeftButtonDown), true);
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (IsClickable && IsEnabled)
        {
            RaiseEvent(new RoutedEventArgs(ClickedEvent, this));
            e.Handled = true;
        }
    }
}
