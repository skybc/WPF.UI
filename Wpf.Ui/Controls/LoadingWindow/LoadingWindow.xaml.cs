// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

/// <summary>
/// 美化的半透明加载窗口，用于显示程序加载状态.
/// </summary>
public partial class LoadingWindow : Window, INotifyPropertyChanged
{
    private string loadingMessage = "加载中...";
    private string subtitleMessage = "请稍候...";
    private bool isAnimating = false;

    /// <summary>
    /// Gets or sets the loading message text.
    /// </summary>
    public string LoadingMessage
    {
        get => this.loadingMessage;
        set
        {
            if (this.loadingMessage != value)
            {
                this.loadingMessage = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the subtitle message text.
    /// </summary>
    public string SubtitleMessage
    {
        get => this.subtitleMessage;
        set
        {
            if (this.subtitleMessage != value)
            {
                this.subtitleMessage = value;
                this.OnPropertyChanged();
                
                // 在 UI 线程上更新副标题
                this.Dispatcher.Invoke(() =>
                {
                    if (SubtitleText != null)
                    {
                        SubtitleText.Text = value;
                    }
                });
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether text pulse animation is enabled.
    /// </summary>
    public bool EnableTextPulse { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether ring pulse animation is enabled.
    /// </summary>
    public bool EnableRingPulse { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingWindow"/> class.
    /// </summary>
    public LoadingWindow()
    {
        InitializeComponent();
        DataContext = this;
        
        // 初始化窗口为不可见
        Opacity = 0;
        
        // 设置全屏尺寸
        SetFullScreenSize();
        
        // 订阅关闭事件
        Closing += OnClosing;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingWindow"/> class with a message.
    /// </summary>
    /// <param name="message">The loading message to display.</param>
    public LoadingWindow(string message) 
        : this()
    {
        LoadingMessage = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingWindow"/> class with messages.
    /// </summary>
    /// <param name="message">The main loading message to display.</param>
    /// <param name="subtitle">The subtitle message to display.</param>
    public LoadingWindow(string message, string subtitle) 
        : this(message)
    {
        SubtitleMessage = subtitle;
    }

    /// <summary>
    /// 显示加载窗口并播放美化动画.
    /// </summary>
    public void ShowWithAnimation()
    {
        if (this.isAnimating) return;
        
        Show();
        this.isAnimating = true;
        
        // 开始组合动画
        BeginShowAnimations();
    }

    /// <summary>
    /// 异步显示加载窗口并播放美化动画.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ShowWithAnimationAsync()
    {
        await Dispatcher.InvokeAsync(() => ShowWithAnimation());
    }

    /// <summary>
    /// 隐藏加载窗口并播放淡出动画.
    /// </summary>
    public void HideWithAnimation()
    {
        if (!this.isAnimating) return;
        
        var fadeOutStoryboard = (Storyboard)FindResource("LoadingFadeOutStoryboard");
        if (fadeOutStoryboard != null)
        {
            fadeOutStoryboard.Completed += (s, e) => 
            {
                Hide();
                this.isAnimating = false;
                StopPulseAnimations();
            };
            fadeOutStoryboard.Begin(this);
        }
        else
        {
            Hide();
            this.isAnimating = false;
            StopPulseAnimations();
        }
    }

    /// <summary>
    /// 异步隐藏加载窗口.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HideWithAnimationAsync()
    {
        await Dispatcher.InvokeAsync(() => HideWithAnimation());
    }

    /// <summary>
    /// 关闭加载窗口并播放淡出动画.
    /// </summary>
    public void CloseWithAnimation()
    {
        var fadeOutStoryboard = (Storyboard)FindResource("LoadingFadeOutStoryboard");
        if (fadeOutStoryboard != null)
        {
            fadeOutStoryboard.Completed += (s, e) => 
            {
                Close();
                this.isAnimating = false;
            };
            fadeOutStoryboard.Begin(this);
        }
        else
        {
            Close();
            this.isAnimating = false;
        }
    }

    /// <summary>
    /// 设置窗口的父窗口.
    /// </summary>
    /// <param name="owner">父窗口.</param>
    public void SetOwner(Window owner)
    {
        if (owner != null)
        {
            Owner = owner;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            SetOwnerScreenSize();
        }
    }

    /// <summary>
    /// 更新加载消息并触发轻微的强调动画.
    /// </summary>
    /// <param name="message">新的加载消息.</param>
    public void UpdateMessage(string message)
    {
        LoadingMessage = message;
        
        // 触发文本强调动画
        TriggerTextEmphasisAnimation();
    }

    /// <summary>
    /// 更新加载消息和副标题.
    /// </summary>
    /// <param name="message">主消息.</param>
    /// <param name="subtitle">副标题.</param>
    public void UpdateMessage(string message, string subtitle)
    {
        LoadingMessage = message;
        SubtitleMessage = subtitle;
        
        TriggerTextEmphasisAnimation();
    }

    private void SetFullScreenSize()
    {
        // 设置为全屏尺寸以提供遮罩效果
        //var screen = System.Windows.Forms.Screen.PrimaryScreen;
        //if (screen != null)
        //{
        //    this.SetCurrentValue(WidthProperty, (double)screen.Bounds.Width);
        //    this.SetCurrentValue(HeightProperty, (double)screen.Bounds.Height);
        //    this.SetCurrentValue(LeftProperty, (double)screen.Bounds.Left);
        //    this.SetCurrentValue(TopProperty, (double)screen.Bounds.Top);
        //}
    }

    private void SetOwnerScreenSize()
    {
        // 如果有父窗口，调整为父窗口大小
        if (this.Owner != null)
        {
            this.SetCurrentValue(WidthProperty, this.Owner.ActualWidth);
            this.SetCurrentValue(HeightProperty, this.Owner.ActualHeight);
            this.SetCurrentValue(LeftProperty, this.Owner.Left);
            this.SetCurrentValue(TopProperty, this.Owner.Top);
        }
    }

    private void BeginShowAnimations()
    {
        // 窗口淡入动画
        var fadeInStoryboard = (Storyboard)FindResource("LoadingFadeInStoryboard");
        fadeInStoryboard?.Begin(this);
        
        // 内容缩放动画
        var scaleInStoryboard = (Storyboard)FindResource("LoadingScaleInStoryboard");
        scaleInStoryboard?.Begin(ContentContainer);
        
        // 开始脉动动画
        StartPulseAnimations();
    }

    private void StartPulseAnimations()
    {
        if (EnableTextPulse)
        {
            var textPulseStoryboard = (Storyboard)FindResource("LoadingTextPulseStoryboard");
            textPulseStoryboard?.Begin(LoadingText);
        }
        
        if (EnableRingPulse)
        {
            var ringPulseStoryboard = (Storyboard)FindResource("LoadingRingPulseStoryboard");
            ringPulseStoryboard?.Begin(LoadingRing);
        }
    }

    private void StopPulseAnimations()
    {
        var textPulseStoryboard = (Storyboard)FindResource("LoadingTextPulseStoryboard");
        textPulseStoryboard?.Stop(LoadingText);
        
        var ringPulseStoryboard = (Storyboard)FindResource("LoadingRingPulseStoryboard");
        ringPulseStoryboard?.Stop(LoadingRing);
    }

    private void TriggerTextEmphasisAnimation()
    {
        // 创建临时的文本强调动画
        var emphasisAnimation = new DoubleAnimation
        {
            From = 1.0,
            To = 1.2,
            Duration = TimeSpan.FromMilliseconds(200),
            AutoReverse = true,
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };
        
        var scaleTransform = new System.Windows.Media.ScaleTransform();
        LoadingText.RenderTransform = scaleTransform;
        LoadingText.RenderTransformOrigin = new Point(0.5, 0.5);
        
        scaleTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, emphasisAnimation);
        scaleTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleYProperty, emphasisAnimation);
    }

    /// <summary>
    /// 处理窗口关闭事件.
    /// </summary>
    private void OnClosing(object? sender, CancelEventArgs e)
    {
        // 阻止用户直接关闭窗口，应该通过服务类来管理
        if (sender is LoadingWindow window && window.Opacity > 0)
        {
            e.Cancel = true;
            HideWithAnimation();
        }
    }

    /// <summary>
    /// PropertyChanged event.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}