namespace Wpf.Ui.Controls;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Container for a PropertyGroupItem that displays properties in an Expander control.
/// </summary>
internal sealed class PropertyGroupContainer : Grid
{
    private FrameworkElement itemsContainer;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyGroupContainer"/> class.
    /// </summary>
    public PropertyGroupContainer()
    {
        this.VerticalAlignment = VerticalAlignment.Top;
        this.Margin = new Thickness(0, 0, 0, 0);

        this.Loaded += this.OnLoaded;
        this.Unloaded += this.OnUnloaded;
        this.DataContextChanged += this.OnDataContextChanged;
    }

    /// <summary>
    /// Gets the PropertyGroupItem associated with this container.
    /// </summary>
    public PropertyGroupItem Group => this.DataContext as PropertyGroupItem;

    /// <summary>
    /// Gets or sets a value indicating whether this is the first group (needs top rounded corners).
    /// </summary>
    public bool IsFirstGroup { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is the last group (needs bottom rounded corners).
    /// </summary>
    public bool IsLastGroup { get; set; }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is PropertyGroupItem group && this.Children.Count == 0)
        {
            this.InitializeContainer(group);
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is PropertyGroupItem group && this.Children.Count == 0)
        {
            this.InitializeContainer(group);
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        this.Dispose();
    }

    private void InitializeContainer(PropertyGroupItem group)
    {
        this.Children.Clear();

        if (group == null)
        {
            return;
        }

        var expander = new Expander
        {
            Header = group.GroupName,
            IsExpanded = group.IsExpanded,
            Margin = new Thickness(0, 0, 0, 0),
        };

        // Set CornerRadius based on position in list
        var cornerRadius = new System.Windows.CornerRadius(0);
        if (this.IsFirstGroup && this.IsLastGroup)
        {
            // Only one group - all corners rounded
            cornerRadius = new System.Windows.CornerRadius(4);
        }
        else if (this.IsFirstGroup)
        {
            // First group - top corners rounded
            cornerRadius = new System.Windows.CornerRadius(4, 4, 0, 0);
        }
        else if (this.IsLastGroup)
        {
            // Last group - bottom corners rounded
            cornerRadius = new System.Windows.CornerRadius(0, 0, 4, 4);
        }
        // else: middle groups - no rounding (default 0,0,0,0)

        expander.SetValue(Border.CornerRadiusProperty, cornerRadius);

        // Bind IsExpanded to the group
        expander.SetBinding(Expander.IsExpandedProperty, new System.Windows.Data.Binding(nameof(PropertyGroupItem.IsExpanded))
        {
            Source = group,
            Mode = System.Windows.Data.BindingMode.TwoWay,
        });

        // Create a StackPanel to hold the items
        var stackPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
        };

        // Add each property item container to the stack panel
        foreach (var propertyItem in group.Items)
        {
            var itemContainer = new PropertyItemContainer
            {
                DataContext = propertyItem,
            };
            stackPanel.Children.Add(itemContainer);
        }

        expander.Content = stackPanel;
        this.Children.Add(expander);
        this.itemsContainer = expander;
    }

    /// <summary>
    /// Cleans up resources.
    /// </summary>
    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        this.isDisposed = true;

        this.Loaded -= this.OnLoaded;
        this.Unloaded -= this.OnUnloaded;
        this.DataContextChanged -= this.OnDataContextChanged;

        this.Children.Clear();
    }

    /// <summary>
    /// Finds the parent element of the specified type in the visual tree.
    /// </summary>
    private T FindParent<T>(DependencyObject child)
        where T : DependencyObject
    {
        DependencyObject parent = System.Windows.Media.VisualTreeHelper.GetParent(child);

        while (parent != null)
        {
            if (parent is T typedParent)
            {
                return typedParent;
            }

            parent = System.Windows.Media.VisualTreeHelper.GetParent(parent);
        }

        return null;
    }
}
