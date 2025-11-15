using System.Reflection;
using System.Windows.Controls;

namespace Wpf.Ui.Controls;

/// <summary>
/// Enum for sort mode.
/// </summary>
public enum PropertySortMode
{
    /// <summary>
    /// Sort by category (group).
    /// </summary>
    Category = 0,

    /// <summary>
    /// Sort by name (flat list).
    /// </summary>
    Name = 1,
}

/// <summary>
/// A dynamic property grid-like panel that generates editors based on attributes and types.
/// Uses ItemsControl with direct control instantiation for better performance and control.
/// </summary>
[TemplatePart(Name = "PART_ItemsPanel", Type = typeof(StackPanel))]
public class PropertyPanel : ItemsControl
{
    /// <summary>
    /// Dependency property for SelectedObject.
    /// </summary>
    public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
        nameof(SelectedObject),
        typeof(object),
        typeof(PropertyPanel),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedObjectChanged));

    /// <summary>
    /// Dependency property for DisplayNameWidth.
    /// </summary>
    public static readonly DependencyProperty DisplayNameWidthProperty = DependencyProperty.Register(
        nameof(DisplayNameWidth),
        typeof(double),
        typeof(PropertyPanel),
        new PropertyMetadata(100d, OnDisplayNamePropertiesChanged));

    /// <summary>
    /// Dependency property for DisplayNameHorizontalAlignment.
    /// </summary>
    public static readonly DependencyProperty DisplayNameHorizontalAlignmentProperty = DependencyProperty.Register(
        nameof(DisplayNameHorizontalAlignment),
        typeof(HorizontalAlignment),
        typeof(PropertyPanel),
        new PropertyMetadata(HorizontalAlignment.Left, OnDisplayNamePropertiesChanged));

    /// <summary>
    /// Dependency property for DisplayNameVerticalAlignment.
    /// </summary>
    public static readonly DependencyProperty DisplayNameVerticalAlignmentProperty = DependencyProperty.Register(
        nameof(DisplayNameVerticalAlignment),
        typeof(VerticalAlignment),
        typeof(PropertyPanel),
        new PropertyMetadata(VerticalAlignment.Center, OnDisplayNamePropertiesChanged));

    /// <summary>
    /// Dependency property for SortMode.
    /// </summary>
    public static readonly DependencyProperty SortModeProperty = DependencyProperty.Register(
        nameof(SortMode),
        typeof(PropertySortMode),
        typeof(PropertyPanel),
        new PropertyMetadata(PropertySortMode.Category, OnSortModeChanged));

    /// <summary>
    /// Dependency property for SearchText.
    /// </summary>
    public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
        nameof(SearchText),
        typeof(string),
        typeof(PropertyPanel),
        new PropertyMetadata(string.Empty, OnSearchTextChanged));

    private Dictionary<PropertyGroupItem, (bool IsFirst, bool IsLast)> pendingGroupMetadata = new Dictionary<PropertyGroupItem, (bool IsFirst, bool IsLast)>();

    /// <summary>
    /// Initializes static members of the PropertyPanel class.
    /// </summary>
    static PropertyPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(PropertyPanel),
            new FrameworkPropertyMetadata(typeof(PropertyPanel)));
    }

    /// <summary>
    /// Initializes a new instance of the PropertyPanel class.
    /// </summary>
    public PropertyPanel()
    {
        this.Loaded += this.OnLoaded;
        this.Unloaded += this.OnUnloaded;
    }

    /// <summary>
    /// Gets or sets the selected object for property editing.
    /// </summary>
    public object SelectedObject
    {
        get => this.GetValue(SelectedObjectProperty);
        set => this.SetValue(SelectedObjectProperty, value);
    }

    /// <summary>
    /// Gets or sets the width of the DisplayName label. Default is 100.
    /// </summary>
    public double DisplayNameWidth
    {
        get => (double)this.GetValue(DisplayNameWidthProperty);
        set => this.SetValue(DisplayNameWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal alignment of the DisplayName label. Default is Left.
    /// </summary>
    public HorizontalAlignment DisplayNameHorizontalAlignment
    {
        get => (HorizontalAlignment)this.GetValue(DisplayNameHorizontalAlignmentProperty);
        set => this.SetValue(DisplayNameHorizontalAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical alignment of the DisplayName label. Default is Center.
    /// </summary>
    public VerticalAlignment DisplayNameVerticalAlignment
    {
        get => (VerticalAlignment)this.GetValue(DisplayNameVerticalAlignmentProperty);
        set => this.SetValue(DisplayNameVerticalAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the sort mode (Category or Name). Default is Category.
    /// </summary>
    public PropertySortMode SortMode
    {
        get => (PropertySortMode)this.GetValue(SortModeProperty);
        set => this.SetValue(SortModeProperty, value);
    }

    /// <summary>
    /// Gets or sets the search text for filtering properties. Default is empty.
    /// </summary>
    public string SearchText
    {
        get => (string)this.GetValue(SearchTextProperty);
        set => this.SetValue(SearchTextProperty, value);
    }

    /// <summary>
    /// Called when the control is initialized.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        this.RebuildItems();
        this.WireUpHeaderButtons();
    }

    /// <summary>
    /// Wires up the header sort buttons.
    /// </summary>
    private void WireUpHeaderButtons()
    {
        var sortCategoryButton = this.Template?.FindName("SortByCategoryButton", this) as System.Windows.Controls.Primitives.ToggleButton;
        var sortNameButton = this.Template?.FindName("SortByNameButton", this) as System.Windows.Controls.Primitives.ToggleButton;

        if (sortCategoryButton != null)
        {
            // Set initial state based on current SortMode
            sortCategoryButton.IsChecked = this.SortMode == PropertySortMode.Category;
            
            sortCategoryButton.Checked += (s, e) =>
            {
                this.SetCurrentValue(SortModeProperty, PropertySortMode.Category);
                if (sortNameButton != null)
                {
                    sortNameButton.IsChecked = false;
                }
            };
            
            sortCategoryButton.Unchecked += (s, e) =>
            {
                // Prevent unchecking - must have at least one selected
                if (sortNameButton != null && sortNameButton.IsChecked != true)
                {
                    sortCategoryButton.IsChecked = true;
                }
            };
        }

        if (sortNameButton != null)
        {
            // Set initial state based on current SortMode
            sortNameButton.IsChecked = this.SortMode == PropertySortMode.Name;
            
            sortNameButton.Checked += (s, e) =>
            {
                this.SetCurrentValue(SortModeProperty, PropertySortMode.Name);
                if (sortCategoryButton != null)
                {
                    sortCategoryButton.IsChecked = false;
                }
            };
            
            sortNameButton.Unchecked += (s, e) =>
            {
                // Prevent unchecking - must have at least one selected
                if (sortCategoryButton != null && sortCategoryButton.IsChecked != true)
                {
                    sortNameButton.IsChecked = true;
                }
            };
        }
    }

    /// <summary>
    /// Called when items collection changed.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);

        if (e.NewItems != null)
        {
            foreach (var newItem in e.NewItems.OfType<PropertyGroupItem>())
            {
                if (this.pendingGroupMetadata.TryGetValue(newItem, out var metadata))
                {
                    var container = this.ItemContainerGenerator.ContainerFromItem(newItem) as PropertyGroupContainer;
                    if (container != null)
                    {
                        container.IsFirstGroup = metadata.IsFirst;
                        container.IsLastGroup = metadata.IsLast;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Called when the control is loaded.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        this.RebuildItems();
        this.WireUpHeaderButtons();
    }

    /// <summary>
    /// Called when the control is unloaded.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        this.CleanupContainers();
    }

    private static void OnSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PropertyPanel panel)
        {
            if (e.OldValue is INotifyPropertyChanged oldInpc)
            {
                oldInpc.PropertyChanged -= panel.OnSelectedObjectPropertyChanged;
            }

            if (e.NewValue is INotifyPropertyChanged newInpc)
            {
                newInpc.PropertyChanged += panel.OnSelectedObjectPropertyChanged;
            }

            panel.RebuildItems();
        }
    }

    private static void OnDisplayNamePropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PropertyPanel panel)
        {
            panel.RebuildItems();
        }
    }

    private static void OnSortModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PropertyPanel panel)
        {
            // Update button states when SortMode changes
            var sortCategoryButton = panel.Template?.FindName("SortByCategoryButton", panel) as System.Windows.Controls.Primitives.ToggleButton;
            var sortNameButton = panel.Template?.FindName("SortByNameButton", panel) as System.Windows.Controls.Primitives.ToggleButton;

            if (sortCategoryButton != null)
            {
                sortCategoryButton.IsChecked = panel.SortMode == PropertySortMode.Category;
            }

            if (sortNameButton != null)
            {
                sortNameButton.IsChecked = panel.SortMode == PropertySortMode.Name;
            }

            panel.RebuildItems();
        }
    }

    private static void OnSearchTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PropertyPanel panel)
        {
            panel.RebuildItems();
        }
    }

    private void OnSelectedObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }

    private void CleanupContainers()
    {
        foreach (var item in this.Items.OfType<PropertyItem>())
        {
            item.Dispose();
        }

        foreach (var group in this.Items.OfType<PropertyGroupItem>())
        {
            group.Dispose();
        }

        this.Items.Clear();
        this.pendingGroupMetadata.Clear();
    }

    private bool MatchesSearchText(PropertyItem item)
    {
        if (string.IsNullOrWhiteSpace(this.SearchText))
        {
            return true;
        }

        return item.DisplayName.Contains(this.SearchText, System.StringComparison.OrdinalIgnoreCase);
    }

    private void RebuildItems()
    {
        this.CleanupContainers();

        if (this.SelectedObject == null)
        {
            return;
        }

        var type = this.SelectedObject.GetType();
        var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.GetMethod != null)
            .Select(p => new { Prop = p, Attr = p.GetCustomAttribute<PropertyPanelAttribute>(true) })
            .Where(x => x.Attr != null && x.Attr.IsVisible)
            .OrderBy(x => x.Attr.Order)
            .ThenBy(x => x.Prop.Name)
            .ToList();

        var filteredProps = props
            .Select(p => new { Prop = p.Prop, Attr = p.Attr, Item = new PropertyItem(this.SelectedObject, p.Prop, p.Attr) })
            .Where(x => this.MatchesSearchText(x.Item))
            .ToList();

        if (this.SortMode == PropertySortMode.Name)
        {
            var sortedByName = filteredProps
                .OrderBy(x => x.Item.DisplayName)
                .ToList();

            foreach (var item in sortedByName)
            {
                this.Items.Add(item.Item);
            }
        }
        else
        {
            var groupedProps = filteredProps
                .GroupBy(x => string.IsNullOrWhiteSpace(x.Attr.GroupName) ? null : x.Attr.GroupName)
                .OrderBy(g => g.Key == null ? 0 : 1)
                .ThenBy(g => g.Key ?? string.Empty)
                .ToList();

            var totalGroups = groupedProps.Count;
            var currentGroupIndex = 0;

            foreach (var group in groupedProps)
            {
                var isFirstGroup = currentGroupIndex == 0;
                var isLastGroup = currentGroupIndex == totalGroups - 1;

                if (group.Key == null)
                {
                    // No grouping - add items directly
                    foreach (var p in group)
                    {
                        this.Items.Add(p.Item);
                    }
                }
                else
                {
                    // Create a group
                    var groupItem = new PropertyGroupItem(group.Key);
                    foreach (var p in group)
                    {
                        groupItem.Items.Add(p.Item);
                    }

                    this.Items.Add(groupItem);

                    // Store metadata for this group
                    this.pendingGroupMetadata[groupItem] = (isFirstGroup, isLastGroup);
                }

                currentGroupIndex++;
            }

            // Apply metadata immediately after all groups are added
            this.ApplyGroupMetadata();
        }
    }

    /// <summary>
    /// Applies group metadata (IsFirstGroup/IsLastGroup) to all PropertyGroupContainers.
    /// </summary>
    private void ApplyGroupMetadata()
    {
        var groupItems = this.Items.OfType<PropertyGroupItem>().ToList();
        
        for (int i = 0; i < groupItems.Count; i++)
        {
            var groupItem = groupItems[i];
            var isFirstGroup = i == 0;
            var isLastGroup = i == groupItems.Count - 1;

            var container = this.ItemContainerGenerator.ContainerFromItem(groupItem) as PropertyGroupContainer;
            if (container != null)
            {
                container.IsFirstGroup = isFirstGroup;
                container.IsLastGroup = isLastGroup;
            }
            else
            {
                // If container not yet created, store for later application
                this.pendingGroupMetadata[groupItem] = (isFirstGroup, isLastGroup);
            }
        }
    }
}
