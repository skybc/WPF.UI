// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// A DataGrid control that displays data in rows and columns and allows
/// for the entering and editing of data.
/// </summary>
[StyleTypedProperty(Property = nameof(CheckBoxColumnElementStyle), StyleTargetType = typeof(CheckBox))]
[StyleTypedProperty(Property = nameof(CheckBoxColumnEditingElementStyle), StyleTargetType = typeof(CheckBox))]
[StyleTypedProperty(Property = nameof(ComboBoxColumnElementStyle), StyleTargetType = typeof(ComboBox))]
[StyleTypedProperty(Property = nameof(ComboBoxColumnEditingElementStyle), StyleTargetType = typeof(ComboBox))]
[StyleTypedProperty(Property = nameof(TextColumnElementStyle), StyleTargetType = typeof(TextBlock))]
[StyleTypedProperty(Property = nameof(TextColumnEditingElementStyle), StyleTargetType = typeof(TextBox))]
public class DataGrid : System.Windows.Controls.DataGrid
{
    private bool _isComboBoxSelecting = false;
    public DataGrid()
    {
        // 设置单击编辑功能
        this.PreparingCellForEdit += OnPreparingCellForEdit;
    }

    /// <summary>Identifies the <see cref="CheckBoxColumnElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty CheckBoxColumnElementStyleProperty =
        DependencyProperty.Register(
            nameof(CheckBoxColumnElementStyle),
            typeof(Style),
            typeof(DataGrid),
            new FrameworkPropertyMetadata(null)
        );

    /// <summary>Identifies the <see cref="CheckBoxColumnEditingElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty CheckBoxColumnEditingElementStyleProperty =
        DependencyProperty.Register(
            nameof(CheckBoxColumnEditingElementStyle),
            typeof(Style),
            typeof(DataGrid),
            new FrameworkPropertyMetadata(null)
        );

    /// <summary>Identifies the <see cref="ComboBoxColumnElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty ComboBoxColumnElementStyleProperty =
        DependencyProperty.Register(
            nameof(ComboBoxColumnElementStyle),
            typeof(Style),
            typeof(DataGrid),
            new FrameworkPropertyMetadata(null)
        );

    /// <summary>Identifies the <see cref="ComboBoxColumnEditingElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty ComboBoxColumnEditingElementStyleProperty =
        DependencyProperty.Register(
            nameof(ComboBoxColumnEditingElementStyle),
            typeof(Style),
            typeof(DataGrid),
            new FrameworkPropertyMetadata(null)
        );

    /// <summary>Identifies the <see cref="TextColumnElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty TextColumnElementStyleProperty = DependencyProperty.Register(
        nameof(TextColumnElementStyle),
        typeof(Style),
        typeof(DataGrid),
        new FrameworkPropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="TextColumnEditingElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty TextColumnEditingElementStyleProperty =
        DependencyProperty.Register(
            nameof(TextColumnEditingElementStyle),
            typeof(Style),
            typeof(DataGrid),
            new FrameworkPropertyMetadata(null)
        );

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnPreviewMouseLeftButtonDown(e);

        // 如果整个DataGrid是只读的，直接返回
        if (this.IsReadOnly)
            return;

