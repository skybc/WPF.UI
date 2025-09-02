// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// 分页控件 - 提供数据分页导航功能
/// Pagination Control - Provides data pagination navigation functionality
/// </summary>
public class Pagination : ContentControl
{
    private static readonly DependencyPropertyKey TotalPagesPropertyKey =
       DependencyProperty.RegisterReadOnly(
           nameof(TotalPages),
           typeof(int),
           typeof(Pagination),
           new PropertyMetadata(1));

    public static readonly DependencyProperty TotalPagesProperty = TotalPagesPropertyKey.DependencyProperty;
    /// <summary>Identifies the <see cref="TotalCount"/> dependency property.</summary>
    public static readonly DependencyProperty TotalCountProperty =
        DependencyProperty.Register(
            nameof(TotalCount),
            typeof(int),
            typeof(Pagination),
            new PropertyMetadata(0, OnTotalCountChanged));

    /// <summary>Identifies the <see cref="PageSize"/> dependency property.</summary>
    public static readonly DependencyProperty PageSizeProperty =
        DependencyProperty.Register(
            nameof(PageSize),
            typeof(int),
            typeof(Pagination),
            new PropertyMetadata(20, OnPageSizeChanged));

    /// <summary>Identifies the <see cref="PageSizeOptions"/> dependency property.</summary>
    public static readonly DependencyProperty PageSizeOptionsProperty =
        DependencyProperty.Register(
            nameof(PageSizeOptions),
            typeof(IEnumerable<int>),
            typeof(Pagination),
            new PropertyMetadata(CreateDefaultPageSizeOptions(), OnPageSizeOptionsChanged));

    /// <summary>Identifies the <see cref="CurrentPage"/> dependency property.</summary>
    public static readonly DependencyProperty CurrentPageProperty =
        DependencyProperty.Register(
            nameof(CurrentPage),
            typeof(int),
            typeof(Pagination),
            new PropertyMetadata(1, OnCurrentPageChanged));

    /// <summary>Identifies the <see cref="TotalPages"/> dependency property.</summary>

