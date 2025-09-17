using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Wpf.Ui.Controls;

/// <summary>
/// Template selector that chooses an editor template for a given PropertyItem.
/// Uses attribute and type information.
/// </summary>
public sealed class PropertyTemplateSelector : DataTemplateSelector
{
    public DataTemplate? TextTemplate { get; set; }
    public DataTemplate? BoolTemplate { get; set; }
    public DataTemplate? EnumTemplate { get; set; }
    public DataTemplate? NumberTemplate { get; set; }
    public DataTemplate? SliderTemplate { get; set; }
    public DataTemplate? DateTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is not PropertyItem pi)
            return base.SelectTemplate(item, container);

        var attr = pi.Attribute;

        // Resource key has top priority
        if (attr.TemplateResourceKey != null && container is FrameworkElement fe)
        {
            var found = fe.TryFindResource(attr.TemplateResourceKey) as DataTemplate;
            if (found != null)
                return found;
        }

        // Custom element type requested -> build an inline template hosting that element
        if (attr.Editor == PropertyEditorKind.Custom || attr.EditorElementType != null)
        {
            var elementType = attr.EditorElementType;
            if (elementType != null && typeof(FrameworkElement).IsAssignableFrom(elementType))
            {
                return BuildElementTemplate(elementType, attr.EditorValuePath);
            }
        }

        // Explicit editor kind
        switch (attr.Editor)
        {
            case PropertyEditorKind.TextBox:
                return TextTemplate ?? FindResource(container, "PropertyPanel.TextTemplate");
            case PropertyEditorKind.CheckBox:
            case PropertyEditorKind.ToggleSwitch:
                return BoolTemplate ?? FindResource(container, "PropertyPanel.BoolTemplate");
            case PropertyEditorKind.ComboBox:
                if (!pi.IsEnum)
                    return BuildComboTemplate(attr, container);
                return EnumTemplate ?? FindResource(container, "PropertyPanel.EnumTemplate");
            case PropertyEditorKind.Slider:
                return SliderTemplate ?? FindResource(container, "PropertyPanel.SliderTemplate");
            case PropertyEditorKind.DatePicker:
                return DateTemplate ?? FindResource(container, "PropertyPanel.DateTemplate");
        }

        // Auto by type
        var t = pi.UnderlyingType;
        if (t == typeof(string)) return TextTemplate ?? FindResource(container, "PropertyPanel.TextTemplate");
        if (t == typeof(bool)) return BoolTemplate ?? FindResource(container, "PropertyPanel.BoolTemplate");
        if (t.IsEnum) return EnumTemplate ?? FindResource(container, "PropertyPanel.EnumTemplate");
        if (t == typeof(DateTime)) return DateTemplate ?? FindResource(container, "PropertyPanel.DateTemplate");
        if (IsNumeric(t))
        {
            // prefer slider if attribute specifies range
            if (pi.Min != 0 || pi.Max != 100 || attr.Editor == PropertyEditorKind.Slider)
                return SliderTemplate ?? FindResource(container, "PropertyPanel.SliderTemplate");
            return NumberTemplate ?? FindResource(container, "PropertyPanel.NumberTemplate");
        }

        // Fallback to Text
        return TextTemplate ?? FindResource(container, "PropertyPanel.TextTemplate");
    }

    private static bool IsNumeric(Type t)
    {
        t = Nullable.GetUnderlyingType(t) ?? t;
        return t == typeof(byte) || t == typeof(sbyte) ||
               t == typeof(short) || t == typeof(ushort) ||
               t == typeof(int) || t == typeof(uint) ||
               t == typeof(long) || t == typeof(ulong) ||
               t == typeof(float) || t == typeof(double) || t == typeof(decimal);
    }

    private static DataTemplate BuildElementTemplate(Type elementType, string valuePath)
    {
        var factory = new FrameworkElementFactory(elementType);
        var dp = GetDependencyPropertyByName(elementType, valuePath);
        if (dp != null)
        {
            factory.SetBinding(
                dp,
                new Binding("CurrentValue") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }
            );
        }
        var template = new DataTemplate { VisualTree = factory };
        return template;
    }

    private static DataTemplate BuildComboTemplate(PropertyPanelAttribute attr, DependencyObject container)
    {
        var factory = new FrameworkElementFactory(typeof(ComboBox));
        factory.SetBinding(Selector.SelectedItemProperty, new Binding("CurrentValue") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        if (attr.ItemsSourceResourceKey != null && container is FrameworkElement)
        {
            factory.SetResourceReference(ItemsControl.ItemsSourceProperty, attr.ItemsSourceResourceKey);
        }
        else if (!string.IsNullOrWhiteSpace(attr.ItemsSourcePath))
        {
            factory.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(attr.ItemsSourcePath!));
        }

        if (!string.IsNullOrWhiteSpace(attr.DisplayMemberPath))
            factory.SetValue(ComboBox.DisplayMemberPathProperty, attr.DisplayMemberPath);
        if (!string.IsNullOrWhiteSpace(attr.SelectedValuePath))
            factory.SetValue(ComboBox.SelectedValuePathProperty, attr.SelectedValuePath);

        var template = new DataTemplate { VisualTree = factory };
        return template;
    }

    private static DependencyProperty? GetDependencyPropertyByName(Type elementType, string dpName)
    {
        // Try find a DP field named <Name>Property
        var field = elementType.GetField(dpName + "Property", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy);
        if (field?.GetValue(null) is DependencyProperty dp)
            return dp;
        return null;
    }

    private static DataTemplate? FindResource(DependencyObject container, object key)
    {
        return (container as FrameworkElement)?.TryFindResource(key) as DataTemplate;
    }
}
