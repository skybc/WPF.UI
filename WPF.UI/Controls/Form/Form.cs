using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Wpf.Ui.Controls
{
    /// <summary>
    /// 表单控件。承载多个 <see cref="FormItem"/>，并统一设置标题等通用属性。
    /// FormItem 上显式设置的值优先于 Form 上的值。
    /// </summary>
    public class Form : ItemsControl
    {
        static Form()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Form), new FrameworkPropertyMetadata(typeof(Form)));
        }

        public static readonly DependencyProperty TitleWidthProperty = DependencyProperty.Register(
            nameof(TitleWidth), typeof(GridLength), typeof(Form),
            new PropertyMetadata(new GridLength(80)));

        public static readonly DependencyProperty TitleHorizontalAlignmentProperty = DependencyProperty.Register(
            nameof(TitleHorizontalAlignment), typeof(HorizontalAlignment), typeof(Form),
            new PropertyMetadata(HorizontalAlignment.Right));

        public static readonly DependencyProperty TitlePlacementProperty = DependencyProperty.Register(
            nameof(TitlePlacement), typeof(TitlePlacement), typeof(Form),
            new PropertyMetadata(TitlePlacement.Left));

        public static readonly DependencyProperty TitlePaddingProperty = DependencyProperty.Register(
            nameof(TitlePadding), typeof(Thickness), typeof(Form),
            new PropertyMetadata(new Thickness(4, 2, 4, 2)));

        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.Register(
            nameof(TitleForeground), typeof(Brush), typeof(Form),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(
            nameof(TitleFontSize), typeof(double), typeof(Form),
            new PropertyMetadata(14.0));

        public static readonly DependencyProperty TitleFontWeightProperty = DependencyProperty.Register(
            nameof(TitleFontWeight), typeof(FontWeight), typeof(Form),
            new PropertyMetadata(FontWeights.Normal));

        public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register(
            nameof(ContentPadding), typeof(Thickness), typeof(Form),
            new PropertyMetadata(new Thickness(0)));

        /// <summary>标题区域宽度。</summary>
        public GridLength TitleWidth
        {
            get => (GridLength)GetValue(TitleWidthProperty);
            set => SetValue(TitleWidthProperty, value);
        }

        /// <summary>标题水平对齐方式。</summary>
        public HorizontalAlignment TitleHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(TitleHorizontalAlignmentProperty);
            set => SetValue(TitleHorizontalAlignmentProperty, value);
        }

        /// <summary>标题放置位置。</summary>
        public TitlePlacement TitlePlacement
        {
            get => (TitlePlacement)GetValue(TitlePlacementProperty);
            set => SetValue(TitlePlacementProperty, value);
        }

        /// <summary>标题区域内边距。</summary>
        public Thickness TitlePadding
        {
            get => (Thickness)GetValue(TitlePaddingProperty);
            set => SetValue(TitlePaddingProperty, value);
        }

        /// <summary>标题前景色。</summary>
        public Brush TitleForeground
        {
            get => (Brush)GetValue(TitleForegroundProperty);
            set => SetValue(TitleForegroundProperty, value);
        }

        /// <summary>标题字体大小。</summary>
        public double TitleFontSize
        {
            get => (double)GetValue(TitleFontSizeProperty);
            set => SetValue(TitleFontSizeProperty, value);
        }

        /// <summary>标题字体粗细。</summary>
        public FontWeight TitleFontWeight
        {
            get => (FontWeight)GetValue(TitleFontWeightProperty);
            set => SetValue(TitleFontWeightProperty, value);
        }

        /// <summary>内容区域边距。</summary>
        public Thickness ContentPadding
        {
            get => (Thickness)GetValue(ContentPaddingProperty);
            set => SetValue(ContentPaddingProperty, value);
        }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is FormItem;

        protected override DependencyObject GetContainerForItemOverride() => new FormItem();

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is not FormItem formItem)
            {
                return;
            }

            formItem.SetCurrentValue(MarginProperty, new Thickness(0, 0, 0, 4));

            BindIfNotSet(formItem, FormItem.TitleWidthProperty, TitleWidthProperty);
            BindIfNotSet(formItem, FormItem.TitleHorizontalAlignmentProperty, TitleHorizontalAlignmentProperty);
            BindIfNotSet(formItem, FormItem.TitlePlacementProperty, TitlePlacementProperty);
            BindIfNotSet(formItem, FormItem.TitlePaddingProperty, TitlePaddingProperty);
            BindIfNotSet(formItem, FormItem.TitleForegroundProperty, TitleForegroundProperty, skipIfNull: true);
            BindIfNotSet(formItem, FormItem.TitleFontSizeProperty, TitleFontSizeProperty);
            BindIfNotSet(formItem, FormItem.TitleFontWeightProperty, TitleFontWeightProperty);
            BindIfNotSet(formItem, FormItem.ContentPaddingProperty, ContentPaddingProperty);
        }

        private void BindIfNotSet(FormItem target, DependencyProperty targetProperty, DependencyProperty sourceProperty, bool skipIfNull = false)
        {
            if (target.ReadLocalValue(targetProperty) != DependencyProperty.UnsetValue)
            {
                return;
            }

            if (skipIfNull && GetValue(sourceProperty) == null)
            {
                return;
            }

            target.SetBinding(targetProperty, new Binding(sourceProperty.Name)
            {
                Source = this,
                Mode = BindingMode.OneWay,
            });
        }
    }
}
