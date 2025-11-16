// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// 步骤容器控件 - 类似 Element Plus el-steps 的步骤条控件
/// Steps Container Control - Similar to Element Plus el-steps
/// </summary>
public class Steps : ItemsControl
{
    /// <summary>Identifies the <see cref="Orientation"/> dependency property.</summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(Steps),
        new PropertyMetadata(Orientation.Horizontal));

    /// <summary>Identifies the <see cref="CurrentStep"/> dependency property.</summary>
    public static readonly DependencyProperty CurrentStepProperty = DependencyProperty.Register(
        nameof(CurrentStep),
        typeof(int),
        typeof(Steps),
        new PropertyMetadata(0, OnCurrentStepChanged));

    /// <summary>Identifies the <see cref="IsLastStep"/> attached property.</summary>
    public static readonly DependencyProperty IsLastStepProperty = DependencyProperty.RegisterAttached(
        "IsLastStep",
        typeof(bool),
        typeof(Steps),
        new PropertyMetadata(false));

    /// <summary>Identifies the <see cref="StepChanged"/> routed event.</summary>
    public static readonly RoutedEvent StepChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(StepChanged),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(Steps));

    /// <summary>
    /// 步骤条方向 - Horizontal 或 Vertical
    /// Steps orientation - Horizontal or Vertical
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// 当前步骤索引（从 0 开始）
    /// Current step index (starting from 0)
    /// </summary>
    public int CurrentStep
    {
        get => (int)GetValue(CurrentStepProperty);
        set => SetValue(CurrentStepProperty, value);
    }

    /// <summary>
    /// 步骤切换事件
    /// Step changed event
    /// </summary>
    public event RoutedEventHandler StepChanged
    {
        add => AddHandler(StepChangedEvent, value);
        remove => RemoveHandler(StepChangedEvent, value);
    }

    /// <summary>
    /// 获取 IsLastStep 附加属性
    /// </summary>
    public static bool GetIsLastStep(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsLastStepProperty);
    }

    /// <summary>
    /// 设置 IsLastStep 附加属性
    /// </summary>
    public static void SetIsLastStep(DependencyObject obj, bool value)
    {
        obj.SetValue(IsLastStepProperty, value);
    }

    static Steps()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Steps), new FrameworkPropertyMetadata(typeof(Steps)));
        ItemsControl.ItemsSourceProperty.OverrideMetadata(typeof(Steps),
            new FrameworkPropertyMetadata(null, OnItemsSourceChanged));
    }

    public Steps()
    {
        AddHandler(Step.ClickedEvent, new RoutedEventHandler(OnStepClicked));
    }

    /// <summary>
    /// 处理 Step 项被点击事件
    /// Handle Step clicked event
    /// </summary>
    private void OnStepClicked(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is Step step)
        {
            int stepIndex = ItemContainerGenerator.IndexFromContainer(step);
            if (stepIndex >= 0)
            {
                SetCurrentValue(CurrentStepProperty, stepIndex);
            }
        }
    }

    protected override void AddChild(object value)
    {
        base.AddChild(value);
    }

    /// <summary>
    /// 当前步骤变化处理器
    /// Current step changed handler
    /// </summary>
    private static void OnCurrentStepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Steps steps)
        {
            int newStep = (int)e.NewValue;
            int oldStep = (int)e.OldValue;

            // 更新所有 Step 的状态和可点击性
            steps.UpdateStepStates(newStep);

            // 触发 StepChanged 事件
            if (newStep != oldStep)
            {
                steps.RaiseEvent(new RoutedEventArgs(StepChangedEvent, steps));
            }
        }
    }

    /// <summary>
    /// 项目源变化处理器
    /// Items source changed handler
    /// </summary>
    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Steps steps)
        {
            steps.UpdateStepStates(steps.CurrentStep);
            steps.MarkLastStep();
        }
    }

    /// <summary>
    /// 标记最后一个步骤
    /// Mark the last step
    /// </summary>
    private void MarkLastStep()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (ItemContainerGenerator.ContainerFromIndex(i) is Step step)
            {
                bool isLast = i == Items.Count - 1;
                SetIsLastStep(step, isLast);
            }
        }
    }

    /// <summary>
    /// 更新所有步骤的状态和可点击性
    /// Update all steps' status and clickability
    /// </summary>
    private void UpdateStepStates(int currentStep)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (ItemContainerGenerator.ContainerFromIndex(i) is Step step)
            {
                step.Index = i;

                // 设置步骤状态
                if (i < currentStep)
                {
                    step.Status = StepStatus.Finish;
                    step.IsClickable = true;
                }
                else if (i == currentStep)
                {
                    step.Status = StepStatus.Process;
                    step.IsClickable = false;
                }
                else
                {
                    step.Status = StepStatus.Wait;
                    step.IsClickable = true;
                }
            }
        }
    }

    /// <summary>
    /// 获取或创建步骤项容器
    /// Get or create step item container
    /// </summary>
    protected override DependencyObject GetContainerForItemOverride()
    {
        return new Step();
    }

    /// <summary>
    /// 判断是否应该使用容器
    /// Determine if item should use container
    /// </summary>
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is Step;
    }

    /// <summary>
    /// 准备项容器
    /// Prepare item container
    /// </summary>
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);

        if (element is Step step)
        {
            // 如果项本身是 Step，使用其属性；否则将项作为 Content
            if (!(item is Step))
            {
                step.Content = item;
            }
        }
    }
}
