// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Services;

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui.Controls;

/// <summary>
/// 加载窗口服务接口.
/// </summary>
public interface ILoadingService
{
    /// <summary>
    /// 显示加载窗口.
    /// </summary>
    /// <param name="message">加载消息.</param>
    void Show(string message = "加载中...");

    /// <summary>
    /// 异步显示加载窗口.
    /// </summary>
    /// <param name="message">加载消息.</param>
    /// <returns>任务.</returns>
    Task ShowAsync(string message = "加载中...");

    /// <summary>
    /// 隐藏加载窗口.
    /// </summary>
    void Hide();

    /// <summary>
    /// 异步隐藏加载窗口.
    /// </summary>
    /// <returns>任务.</returns>
    Task HideAsync();

    /// <summary>
    /// 更新加载消息.
    /// </summary>
    /// <param name="message">新的加载消息.</param>
    void UpdateMessage(string message);

    /// <summary>
    /// 更新加载消息和副标题.
    /// </summary>
    /// <param name="message">主消息.</param>
    /// <param name="subtitle">副标题.</param>
    void UpdateMessage(string message, string subtitle);

    /// <summary>
    /// 显示带副标题的加载窗口.
    /// </summary>
    /// <param name="message">主消息.</param>
    /// <param name="subtitle">副标题.</param>
    void Show(string message, string subtitle);

    /// <summary>
    /// 异步显示带副标题的加载窗口.
    /// </summary>
    /// <param name="message">主消息.</param>
    /// <param name="subtitle">副标题.</param>
    /// <returns>任务.</returns>
    Task ShowAsync(string message, string subtitle);

    /// <summary>
    /// 设置父窗口.
    /// </summary>
    /// <param name="owner">父窗口.</param>
    void SetOwner(Window owner);

    /// <summary>
    /// Gets a value indicating whether loading window is currently showing.
    /// </summary>
    bool IsShowing { get; }
}

/// <summary>
/// 加载窗口服务实现.
/// </summary>
public class LoadingService : ILoadingService
{
    private LoadingWindow loadingWindow;
    private Window owner;

    /// <summary>
    /// Gets a value indicating whether loading window is currently showing.
    /// </summary>
    public bool IsShowing => this.loadingWindow?.IsVisible == true;

    /// <summary>
    /// 显示加载窗口.
    /// </summary>
    /// <param name="message">加载消息.</param>
    public void Show(string message = "加载中...")
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            this.EnsureLoadingWindow();
            
            if (this.loadingWindow != null)
            {
                this.loadingWindow.LoadingMessage = message;
                
                if (this.owner != null)
                {
                    this.loadingWindow.SetOwner(this.owner);
                }
                
                this.loadingWindow.ShowWithAnimation();
            }
        });
    }

    /// <summary>
    /// 显示带副标题的加载窗口.
    /// </summary>
    /// <param name="message">主消息.</param>
    /// <param name="subtitle">副标题.</param>
    public void Show(string message, string subtitle)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            this.EnsureLoadingWindow();
            
            if (this.loadingWindow != null)
            {
                this.loadingWindow.LoadingMessage = message;
                this.loadingWindow.SubtitleMessage = subtitle;
                
                if (this.owner != null)
                {
                    this.loadingWindow.SetOwner(this.owner);
                }
                
                this.loadingWindow.ShowWithAnimation();
            }
        });
    }

    /// <summary>
    /// 异步显示加载窗口.
    /// </summary>
    /// <param name="message">加载消息.</param>
    /// <returns>任务.</returns>
    public async Task ShowAsync(string message = "加载中...")
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            this.Show(message);
        });
    }

    /// <summary>
    /// 异步显示带副标题的加载窗口.
    /// </summary>
    /// <param name="message">主消息.</param>
    /// <param name="subtitle">副标题.</param>
    /// <returns>任务.</returns>
    public async Task ShowAsync(string message, string subtitle)
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            this.Show(message, subtitle);
        });
    }

    /// <summary>
    /// 隐藏加载窗口.
    /// </summary>
    public void Hide()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            this.loadingWindow?.HideWithAnimation();
        });
    }

    /// <summary>
    /// 异步隐藏加载窗口.
    /// </summary>
    /// <returns>任务.</returns>
    public async Task HideAsync()
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            this.Hide();
        });
    }

    /// <summary>
    /// 更新加载消息.
    /// </summary>
    /// <param name="message">新的加载消息.</param>
    public void UpdateMessage(string message)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (this.loadingWindow != null)
            {
                this.loadingWindow.UpdateMessage(message);
            }
        });
    }

    /// <summary>
    /// 更新加载消息和副标题.
    /// </summary>
    /// <param name="message">主消息.</param>
    /// <param name="subtitle">副标题.</param>
    public void UpdateMessage(string message, string subtitle)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (this.loadingWindow != null)
            {
                this.loadingWindow.UpdateMessage(message, subtitle);
            }
        });
    }

    /// <summary>
    /// 设置父窗口.
    /// </summary>
    /// <param name="owner">父窗口.</param>
    public void SetOwner(Window owner)
    {
        this.owner = owner;
        
        if (this.loadingWindow != null && owner != null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.loadingWindow.SetOwner(owner);
            });
        }
    }

    /// <summary>
    /// 释放资源.
    /// </summary>
    public void Dispose()
    {
        this.loadingWindow?.Close();
        this.loadingWindow = null;
    }

    /// <summary>
    /// 确保加载窗口已创建.
    /// </summary>
    private void EnsureLoadingWindow()
    {
        if (this.loadingWindow == null)
        {
            this.loadingWindow = new LoadingWindow();
        }
    }
}

