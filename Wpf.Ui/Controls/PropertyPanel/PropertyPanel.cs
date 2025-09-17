using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;

namespace Wpf.Ui.Controls;

/// <summary>
/// A dynamic property grid-like panel that generates editors based on attributes and types.
/// Inherits from ItemsControl and binds its ItemsSource to an internal collection of PropertyItem.
/// </summary>
public class PropertyPanel : ItemsControl
{
    public PropertyPanel()
    {
        ItemsCollection = new ObservableCollection<PropertyItem>();
        base.ItemsSource = ItemsCollection;
        TemplateSelector = new PropertyTemplateSelector();
        Loaded += OnLoaded;

        ItemContainerStyle = new Style(typeof(ContentPresenter))
        {
            Setters = { new Setter(MarginProperty, new Thickness(0, 4, 0, 4)) }
        };

        // Expose selector as resource for XAML DynamicResource binding inside default ItemTemplate
        Resources["PropertyPanel.TemplateSelector"] = TemplateSelector;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        EnsureDefaultResources();
        TryApplyDefaultItemTemplate();
        RebuildItems();
    }

    private void EnsureDefaultResources()
    {
        try
        {
            if (TryFindResource("PropertyPanel.TextTemplate") == null)
            {
                var dict = new ResourceDictionary
                {
                    Source = new Uri("/Wpf.Ui;component/Controls/PropertyPanel/PropertyPanel.xaml", UriKind.Relative)
                };
                Resources.MergedDictionaries.Add(dict);
            }
        }
        catch
        {
            // ignore if dictionary not found
        }
    }

    private void TryApplyDefaultItemTemplate()
    {
        if (ItemTemplate != null)
            return;

        var tpl = TryFindResource("PropertyPanel.ItemTemplate") as DataTemplate;
        if (tpl != null)
        {
            ItemTemplate = tpl;
        }
        else
        {
            // Fallback minimal template: label then editor vertically
            var root = new FrameworkElementFactory(typeof(StackPanel));
            root.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);

            var label = new FrameworkElementFactory(typeof(TextBlock));
            label.SetBinding(TextBlock.TextProperty, new Binding("DisplayName"));
            root.AppendChild(label);

            var editor = new FrameworkElementFactory(typeof(ContentPresenter));
            editor.SetBinding(ContentPresenter.ContentProperty, new Binding());
            editor.SetValue(ContentPresenter.ContentTemplateSelectorProperty, TemplateSelector);
            root.AppendChild(editor);

            ItemTemplate = new DataTemplate(typeof(PropertyItem)) { VisualTree = root };
        }
    }

    #region SelectedObject
    public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
        name: nameof(SelectedObject),
        propertyType: typeof(object),
        ownerType: typeof(PropertyPanel),
        typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedObjectChanged));

    public object? SelectedObject
    {
        get => GetValue(SelectedObjectProperty);
        set => SetValue(SelectedObjectProperty, value);
    }

    private static void OnSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PropertyPanel panel)
        {
            if (e.OldValue is INotifyPropertyChanged oldInpc)
                oldInpc.PropertyChanged -= panel.OnSelectedObjectPropertyChanged;

            if (e.NewValue is INotifyPropertyChanged newInpc)
                newInpc.PropertyChanged += panel.OnSelectedObjectPropertyChanged;

            panel.RebuildItems();
        }
    }

    private void OnSelectedObjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Items listen to owner changes; nothing to do here.
    }
    #endregion

    #region Items backing collection (read-only)
    internal ObservableCollection<PropertyItem> ItemsCollection { get; }
    #endregion

    #region TemplateSelector DP (allows custom selector injection)
    public static readonly DependencyProperty TemplateSelectorProperty = DependencyProperty.Register(
        nameof(TemplateSelector), typeof(DataTemplateSelector), typeof(PropertyPanel), new PropertyMetadata(new PropertyTemplateSelector(), OnTemplateSelectorChanged));

    private static void OnTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PropertyPanel p)
        {
            p.Resources["PropertyPanel.TemplateSelector"] = e.NewValue as DataTemplateSelector ?? new PropertyTemplateSelector();
        }
    }

    public new DataTemplateSelector TemplateSelector
    {
        get => (DataTemplateSelector)GetValue(TemplateSelectorProperty);
        set => SetValue(TemplateSelectorProperty, value);
    }
    #endregion

    private void RebuildItems()
    {
        ItemsCollection.Clear();

        if (SelectedObject == null)
            return;

        var type = SelectedObject.GetType();
        var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.GetMethod != null)
            .Select(p => new { Prop = p, Attr = p.GetCustomAttribute<PropertyPanelAttribute>(true) })
            .Where(x => x.Attr != null && x.Attr.IsVisible)
            .OrderBy(x => x.Attr!.Order)
            .ThenBy(x => x.Prop.Name)
            .ToList();

        foreach (var p in props)
        {
            ItemsCollection.Add(new PropertyItem(SelectedObject, p.Prop, p.Attr!));
        }
    }
}