        // 获取点击的单元格
        var hitTest = VisualTreeHelper.HitTest(this, e.GetPosition(this));
        if (hitTest?.VisualHit != null)
        {
            var cell = FindParent<DataGridCell>(hitTest.VisualHit);
            if (cell != null && !cell.IsReadOnly)
            {
                var row = FindParent<DataGridRow>(cell);

                // 检查是否是ComboBox列
                if (cell.Column is DataGridComboBoxColumn)
                {
                    // 如果正在操作ComboBox且点击的不是当前编辑的单元格，跳过处理
                    if (_isComboBoxSelecting && this.CurrentCell.IsValid)
                    {
                        var currentCell = GetCellFromCellInfo(this.CurrentCell);
                        if (currentCell != cell)
                        {
                            return;
                        }
                    }

                    // ComboBox列的正常编辑处理
                    if (row != null)
                    {
                        this.SelectedItem = row.DataContext;
                        this.CurrentCell = new DataGridCellInfo(cell);
                        this.BeginEdit();
                    }
                    return;
                }
                if (cell.Column is DataGridTemplateColumn templateColumn)
                {
                    // 检查编辑元素是否是ComboBox
                    if (row != null)
                    {
                        this.SelectedItem = row.DataContext;
                        this.CurrentCell = new DataGridCellInfo(cell);

                        // 开始编辑
                        this.BeginEdit();

                        // 延迟查找并打开ComboBox下拉列表
                        _ = this.Dispatcher.BeginInvoke(
                            new Action(() =>
                        {
                            var comboBox = FindComboBoxInCell(cell);
                            if (comboBox != null)
                            {
                                // 设置焦点并打开下拉列表
                                comboBox.Focus();
                                comboBox.IsDropDownOpen = true;

                                // 标记正在操作ComboBox
                                _isComboBoxSelecting = true;

                                // 添加事件处理器
                                comboBox.SelectionChanged -= ComboBox_SelectionChanged;
                                comboBox.DropDownClosed -= ComboBox_DropDownClosed;
                                comboBox.SelectionChanged += ComboBox_SelectionChanged;
                                comboBox.DropDownClosed += ComboBox_DropDownClosed;
                            }
                        }), System.Windows.Threading.DispatcherPriority.Background);
                    }
                    return;
                }
                // 非ComboBox列的正常编辑处理
                if (row != null)
                {
                    this.SelectedItem = row.DataContext;
                    this.CurrentCell = new DataGridCellInfo(cell);

                    // 开始编辑
                    this.BeginEdit();
                }
            }
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        // 如果整个DataGrid是只读的，对于非导航键直接返回
        if (this.IsReadOnly && !IsNavigationKey(e.Key))
        {
            base.OnKeyDown(e);
            return;
        }

        // 处理方向键和Tab键导航并自动进入编辑
        if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right ||
            e.Key == Key.Tab)
        {
            // 如果当前正在编辑，先提交编辑
            if (this.CurrentCell.IsValid)
            {
                this.CommitEdit();
            }

            // 调用基类处理导航
            base.OnKeyDown(e);

            // 延迟进入编辑状态，让导航完成
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (this.CurrentCell.IsValid && this.CurrentCell.Column != null && !this.IsReadOnly)
                {
                    var currentCellInfo = this.CurrentCell;
                    var column = currentCellInfo.Column;

                    // 检查列是否可编辑
                    if (!IsColumnReadOnly(column))
                    {
                        this.BeginEdit();
                    }
                }
            }), System.Windows.Threading.DispatcherPriority.Background);
        }
        else if (e.Key == Key.Enter)
        {
            // Enter键：提交当前编辑并移动到下一行同一列
            if (this.CurrentCell.IsValid)
            {
                this.CommitEdit();
            }

            // 移动到下一行
            if (this.CurrentCell.IsValid)
            {
                var currentRow = this.Items.IndexOf(this.CurrentCell.Item);
                if (currentRow >= 0 && currentRow < this.Items.Count - 1)
                {
                    var nextItem = this.Items[currentRow + 1];
                    var currentColumn = this.CurrentCell.Column;

                    this.CurrentCell = new DataGridCellInfo(nextItem, currentColumn);

                    // 延迟进入编辑状态
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!this.IsReadOnly && !IsColumnReadOnly(currentColumn))
                        {
                            this.BeginEdit();
                        }
                    }), System.Windows.Threading.DispatcherPriority.Background);
                }
            }

            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            // Escape键：取消当前编辑
            if (this.CurrentCell.IsValid)
            {
                this.CancelEdit();
            }
            e.Handled = true;
        }
        else
        {
            // 对于其他按键（如字符输入），检查是否应该自动进入编辑模式
            if (IsTextInputKey(e.Key))
            {
                if (this.CurrentCell.IsValid && this.CurrentCell.Column != null && !this.IsReadOnly)
                {
                    var column = this.CurrentCell.Column;
                    if (!IsColumnReadOnly(column))
                    {
                        // 开始编辑
                        this.BeginEdit();
                        e.Handled = false; // 让编辑控件处理这个按键
                        return;
                    }
                }
            }
            base.OnKeyDown(e);
        }
    }

    private bool IsColumnReadOnly(DataGridColumn column)
    {
        // 首先检查整个DataGrid是否为只读
        if (this.IsReadOnly)
            return true;

        // 检查列是否为只读
        if (column is DataGridBoundColumn boundColumn)
        {
            return boundColumn.IsReadOnly;
        }

        // 对于其他类型的列，默认为可编辑
        return false;
    }

    private bool IsNavigationKey(Key key)
    {
        // 检查是否为导航按键
        return key == Key.Up || key == Key.Down || key == Key.Left || key == Key.Right ||
               key == Key.Tab || key == Key.Enter || key == Key.Escape ||
               key == Key.Home || key == Key.End || key == Key.PageUp || key == Key.PageDown;
    }

    private bool IsTextInputKey(Key key)
    {
        // 检查是否为文本输入按键
        return (key >= Key.A && key <= Key.Z) ||
               (key >= Key.D0 && key <= Key.D9) ||
               (key >= Key.NumPad0 && key <= Key.NumPad9) ||
               key == Key.Space ||
               key == Key.OemPeriod ||
               key == Key.OemComma ||
               key == Key.OemMinus ||
               key == Key.OemPlus ||
               key == Key.Decimal ||
               key == Key.Add ||
               key == Key.Subtract ||
               key == Key.Multiply ||
               key == Key.Divide ||
               key == Key.Back ||
               key == Key.Delete;
    }

    private bool IsComboBoxRelatedElement(DependencyObject element)
    {
        // 向上遍历视觉树，检查是否在ComboBox或其下拉列表中
        var current = element;
        while (current != null)
        {
            // 直接检查ComboBox和ComboBoxItem
            if (current is ComboBox || current is ComboBoxItem)
            {
                return true;
            }

            // 检查是否是ComboBox的Popup部分
            if (current.GetType().Name == "Popup")
            {
                // 进一步确认这个Popup是否属于ComboBox
                var parent = VisualTreeHelper.GetParent(current);
                while (parent != null)
                {
                    if (parent is ComboBox)
                        return true;
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }

            // 如果到达DataGridCell，停止向上搜索
            if (current is DataGridCell)
            {
                break;
            }

            current = VisualTreeHelper.GetParent(current);
        }
        return false;
    }

    private bool IsComboBoxColumnInEditMode(DataGridCell cell)
    {
        // 检查单元格是否属于ComboBox列且正在编辑模式
        if (cell.Column is DataGridComboBoxColumn)
        {
            // 查找单元格中是否有ComboBox控件
            return FindComboBoxInCell(cell) != null;
        }
        return false;
    }

    private ComboBox FindComboBoxInCell(DataGridCell cell)
    {
        // 在单元格中查找ComboBox控件
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(cell); i++)
        {
            var child = VisualTreeHelper.GetChild(cell, i);
            if (child is ComboBox comboBox)
            {
                return comboBox;
            }

            // 递归查找
            var found = FindComboBoxRecursive(child);
            if (found != null)
                return found;
        }
        return null;
    }

    private ComboBox FindComboBoxRecursive(DependencyObject parent)
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is ComboBox comboBox)
            {
                return comboBox;
            }

            var found = FindComboBoxRecursive(child);
            if (found != null)
                return found;
        }
        return null;
    }

    private DataGridCell GetCellFromCellInfo(DataGridCellInfo cellInfo)
    {
        if (!cellInfo.IsValid) return null;

        var row = this.ItemContainerGenerator.ContainerFromItem(cellInfo.Item) as DataGridRow;
        if (row == null) return null;

        var cellsPresenter = FindVisualChild<DataGridCellsPresenter>(row);
        if (cellsPresenter == null) return null;

        var cellPresenter = cellsPresenter.ItemContainerGenerator.ContainerFromIndex(cellInfo.Column.DisplayIndex) as DataGridCell;
        return cellPresenter;
    }

    private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T)
                return (T)child;

            var childOfChild = FindVisualChild<T>(child);
            if (childOfChild != null)
                return childOfChild;
        }
        return null;
    }

    private static T FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        var parentObject = VisualTreeHelper.GetParent(child);

        if (parentObject == null)
            return null;

        if (parentObject is T parent)
            return parent;

        return FindParent<T>(parentObject);
    }

    private void OnPreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
        // 确保单元格获得焦点
        if (e.EditingElement != null)
        {
            e.EditingElement.Focus();

            // 如果是ComboBox，添加选择完成事件处理
            if (e.EditingElement is ComboBox comboBox)
            {
                // 移除之前的事件处理器（如果有）
                comboBox.SelectionChanged -= ComboBox_SelectionChanged;
                comboBox.DropDownClosed -= ComboBox_DropDownClosed;
                _isComboBoxSelecting = true;

                // 添加新的事件处理器
                comboBox.SelectionChanged += ComboBox_SelectionChanged;
                comboBox.DropDownClosed += ComboBox_DropDownClosed;

                // 自动打开下拉列表
                comboBox.IsDropDownOpen = true;
            }
            else
            {
                // 如果编辑元素不是直接的ComboBox，在子元素中查找
                var foundComboBox = FindComboBoxRecursive(e.EditingElement);
                if (foundComboBox != null)
                {
                    // 移除之前的事件处理器（如果有）
                    foundComboBox.SelectionChanged -= ComboBox_SelectionChanged;
                    foundComboBox.DropDownClosed -= ComboBox_DropDownClosed;
                    _isComboBoxSelecting = true;

                    // 添加新的事件处理器
                    foundComboBox.SelectionChanged += ComboBox_SelectionChanged;
                    foundComboBox.DropDownClosed += ComboBox_DropDownClosed;

                    // 设置焦点并自动打开下拉列表
                    foundComboBox.Focus();
                    foundComboBox.IsDropDownOpen = true;
                }
            }
        }
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // ComboBox选择改变时，不需要特殊处理
        // 让下拉框正常工作
    }

    private void ComboBox_DropDownClosed(object sender, EventArgs e)
    {
        // 下拉框关闭时，短暂阻止其他单元格编辑
        if (sender is ComboBox comboBox)
        {

            // 100ms后重置标记，这样可以防止意外的单元格编辑
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                _isComboBoxSelecting = false;
            }), System.Windows.Threading.DispatcherPriority.Background);

            // 延迟50ms后再重置，确保有足够时间阻止意外编辑
            Task.Delay(100).ContinueWith(_ =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    _isComboBoxSelecting = false;
                });
            });
        }
    }

    /// <summary>
    /// Gets or sets the style which is applied to all checkbox column in the DataGrid
    /// </summary>
    public Style? CheckBoxColumnElementStyle
    {
        get => (Style?)GetValue(CheckBoxColumnElementStyleProperty);
        set => SetValue(CheckBoxColumnElementStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the style for all the column checkboxes in the DataGrid
    /// </summary>
    public Style? CheckBoxColumnEditingElementStyle
    {
        get => (Style?)GetValue(CheckBoxColumnEditingElementStyleProperty);
        set => SetValue(CheckBoxColumnEditingElementStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the style which is applied to all combobox column in the DataGrid
    /// </summary>
    public Style? ComboBoxColumnElementStyle
    {
        get => (Style?)GetValue(ComboBoxColumnElementStyleProperty);
        set => SetValue(ComboBoxColumnElementStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the style for all the column comboboxes in the DataGrid
    /// </summary>
    public Style? ComboBoxColumnEditingElementStyle
    {
        get => (Style?)GetValue(ComboBoxColumnEditingElementStyleProperty);
        set => SetValue(ComboBoxColumnEditingElementStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the style which is applied to all textbox column in the DataGrid
    /// </summary>
    public Style? TextColumnElementStyle
    {
        get => (Style?)GetValue(TextColumnElementStyleProperty);
        set => SetValue(TextColumnElementStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the style for all the column textboxes in the DataGrid
    /// </summary>
    public Style? TextColumnEditingElementStyle
    {
        get => (Style?)GetValue(TextColumnEditingElementStyleProperty);
        set => SetValue(TextColumnEditingElementStyleProperty, value);
    }

    protected override void OnInitialized(EventArgs e)
    {
        Columns.CollectionChanged += ColumnsOnCollectionChanged;

        UpdateColumnElementStyles();

        base.OnInitialized(e);
    }

    private void ColumnsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateColumnElementStyles();
    }

    private void UpdateColumnElementStyles()
    {
        foreach (DataGridColumn singleColumn in Columns)
        {
            UpdateSingleColumn(singleColumn);
        }
    }

    private void UpdateSingleColumn(DataGridColumn dataGridColumn)
    {
        switch (dataGridColumn)
        {
            case DataGridCheckBoxColumn checkBoxColumn:
                if (
                    checkBoxColumn.ReadLocalValue(DataGridBoundColumn.ElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        checkBoxColumn,
                        DataGridBoundColumn.ElementStyleProperty,
                        new Binding
                        {
                            Path = new PropertyPath(CheckBoxColumnElementStyleProperty),
                            Source = this,
                        }
                    );
                }

                if (
                    checkBoxColumn.ReadLocalValue(DataGridBoundColumn.EditingElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        checkBoxColumn,
                        DataGridBoundColumn.EditingElementStyleProperty,
                        new Binding
                        {
                            Path = new PropertyPath(CheckBoxColumnEditingElementStyleProperty),
                            Source = this,
                        }
                    );
                }

                break;

            case DataGridComboBoxColumn comboBoxColumn:
                if (
                    comboBoxColumn.ReadLocalValue(DataGridBoundColumn.ElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        comboBoxColumn,
                        DataGridBoundColumn.ElementStyleProperty,
                        new Binding
                        {
                            Path = new PropertyPath(ComboBoxColumnElementStyleProperty),
                            Source = this,
                        }
                    );
                }

                if (
                    comboBoxColumn.ReadLocalValue(DataGridBoundColumn.EditingElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        comboBoxColumn,
                        DataGridBoundColumn.EditingElementStyleProperty,
                        new Binding
                        {
                            Path = new PropertyPath(ComboBoxColumnEditingElementStyleProperty),
                            Source = this,
                        }
                    );
                }

                if (
                    comboBoxColumn.ReadLocalValue(DataGridBoundColumn.EditingElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        comboBoxColumn,
                        DataGridBoundColumn.EditingElementStyleProperty,
                        new Binding
                        {
                            Path = new PropertyPath(ComboBoxColumnEditingElementStyleProperty),
                            Source = this,
                        }
                    );
                }

                break;

            case DataGridTextColumn textBoxColumn:
                if (
                    textBoxColumn.ReadLocalValue(DataGridBoundColumn.ElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        textBoxColumn,
                        DataGridBoundColumn.ElementStyleProperty,
                        new Binding { Path = new PropertyPath(TextColumnElementStyleProperty), Source = this }
                    );
                }

                if (
                    textBoxColumn.ReadLocalValue(DataGridBoundColumn.EditingElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        textBoxColumn,
                        DataGridBoundColumn.EditingElementStyleProperty,
                        new Binding
                        {
                            Path = new PropertyPath(TextColumnEditingElementStyleProperty),
                            Source = this,
                        }
                    );
                }

                break;
        }
    }
}
