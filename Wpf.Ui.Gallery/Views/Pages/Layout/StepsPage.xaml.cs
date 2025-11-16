// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;

namespace Wpf.Ui.Gallery.Views.Pages.Layout;

/// <summary>
/// Interaction logic for StepsPage.xaml
/// </summary>
[GalleryPage("Steps control - Similar to Element Plus el-steps.", SymbolRegular.NumberSymbol24)]
public partial class StepsPage : Page
{
    public StepsPage()
    {
        InitializeComponent();
    }

    private void OnHorizontalStep1Click(object sender, RoutedEventArgs e)
    {
        HorizontalSteps.CurrentStep = 0;
        UpdateHorizontalStatus(0);
    }

    private void OnHorizontalStep2Click(object sender, RoutedEventArgs e)
    {
        HorizontalSteps.CurrentStep = 1;
        UpdateHorizontalStatus(1);
    }

    private void OnHorizontalStep3Click(object sender, RoutedEventArgs e)
    {
        HorizontalSteps.CurrentStep = 2;
        UpdateHorizontalStatus(2);
    }

    private void OnVerticalStep1Click(object sender, RoutedEventArgs e)
    {
        VerticalSteps.CurrentStep = 0;
        UpdateVerticalStatus(0);
    }

    private void OnVerticalStep2Click(object sender, RoutedEventArgs e)
    {
        VerticalSteps.CurrentStep = 1;
        UpdateVerticalStatus(1);
    }

    private void OnVerticalStep3Click(object sender, RoutedEventArgs e)
    {
        VerticalSteps.CurrentStep = 2;
        UpdateVerticalStatus(2);
    }

    private void OnVerticalStep4Click(object sender, RoutedEventArgs e)
    {
        VerticalSteps.CurrentStep = 3;
        UpdateVerticalStatus(3);
    }

    private void OnStartDemoClick(object sender, RoutedEventArgs e)
    {
        // 启动演示动画
        var button = (System.Windows.Controls.Button)sender;
        button.IsEnabled = false;

        int step = 0;
        var timer = new System.Windows.Threading.DispatcherTimer();
        timer.Interval = System.TimeSpan.FromSeconds(1);
        timer.Tick += (s, args) =>
        {
            if (step < 3)
            {
                DynamicSteps.CurrentStep = step;
                UpdateDemoStatus(step);
                step++;
            }
            else
            {
                timer.Stop();
                button.IsEnabled = true;
                step = 0;
            }
        };
        timer.Start();
    }

    private void UpdateHorizontalStatus(int currentStep)
    {
        // 在这里可以更新 UI 显示当前步骤
        // 例如：可以在 Footer 或其他地方显示状态
    }

    private void UpdateVerticalStatus(int currentStep)
    {
        // 在这里可以更新 UI 显示当前步骤
    }

    private void UpdateDemoStatus(int currentStep)
    {
        var steps = new[] { "开始", "进行中", "完成" };
        if (currentStep >= 0 && currentStep < steps.Length)
        {
            DemoStatus.Text = $"当前步骤: {currentStep} ({steps[currentStep]})";
        }
    }
}
