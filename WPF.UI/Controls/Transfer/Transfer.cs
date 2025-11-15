// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Transfer control for moving items between two lists.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:Transfer
///     LeftItemsSource="{Binding AvailableItems}"
///     RightItemsSource="{Binding SelectedItems}"
///     TransferMode="Multiple"
///     IsSearchEnabled="True" /&gt;
/// </code>
/// </example>
[TemplatePart(Name = LeftListBoxPartName, Type = typeof(ListBox))]
[TemplatePart(Name = RightListBoxPartName, Type = typeof(ListBox))]
[TemplatePart(Name = MoveRightButtonPartName, Type = typeof(Button))]
[TemplatePart(Name = MoveLeftButtonPartName, Type = typeof(Button))]
[TemplatePart(Name = MoveRightAllButtonPartName, Type = typeof(Button))]
[TemplatePart(Name = MoveLeftAllButtonPartName, Type = typeof(Button))]
public class Transfer : Control
{
    private const string LeftListBoxPartName = "PART_LeftListBox";
    private const string RightListBoxPartName = "PART_RightListBox";
    private const string MoveRightButtonPartName = "PART_MoveRightButton";
    private const string MoveLeftButtonPartName = "PART_MoveLeftButton";
    private const string MoveRightAllButtonPartName = "PART_MoveRightAllButton";
    private const string MoveLeftAllButtonPartName = "PART_MoveLeftAllButton";
    private const string DragItemsFormat = "TransferItems";
    private const string DragSourceFormat = "TransferSource";

    private ListBox? _leftListBox;
    private ListBox? _rightListBox;
    private Button? _moveRightButton;
    private Button? _moveLeftButton;
    private Button? _moveRightAllButton;
    private Button? _moveLeftAllButton;

    private INotifyCollectionChanged? _leftItemsCollection;
    private INotifyCollectionChanged? _rightItemsCollection;
    private ListBox? _dragSourceListBox;
    private ListBoxItem? _draggedItem;
    private Point _dragStartPoint;
    /// <summary>Identifies the <see cref="LeftItemsSource"/> dependency property.</summary>
    public static readonly DependencyProperty LeftItemsSourceProperty = DependencyProperty.Register(
        nameof(LeftItemsSource),
        typeof(IEnumerable),
        typeof(Transfer),
        new PropertyMetadata(null, OnLeftItemsSourceChanged)
    );

    /// <summary>Identifies the <see cref="RightItemsSource"/> dependency property.</summary>
    public static readonly DependencyProperty RightItemsSourceProperty = DependencyProperty.Register(
        nameof(RightItemsSource),
        typeof(IEnumerable),
        typeof(Transfer),
        new PropertyMetadata(null, OnRightItemsSourceChanged)
    );