    /// <summary>Identifies the <see cref="PageChanged"/> routed event.</summary>
    public static readonly RoutedEvent PageChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(PageChanged), RoutingStrategy.Bubble, typeof(PageChangedEventHandler), typeof(Pagination));



    private System.Windows.Controls.Button _firstPageButton;
    private System.Windows.Controls.Button _previousPageButton;
    private System.Windows.Controls.Button _nextPageButton;
    private System.Windows.Controls.Button _lastPageButton;
    private System.Windows.Controls.Button _jumpButton;
    private System.Windows.Controls.TextBlock _pageInfoTextBlock;
    private System.Windows.Controls.TextBox _pageInputTextBox;
    private System.Windows.Controls.ComboBox _pageSizeComboBox;
 
    /// <summary>
    /// 总记录数
    /// Total count of records
    /// </summary>
    public int TotalCount
    {
        get => (int)this.GetValue(TotalCountProperty);
        set => this.SetValue(TotalCountProperty, value);
    }

    /// <summary>
    /// 每页显示数量
    /// Page size - number of items per page
    /// </summary>
    public int PageSize
    {
        get => (int)this.GetValue(PageSizeProperty);
        set => this.SetValue(PageSizeProperty, value);
    }

    /// <summary>
    /// 页面大小选项列表
    /// Page size options list
    /// </summary>
    public IEnumerable<int> PageSizeOptions
    {
        get => (IEnumerable<int>)this.GetValue(PageSizeOptionsProperty);
        set => this.SetValue(PageSizeOptionsProperty, value);
    }

    /// <summary>
    /// 当前页码
    /// Current page number
    /// </summary>
    public int CurrentPage
    {
        get => (int)this.GetValue(CurrentPageProperty);
        set => this.SetValue(CurrentPageProperty, value);
    }

    /// <summary>
    /// 总页数（只读）
    /// Total pages (read-only)
    /// </summary>
    public int TotalPages
    {
        get => (int)this.GetValue(TotalPagesProperty);
        private set => this.SetValue(TotalPagesPropertyKey, value);
    }

    /// <summary>
    /// 页码变化事件处理器
    /// Page changed event handler
    /// </summary>
    public event PageChangedEventHandler PageChanged
    {
        add => this.AddHandler(PageChangedEvent, value);
        remove => this.RemoveHandler(PageChangedEvent, value);
    }

    /// <summary>
    /// 构造函数，初始化分页控件
    /// Constructor, initializes the pagination control
    /// </summary>
    public Pagination()
    {
        this.Loaded += PaginationControl_Loaded;
    }


    /// <summary>
    /// 应用控件模板时调用
    /// Called when applying control template
    /// </summary>
    public override void OnApplyTemplate()
    {
        // 移除旧的事件处理器
        if (_firstPageButton != null)
            _firstPageButton.Click -= FirstPageButton_Click;
        if (_previousPageButton != null)
            _previousPageButton.Click -= PreviousPageButton_Click;
        if (_nextPageButton != null)
            _nextPageButton.Click -= NextPageButton_Click;
        if (_lastPageButton != null)
            _lastPageButton.Click -= LastPageButton_Click;
        if (_jumpButton != null)
            _jumpButton.Click -= JumpButton_Click;
        if (_pageInputTextBox != null)
            _pageInputTextBox.KeyDown -= PageInputTextBox_KeyDown;
        if (_pageSizeComboBox != null)
            _pageSizeComboBox.SelectionChanged -= PageSizeComboBox_SelectionChanged;

        base.OnApplyTemplate();

        // 获取模板元素
        var btn = this.GetTemplateChild("PART_FirstPageButton");
        _firstPageButton = this.GetTemplateChild("PART_FirstPageButton") as System.Windows.Controls.Button;
        _previousPageButton = this.GetTemplateChild("PART_PreviousPageButton") as System.Windows.Controls.Button;
        _nextPageButton = this.GetTemplateChild("PART_NextPageButton") as System.Windows.Controls.Button;
        _lastPageButton = this.GetTemplateChild("PART_LastPageButton") as System.Windows.Controls.Button;
        _jumpButton = this.GetTemplateChild("PART_JumpButton") as System.Windows.Controls.Button;
        _pageInfoTextBlock = this.GetTemplateChild("PART_PageInfoTextBlock") as System.Windows.Controls.TextBlock;
        _pageInputTextBox = this.GetTemplateChild("PART_PageInputTextBox") as System.Windows.Controls.TextBox;
        _pageSizeComboBox = this.GetTemplateChild("PART_PageSizeComboBox") as System.Windows.Controls.ComboBox;

        // 添加新的事件处理器
        if (_firstPageButton != null)
            _firstPageButton.Click += FirstPageButton_Click;
        if (_previousPageButton != null)
            _previousPageButton.Click += PreviousPageButton_Click;
        if (_nextPageButton != null)
            _nextPageButton.Click += NextPageButton_Click;
        if (_lastPageButton != null)
            _lastPageButton.Click += LastPageButton_Click;
        if (_jumpButton != null)
            _jumpButton.Click += JumpButton_Click;
        if (_pageInputTextBox != null)
            _pageInputTextBox.KeyDown += PageInputTextBox_KeyDown;
        if (_pageSizeComboBox != null)
            _pageSizeComboBox.SelectionChanged += PageSizeComboBox_SelectionChanged;

        UpdateUI();
    }

    /// <summary>
    /// 创建默认页面大小选项，避免共享引用类型
    /// Create default page size options to avoid shared reference type
    /// </summary>
    private static IEnumerable<int> CreateDefaultPageSizeOptions()
    {
        return new int[] { 10, 20,50,100,200,500,1000 };
    }

    /// <summary>
    /// 控件加载完成事件处理
    /// Handle control loaded event
    /// </summary>
    private void PaginationControl_Loaded(object sender, RoutedEventArgs e)
    {
        UpdateUI();
    }

    /// <summary>
    /// 总记录数变化时的处理
    /// Handle total count changed
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnTotalCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination control)
        {
            control.UpdateTotalPages();
            control.ValidateCurrentPage();
            control.UpdateUI();
        }
    }

    /// <summary>
    /// 页面大小变化时的处理
    /// Handle page size changed
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination control)
        {
            control.UpdateTotalPages();
            control.ValidateCurrentPage();
            control.UpdateUI();
        }
    }

    /// <summary>
    /// 页面大小选项变化时的处理
    /// Handle page size options changed
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnPageSizeOptionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination control)
        {
            control.UpdateUI();
        }
    }

    /// <summary>
    /// 当前页码变化时的处理
    /// Handle current page changed
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination control)
        {
            var newPage = (int)e.NewValue;
            var oldPage = (int)e.OldValue;

            control.UpdateUI();

            if (newPage != oldPage)
            {
                // 触发页码变化事件
                control.RaisePageChangedEvent(newPage, oldPage);
            }
        }
    }

    /// <summary>
    /// 更新总页数
    /// Update total pages
    /// </summary>
    private void UpdateTotalPages()
    {
        if (PageSize <= 0)
        {
            TotalPages = 1;
            return;
        }

        TotalPages = Math.Max(1, (int)Math.Ceiling((double)TotalCount / PageSize));
    }

    /// <summary>
    /// 验证并修正当前页码
    /// Validate and correct current page
    /// </summary>
    private void ValidateCurrentPage()
    {
        var validPage = Math.Max(1, Math.Min(CurrentPage, TotalPages));
        if (validPage != CurrentPage)
        {
            this.SetCurrentValue(CurrentPageProperty, validPage);
        }
    }

    /// <summary>
    /// 更新UI界面
    /// Update UI interface
    /// </summary>
    private void UpdateUI()
    {
        if (!this.IsLoaded) return;

        // 更新页码信息显示
        if (_pageInfoTextBlock != null)
        {
            _pageInfoTextBlock.SetCurrentValue(TextBlock.TextProperty, $"第 {CurrentPage} 页 / 共 {TotalPages} 页 {TotalCount} 条");
        }

        // 更新输入框
        if (_pageInputTextBox != null)
        {
            _pageInputTextBox.SetCurrentValue(TextBox.TextProperty, CurrentPage.ToString());
        }

        // 更新按钮状态
        if (_firstPageButton != null)
            _firstPageButton.SetCurrentValue(IsEnabledProperty, CurrentPage > 1);

        if (_previousPageButton != null)
            _previousPageButton.SetCurrentValue(IsEnabledProperty, CurrentPage > 1);

        if (_nextPageButton != null)
            _nextPageButton.SetCurrentValue(IsEnabledProperty, CurrentPage < TotalPages);

        if (_lastPageButton != null)
            _lastPageButton.SetCurrentValue(IsEnabledProperty, CurrentPage < TotalPages);

        // 更新页面大小下拉框
        if (_pageSizeComboBox != null)
        {
            _pageSizeComboBox.SetCurrentValue(ItemsControl.ItemsSourceProperty, PageSizeOptions);
            _pageSizeComboBox.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, PageSize);
        }
    }

    /// <summary>
    /// 触发页码变化事件
    /// Raise page changed event
    /// </summary>
    private void RaisePageChangedEvent(int newPage, int oldPage)
    {
        var args = new PageChangedEventArgs(PageChangedEvent, newPage, oldPage);
        this.RaiseEvent(args);
    }

    /// <summary>
    /// 首页按钮点击
    /// First page button click
    /// </summary>
    private void FirstPageButton_Click(object sender, RoutedEventArgs e)
    {
        this.SetCurrentValue(CurrentPageProperty, 1);
    }

    /// <summary>
    /// 上一页按钮点击
    /// Previous page button click
    /// </summary>
    private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
    {
        if (CurrentPage > 1)
        {
            this.SetCurrentValue(CurrentPageProperty, CurrentPage - 1);
        }
    }

    /// <summary>
    /// 下一页按钮点击
    /// Next page button click
    /// </summary>
    private void NextPageButton_Click(object sender, RoutedEventArgs e)
    {
        if (CurrentPage < TotalPages)
        {
            this.SetCurrentValue(CurrentPageProperty, CurrentPage + 1);
        }
    }

    /// <summary>
    /// 尾页按钮点击
    /// Last page button click
    /// </summary>
    private void LastPageButton_Click(object sender, RoutedEventArgs e)
    {
        this.SetCurrentValue(CurrentPageProperty, TotalPages);
    }

    /// <summary>
    /// 跳转按钮点击
    /// Jump button click
    /// </summary>
    private void JumpButton_Click(object sender, RoutedEventArgs e)
    {
        JumpToPage();
    }

    /// <summary>
    /// 页码输入框回车键处理
    /// Handle Enter key in page input textbox
    /// </summary>
    private void PageInputTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            JumpToPage();
            e.Handled = true;
        }
    }

    /// <summary>
    /// 页面大小下拉框选择变化
    /// Handle page size combobox selection changed
    /// </summary>
    private void PageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedItem is int selectedPageSize)
        {
            this.SetCurrentValue(PageSizeProperty, selectedPageSize);
        }
    }

    /// <summary>
    /// 跳转到指定页码
    /// Jump to specified page
    /// </summary>
    private void JumpToPage()
    {
        if (_pageInputTextBox != null && int.TryParse(_pageInputTextBox.Text, out int targetPage))
        {
            this.SetCurrentValue(CurrentPageProperty, Math.Max(1, Math.Min(targetPage, TotalPages)));
        }
    }
}

/// <summary>
/// 页码变化事件参数
/// Page changed event arguments
/// </summary>
public class PageChangedEventArgs : RoutedEventArgs
{
    /// <summary>
    /// 新页码
    /// New page number
    /// </summary>
    public int NewPage { get; }

    /// <summary>
    /// 旧页码
    /// Old page number
    /// </summary>
    public int OldPage { get; }

    /// <summary>
    /// 构造函数
    /// Constructor
    /// </summary>
    public PageChangedEventArgs(RoutedEvent routedEvent, int newPage, int oldPage) : base(routedEvent)
    {
        NewPage = newPage;
        OldPage = oldPage;
    }
}

/// <summary>
/// 页码变化事件处理器委托
/// Page changed event handler delegate
/// </summary>
public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);