/// <summary>
/// 加载窗口静态服务.
/// </summary>
public static class LoadingHelper
{
    private static readonly Lazy<ILoadingService> LazyInstance = new Lazy<ILoadingService>(() => new LoadingService());

    /// <summary>
    /// Gets default loading service instance.
    /// </summary>
    public static ILoadingService Default => LazyInstance.Value;

    /// <summary>
    /// Gets a value indicating whether loading window is currently showing.
    /// </summary>
    public static bool IsShowing => Default.IsShowing;

    /// <summary>
    /// 显示加载窗口.
    /// </summary>
    /// <param name="message">加载消息.</param>
    public static void Show(string message = "加载中...")
    {
        Default.Show(message);
    }

    /// <summary>
    /// 显示带副标题的加载窗口.
    /// </summary>
    /// <param name="message">主消息.</param>
    /// <param name="subtitle">副标题.</param>
    public static void Show(string message, string subtitle)
    {
        Default.Show(message, subtitle);
    }

    /// <summary>
    /// 异步显示加载窗口.
    /// </summary>
    /// <param name="message">加载消息.</param>
    /// <returns>任务.</returns>
    public static async Task ShowAsync(string message = "加载中...")
    {
        await Default.ShowAsync(message);
    }

    /// <summary>
    /// 异步显示带副标题的加载窗口.
    /// </summary>
    /// <param name="message">主消息.</param>
    /// <param name="subtitle">副标题.</param>
    /// <returns>任务.</returns>
    public static async Task ShowAsync(string message, string subtitle)
    {
        await Default.ShowAsync(message, subtitle);
    }

    /// <summary>
    /// 隐藏加载窗口.
    /// </summary>
    public static void Hide()
    {
        Default.Hide();
    }

    /// <summary>
    /// 异步隐藏加载窗口.
    /// </summary>
    /// <returns>任务.</returns>
    public static async Task HideAsync()
    {
        await Default.HideAsync();
    }

    /// <summary>
    /// 更新加载消息.
    /// </summary>
    /// <param name="message">新的加载消息.</param>
    public static void UpdateMessage(string message)
    {
        Default.UpdateMessage(message);
    }

    /// <summary>
    /// 更新加载消息和副标题.
    /// </summary>
    /// <param name="message">主消息.</param>
    /// <param name="subtitle">副标题.</param>
    public static void UpdateMessage(string message, string subtitle)
    {
        Default.UpdateMessage(message, subtitle);
    }

    /// <summary>
    /// 设置父窗口.
    /// </summary>
    /// <param name="owner">父窗口.</param>
    public static void SetOwner(Window owner)
    {
        Default.SetOwner(owner);
    }

    /// <summary>
    /// 执行异步操作并显示加载窗口.
    /// </summary>
    /// <param name="asyncOperation">异步操作.</param>
    /// <param name="message">加载消息.</param>
    /// <returns>任务.</returns>
    public static async Task ExecuteWithLoadingAsync(Func<Task> asyncOperation, string message = "加载中...")
    {
        try
        {
            await ShowAsync(message);
            await asyncOperation();
        }
        finally
        {
            await HideAsync();
        }
    }

    /// <summary>
    /// 执行异步操作并显示加载窗口.
    /// </summary>
    /// <typeparam name="T">返回类型.</typeparam>
    /// <param name="asyncOperation">异步操作.</param>
    /// <param name="message">加载消息.</param>
    /// <returns>任务结果.</returns>
    public static async Task<T> ExecuteWithLoadingAsync<T>(Func<Task<T>> asyncOperation, string message = "加载中...")
    {
        try
        {
            await ShowAsync(message);
            return await asyncOperation();
        }
        finally
        {
            await HideAsync();
        }
    }
}