    /// <summary>Identifies the <see cref="ItemTemplate"/> dependency property.</summary>
    public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
        nameof(ItemTemplate),
        typeof(DataTemplate),
        typeof(Transfer),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="SelectedLeftItems"/> dependency property.</summary>
    public static readonly DependencyProperty SelectedLeftItemsProperty = DependencyProperty.Register(
        nameof(SelectedLeftItems),
        typeof(IList),
        typeof(Transfer),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="SelectedRightItems"/> dependency property.</summary>
    public static readonly DependencyProperty SelectedRightItemsProperty = DependencyProperty.Register(
        nameof(SelectedRightItems),
        typeof(IList),
        typeof(Transfer),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="TransferMode"/> dependency property.</summary>
    public static readonly DependencyProperty TransferModeProperty = DependencyProperty.Register(
        nameof(TransferMode),
        typeof(TransferMode),
        typeof(Transfer),
        new PropertyMetadata(TransferMode.Multiple, OnTransferModeChanged)
    );

    /// <summary>Identifies the <see cref="IsSearchEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty IsSearchEnabledProperty = DependencyProperty.Register(
        nameof(IsSearchEnabled),
        typeof(bool),
        typeof(Transfer),
        new PropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="LeftListBoxHeight"/> dependency property.</summary>
    public static readonly DependencyProperty LeftListBoxHeightProperty = DependencyProperty.Register(
        nameof(LeftListBoxHeight),
        typeof(double),
        typeof(Transfer),
        new PropertyMetadata(300d)
    );

    /// <summary>Identifies the <see cref="RightListBoxHeight"/> dependency property.</summary>
    public static readonly DependencyProperty RightListBoxHeightProperty = DependencyProperty.Register(
        nameof(RightListBoxHeight),
        typeof(double),
        typeof(Transfer),
        new PropertyMetadata(300d)
    );

    /// <summary>Identifies the <see cref="ButtonForeground"/> dependency property.</summary>
    public static readonly DependencyProperty ButtonForegroundProperty = DependencyProperty.Register(
        nameof(ButtonForeground),
        typeof(Brush),
        typeof(Transfer),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="ButtonBackground"/> dependency property.</summary>
    public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register(
        nameof(ButtonBackground),
        typeof(Brush),
        typeof(Transfer),
        new PropertyMetadata(null)
    );

    static Transfer()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(Transfer),
            new FrameworkPropertyMetadata(typeof(Transfer))
        );
    }

    /// <summary>
    /// Gets or sets the left side items source.
    /// </summary>
    public IEnumerable? LeftItemsSource
    {
        get => (IEnumerable?)GetValue(LeftItemsSourceProperty);
        set => SetValue(LeftItemsSourceProperty, value);
    }

    /// <summary>
    /// Gets or sets the right side items source.
    /// </summary>
    public IEnumerable? RightItemsSource
    {
        get => (IEnumerable?)GetValue(RightItemsSourceProperty);
        set => SetValue(RightItemsSourceProperty, value);
    }

    /// <summary>
    /// Gets or sets the item template for custom item display.
    /// </summary>
    public DataTemplate? ItemTemplate
    {
        get => (DataTemplate?)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected items on the left side.
    /// </summary>
    public IList? SelectedLeftItems
    {
        get => (IList?)GetValue(SelectedLeftItemsProperty);
        set => SetValue(SelectedLeftItemsProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected items on the right side.
    /// </summary>
    public IList? SelectedRightItems
    {
        get => (IList?)GetValue(SelectedRightItemsProperty);
        set => SetValue(SelectedRightItemsProperty, value);
    }

    /// <summary>
    /// Gets or sets the transfer mode (Single, Multiple, or All).
    /// </summary>
    public TransferMode TransferMode
    {
        get => (TransferMode)GetValue(TransferModeProperty);
        set => SetValue(TransferModeProperty, value);
    }

    /// <summary>
    /// Gets or sets whether search is enabled.
    /// </summary>
    public bool IsSearchEnabled
    {
        get => (bool)GetValue(IsSearchEnabledProperty);
        set => SetValue(IsSearchEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets the height of the left ListBox.
    /// </summary>
    public double LeftListBoxHeight
    {
        get => (double)GetValue(LeftListBoxHeightProperty);
        set => SetValue(LeftListBoxHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the height of the right ListBox.
    /// </summary>
    public double RightListBoxHeight
    {
        get => (double)GetValue(RightListBoxHeightProperty);
        set => SetValue(RightListBoxHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the button foreground brush.
    /// </summary>
    public Brush? ButtonForeground
    {
        get => (Brush?)GetValue(ButtonForegroundProperty);
        set => SetValue(ButtonForegroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the button background brush.
    /// </summary>
    public Brush? ButtonBackground
    {
        get => (Brush?)GetValue(ButtonBackgroundProperty);
        set => SetValue(ButtonBackgroundProperty, value);
    }

    private static void OnLeftItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (Transfer)d;
        control.HandleItemsSourceChanged(ref control._leftItemsCollection, e.NewValue as IEnumerable, control._leftListBox);
    }

    private static void OnRightItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (Transfer)d;
        control.HandleItemsSourceChanged(ref control._rightItemsCollection, e.NewValue as IEnumerable, control._rightListBox);
    }

    private static void OnTransferModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (Transfer)d;
        control.UpdateSelectionMode();
        control.UpdateButtonStates();
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        DetachTemplatePartEvents();

        _leftListBox = GetTemplateChild(LeftListBoxPartName) as ListBox;
        _rightListBox = GetTemplateChild(RightListBoxPartName) as ListBox;
        _moveRightButton = GetTemplateChild(MoveRightButtonPartName) as Button;
        _moveLeftButton = GetTemplateChild(MoveLeftButtonPartName) as Button;
        _moveRightAllButton = GetTemplateChild(MoveRightAllButtonPartName) as Button;
        _moveLeftAllButton = GetTemplateChild(MoveLeftAllButtonPartName) as Button;

        AttachTemplatePartEvents();

        UpdateSelectionMode();
        EnsureDefaultSelection(_leftListBox);
        EnsureDefaultSelection(_rightListBox);
        UpdateSelectedItems(_leftListBox, SelectedLeftItemsProperty);
        UpdateSelectedItems(_rightListBox, SelectedRightItemsProperty);
        UpdateButtonStates();
    }

    private void DetachTemplatePartEvents()
    {
        if (_leftListBox != null)
        {
            _leftListBox.SelectionChanged -= OnLeftListBoxSelectionChanged;
            DetachDragDropHandlers(_leftListBox);
            _leftListBox = null;
        }

        if (_rightListBox != null)
        {
            _rightListBox.SelectionChanged -= OnRightListBoxSelectionChanged;
            DetachDragDropHandlers(_rightListBox);
            _rightListBox = null;
        }

        if (_moveRightButton != null)
        {
            _moveRightButton.Click -= OnMoveRightButtonClick;
            _moveRightButton = null;
        }

        if (_moveLeftButton != null)
        {
            _moveLeftButton.Click -= OnMoveLeftButtonClick;
            _moveLeftButton = null;
        }

        if (_moveRightAllButton != null)
        {
            _moveRightAllButton.Click -= OnMoveRightAllButtonClick;
            _moveRightAllButton = null;
        }

        if (_moveLeftAllButton != null)
        {
            _moveLeftAllButton.Click -= OnMoveLeftAllButtonClick;
            _moveLeftAllButton = null;
        }
    }

    private void AttachTemplatePartEvents()
    {
        if (_leftListBox != null)
        {
            _leftListBox.SelectionChanged += OnLeftListBoxSelectionChanged;
            AttachDragDropHandlers(_leftListBox);
        }

        if (_rightListBox != null)
        {
            _rightListBox.SelectionChanged += OnRightListBoxSelectionChanged;
            AttachDragDropHandlers(_rightListBox);
        }

        if (_moveRightButton != null)
        {
            _moveRightButton.Click += OnMoveRightButtonClick;
        }

        if (_moveLeftButton != null)
        {
            _moveLeftButton.Click += OnMoveLeftButtonClick;
        }

        if (_moveRightAllButton != null)
        {
            _moveRightAllButton.Click += OnMoveRightAllButtonClick;
        }

        if (_moveLeftAllButton != null)
        {
            _moveLeftAllButton.Click += OnMoveLeftAllButtonClick;
        }
    }

    private void UpdateSelectionMode()
    {
        var selectionMode = TransferMode == TransferMode.Single ? SelectionMode.Single : SelectionMode.Extended;

        if (_leftListBox != null)
        {
            _leftListBox.SelectionMode = selectionMode;
        }

        if (_rightListBox != null)
        {
            _rightListBox.SelectionMode = selectionMode;
        }
    }

    private void UpdateButtonStates()
    {
        var leftList = LeftItemsSource as IList;
        var rightList = RightItemsSource as IList;
        var leftEditable = IsCollectionEditable(leftList);
        var rightEditable = IsCollectionEditable(rightList);
        var canTransferSelection = TransferMode != TransferMode.All && leftEditable && rightEditable;
        var hasLeftItems = HasItems(LeftItemsSource);
        var hasRightItems = HasItems(RightItemsSource);
        var leftSelectionCount = _leftListBox?.SelectedItems.Count ?? 0;
        var rightSelectionCount = _rightListBox?.SelectedItems.Count ?? 0;

        if (_moveRightButton != null)
        {
            _moveRightButton.IsEnabled = canTransferSelection && leftSelectionCount > 0;
        }

        if (_moveLeftButton != null)
        {
            _moveLeftButton.IsEnabled = canTransferSelection && rightSelectionCount > 0;
        }

        if (_moveRightAllButton != null)
        {
            _moveRightAllButton.IsEnabled = leftEditable && rightEditable && hasLeftItems;
        }

        if (_moveLeftAllButton != null)
        {
            _moveLeftAllButton.IsEnabled = leftEditable && rightEditable && hasRightItems;
        }
    }

    private void OnLeftListBoxSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        UpdateSelectedItems(_leftListBox, SelectedLeftItemsProperty);
        UpdateButtonStates();
    }

    private void OnRightListBoxSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        UpdateSelectedItems(_rightListBox, SelectedRightItemsProperty);
        UpdateButtonStates();
    }

    private void OnMoveRightButtonClick(object? sender, RoutedEventArgs e)
    {
        if (!TryGetEditableLists(out var leftList, out var rightList))
        {
            return;
        }

        var recordedIndex = _leftListBox?.SelectedIndex ?? -1;
        var itemsToMove = CollectSelectedItems(_leftListBox);
        MoveItems(leftList, rightList, itemsToMove);
        ClearSelection(_leftListBox);
        RestoreSelectionAfterMove(_leftListBox, recordedIndex);
        UpdateSelectedItems(_leftListBox, SelectedLeftItemsProperty);
        UpdateSelectedItems(_rightListBox, SelectedRightItemsProperty);
        UpdateButtonStates();
    }

    private void OnMoveLeftButtonClick(object? sender, RoutedEventArgs e)
    {
        if (!TryGetEditableLists(out var leftList, out var rightList))
        {
            return;
        }

        var recordedIndex = _rightListBox?.SelectedIndex ?? -1;
        var itemsToMove = CollectSelectedItems(_rightListBox);
        MoveItems(rightList, leftList, itemsToMove);
        ClearSelection(_rightListBox);
        RestoreSelectionAfterMove(_rightListBox, recordedIndex);
        UpdateSelectedItems(_rightListBox, SelectedRightItemsProperty);
        UpdateSelectedItems(_leftListBox, SelectedLeftItemsProperty);
        UpdateButtonStates();
    }

    private void OnMoveRightAllButtonClick(object? sender, RoutedEventArgs e)
    {
        if (!TryGetEditableLists(out var leftList, out var rightList))
        {
            return;
        }

        MoveItems(leftList, rightList, leftList.Cast<object>().ToList());
        ClearSelection(_leftListBox);
        EnsureDefaultSelection(_leftListBox);
        EnsureDefaultSelection(_rightListBox);
        UpdateSelectedItems(_leftListBox, SelectedLeftItemsProperty);
        UpdateSelectedItems(_rightListBox, SelectedRightItemsProperty);
        UpdateButtonStates();
    }

    private void OnMoveLeftAllButtonClick(object? sender, RoutedEventArgs e)
    {
        if (!TryGetEditableLists(out var leftList, out var rightList))
        {
            return;
        }

        MoveItems(rightList, leftList, rightList.Cast<object>().ToList());
        ClearSelection(_rightListBox);
        EnsureDefaultSelection(_rightListBox);
        EnsureDefaultSelection(_leftListBox);
        UpdateSelectedItems(_rightListBox, SelectedRightItemsProperty);
        UpdateSelectedItems(_leftListBox, SelectedLeftItemsProperty);
        UpdateButtonStates();
    }

    private void HandleItemsSourceChanged(ref INotifyCollectionChanged? collection, IEnumerable? newSource, ListBox? listBox)
    {
        if (collection != null)
        {
            collection.CollectionChanged -= OnItemsSourceCollectionChanged;
        }

        collection = newSource as INotifyCollectionChanged;

        if (collection != null)
        {
            collection.CollectionChanged += OnItemsSourceCollectionChanged;
        }

        EnsureDefaultSelection(listBox);

        UpdateButtonStates();
    }

    private void OnItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (ReferenceEquals(sender, _leftItemsCollection))
        {
            EnsureDefaultSelection(_leftListBox);
        }
        else if (ReferenceEquals(sender, _rightItemsCollection))
        {
            EnsureDefaultSelection(_rightListBox);
        }

        UpdateButtonStates();
    }

    private static void MoveItems(IList source, IList target, IReadOnlyCollection<object> items, int insertIndex = -1)
    {
        if (source == target)
        {
            ReorderItems(source, items, insertIndex);
            return;
        }

        var movableItems = items.Where(source.Contains).ToList();
        if (movableItems.Count == 0)
        {
            return;
        }

        foreach (var item in movableItems)
        {
            source.Remove(item);
        }

        var targetIndex = insertIndex < 0 ? target.Count : Math.Min(insertIndex, target.Count);

        foreach (var item in movableItems)
        {
            if (targetIndex >= target.Count)
            {
                target.Add(item);
            }
            else
            {
                target.Insert(targetIndex, item);
            }

            targetIndex++;
        }
    }

    private static void ReorderItems(IList list, IReadOnlyCollection<object> items, int insertIndex)
    {
        var indexedItems = items
            .Select(item => (Item: item, Index: list.IndexOf(item)))
            .Where(entry => entry.Index >= 0)
            .OrderBy(entry => entry.Index)
            .ToList();

        if (indexedItems.Count == 0)
        {
            return;
        }

        foreach (var entry in indexedItems.OrderByDescending(entry => entry.Index))
        {
            list.RemoveAt(entry.Index);
        }

        var originalCount = list.Count + indexedItems.Count;
        var boundedIndex = Math.Min(Math.Max(insertIndex, 0), originalCount);
        var removedBefore = indexedItems.Count(entry => entry.Index < boundedIndex);
        var finalIndex = Math.Max(0, boundedIndex - removedBefore);
        finalIndex = Math.Min(finalIndex, list.Count);

        foreach (var entry in indexedItems)
        {
            list.Insert(finalIndex, entry.Item);
            finalIndex++;
        }
    }

    private List<object> CollectSelectedItems(ListBox? listBox)
    {
        if (listBox?.SelectedItems == null)
        {
            return new List<object>();
        }

        var selectedItems = listBox.SelectedItems.Cast<object>().ToList();

        if (TransferMode == TransferMode.Single && selectedItems.Count > 1)
        {
            return new List<object> { selectedItems[0] };
        }

        return selectedItems;
    }

    private bool TryGetEditableLists(out IList? leftList, out IList? rightList)
    {
        leftList = LeftItemsSource as IList;
        rightList = RightItemsSource as IList;

        return IsCollectionEditable(leftList) && IsCollectionEditable(rightList);
    }

    private static void ClearSelection(ListBox? listBox)
    {
        listBox?.SelectedItems.Clear();
    }

    private static bool HasItems(IEnumerable? source)
    {
        if (source == null)
        {
            return false;
        }

        if (source is ICollection collection)
        {
            return collection.Count > 0;
        }

        var enumerator = source.GetEnumerator();
        return enumerator.MoveNext();
    }

    private static bool IsCollectionEditable(IList? collection)
    {
        return collection != null && !collection.IsReadOnly && !collection.IsFixedSize;
    }

    private void UpdateSelectedItems(ListBox? listBox, DependencyProperty property)
    {
        if (listBox == null)
        {
            SetValue(property, null);
            return;
        }

        SetValue(property, listBox.SelectedItems.Cast<object>().ToList());
    }

    private void EnsureDefaultSelection(ListBox? listBox)
    {
        if (listBox == null)
        {
            return;
        }

        if (listBox.Items.Count == 0)
        {
            listBox.SelectedIndex = -1;
            return;
        }

        if (listBox.SelectedIndex < 0)
        {
            listBox.SelectedIndex = 0;
        }
    }

    private void RestoreSelectionAfterMove(ListBox? listBox, int index)
    {
        if (listBox == null)
        {
            return;
        }

        if (listBox.Items.Count == 0)
        {
            listBox.SelectedIndex = -1;
            return;
        }

        var desiredIndex = index < 0 ? 0 : index;

        while (desiredIndex >= listBox.Items.Count && desiredIndex > 0)
        {
            desiredIndex--;
        }

        listBox.SelectedIndex = Math.Min(desiredIndex, listBox.Items.Count - 1);
    }

    private void AttachDragDropHandlers(ListBox listBox)
    {
        listBox.AllowDrop = true;
        listBox.PreviewMouseLeftButtonDown += OnListBoxPreviewMouseLeftButtonDown;
        listBox.MouseMove += OnListBoxMouseMove;
        listBox.DragOver += OnListBoxDragOver;
        listBox.Drop += OnListBoxDrop;
    }

    private void DetachDragDropHandlers(ListBox listBox)
    {
        listBox.AllowDrop = false;
        listBox.PreviewMouseLeftButtonDown -= OnListBoxPreviewMouseLeftButtonDown;
        listBox.MouseMove -= OnListBoxMouseMove;
        listBox.DragOver -= OnListBoxDragOver;
        listBox.Drop -= OnListBoxDrop;
    }

    private void OnListBoxPreviewMouseLeftButtonDown(object? sender, MouseButtonEventArgs e)
    {
        _dragStartPoint = e.GetPosition(null);
        _dragSourceListBox = sender as ListBox;
        _draggedItem = FindVisualAncestor<ListBoxItem>(e.OriginalSource as DependencyObject);
    }

    private void OnListBoxMouseMove(object? sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed || _dragSourceListBox == null || _draggedItem == null)
        {
            return;
        }

        var currentPosition = e.GetPosition(null);
        var diff = _dragStartPoint - currentPosition;

        if (Math.Abs(diff.X) < SystemParameters.MinimumHorizontalDragDistance &&
            Math.Abs(diff.Y) < SystemParameters.MinimumVerticalDragDistance)
        {
            return;
        }

        var selectedItems = _dragSourceListBox.SelectedItems.Cast<object>().ToList();

        if (selectedItems.Count == 0 && _draggedItem.DataContext != null)
        {
            selectedItems.Add(_draggedItem.DataContext);
        }

        if (selectedItems.Count == 0)
        {
            return;
        }

        var dataObject = new DataObject(DragItemsFormat, selectedItems);
        dataObject.SetData(DragSourceFormat, _dragSourceListBox);

        DragDrop.DoDragDrop(_draggedItem, dataObject, DragDropEffects.Move);

        _draggedItem = null;
        _dragSourceListBox = null;
    }

    private void OnListBoxDragOver(object? sender, DragEventArgs e)
    {
        e.Effects = e.Data.GetDataPresent(DragItemsFormat) ? DragDropEffects.Move : DragDropEffects.None;
        e.Handled = true;
    }

    private void OnListBoxDrop(object? sender, DragEventArgs e)
    {
        if (sender is not ListBox targetListBox || !e.Data.GetDataPresent(DragItemsFormat))
        {
            return;
        }

        var data = e.Data.GetData(DragItemsFormat) as List<object>;

        if (data == null || data.Count == 0)
        {
            return;
        }

        var sourceListBox = e.Data.GetData(DragSourceFormat) as ListBox ?? _dragSourceListBox;

        if (sourceListBox == null)
        {
            return;
        }

        var sourceList = GetEditableList(sourceListBox);
        var targetList = GetEditableList(targetListBox);

        if (!IsCollectionEditable(sourceList) || !IsCollectionEditable(targetList))
        {
            return;
        }

        var insertIndex = CalculateDropIndex(targetListBox, e);
        MoveItems(sourceList!, targetList!, data, insertIndex);

        EnsureDefaultSelection(sourceListBox == _leftListBox ? _leftListBox : _rightListBox);
        EnsureDefaultSelection(targetListBox);
        UpdateSelectedItems(_leftListBox, SelectedLeftItemsProperty);
        UpdateSelectedItems(_rightListBox, SelectedRightItemsProperty);
        UpdateButtonStates();

        _draggedItem = null;
        _dragSourceListBox = null;

        e.Handled = true;
    }

    private int CalculateDropIndex(ListBox listBox, DragEventArgs e)
    {
        var position = e.GetPosition(listBox);
        var hit = listBox.InputHitTest(position) as DependencyObject;
        var container = FindVisualAncestor<ListBoxItem>(hit);

        if (container == null)
        {
            return listBox.Items.Count;
        }

        var index = listBox.ItemContainerGenerator.IndexFromContainer(container);

        if (index < 0)
        {
            return listBox.Items.Count;
        }

        var containerPosition = container.TransformToAncestor(listBox).Transform(new Point(0, 0));
        var relativeY = position.Y - containerPosition.Y;

        if (relativeY > container.ActualHeight / 2)
        {
            index++;
        }

        return Math.Min(index, listBox.Items.Count);
    }

    private IList? GetEditableList(ListBox? listBox)
    {
        if (listBox == null)
        {
            return null;
        }

        return listBox == _leftListBox ? LeftItemsSource as IList : RightItemsSource as IList;
    }

    private static T? FindVisualAncestor<T>(DependencyObject? current) where T : DependencyObject
    {
        while (current != null)
        {
            if (current is T typed)
            {
                return typed;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return null;
    }
}
