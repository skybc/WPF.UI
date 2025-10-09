using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf.Ui.Controls
{
    /// <summary>
    /// 表单项控件。左侧显示 Title，右侧显示自定义内容。
    /// </summary>
    public class FormItem : ContentControl
    {
        static FormItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormItem), new FrameworkPropertyMetadata(typeof(FormItem)));
        }

        /// <summary>
        /// 表单项标题。
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(FormItem), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 标题区域宽度。
        /// Changed to GridLength so it can be bound to ColumnDefinition.Width.
        /// </summary>
        public static readonly DependencyProperty TitleWidthProperty = DependencyProperty.Register(
            nameof(TitleWidth), typeof(GridLength), typeof(FormItem), new PropertyMetadata(new GridLength(120.0), OnTitleWidthChanged));

        private static void OnTitleWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormItem formItem && e.NewValue is GridLength newWidth)
            {
                // If pixel-based width is negative, clamp to 0
                if (newWidth.GridUnitType == GridUnitType.Pixel && newWidth.Value < 0)
                {
                    formItem.SetValue(TitleWidthProperty, new GridLength(0.0));
                }
                else
                {
                    // no-op: valid GridLength
                }
            }
        }

        /// <summary>
        /// 标题水平对齐方式。
        /// </summary>
        public static readonly DependencyProperty TitleHorizontalAlignmentProperty = DependencyProperty.Register(
            nameof(TitleHorizontalAlignment), typeof(HorizontalAlignment), typeof(FormItem), new PropertyMetadata(HorizontalAlignment.Right));

        /// <summary>
        /// 标题区域内边距。
        /// </summary>
        public static readonly DependencyProperty TitlePaddingProperty = DependencyProperty.Register(
            nameof(TitlePadding), typeof(Thickness), typeof(FormItem), new PropertyMetadata(new Thickness(4, 0, 4, 0)));

        /// <summary>
        /// 标题前景色。
        /// </summary>
        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.Register(
            nameof(TitleForeground), typeof(Brush), typeof(FormItem), new PropertyMetadata(null));

        /// <summary>
        /// 标题字体大小。
        /// </summary>
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(
            nameof(TitleFontSize), typeof(double), typeof(FormItem), new PropertyMetadata(14.0));

        /// <summary>
        /// 标题字体粗细。
        /// </summary>
        public static readonly DependencyProperty TitleFontWeightProperty = DependencyProperty.Register(
            nameof(TitleFontWeight), typeof(FontWeight), typeof(FormItem), new PropertyMetadata(FontWeights.Normal));

        /// <summary>
        /// 内容区域内边距。
        /// </summary>
        public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register(
            nameof(ContentPadding), typeof(Thickness), typeof(FormItem), new PropertyMetadata(new Thickness(0)));

        /// <summary>
        /// 表单项标题。
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// 标题区域宽度。
        /// </summary>
        public GridLength TitleWidth
        {
            get => (GridLength)GetValue(TitleWidthProperty);
            set => SetValue(TitleWidthProperty, value);
        }

        /// <summary>
        /// 标题水平对齐方式。
        /// </summary>
        public HorizontalAlignment TitleHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(TitleHorizontalAlignmentProperty);
            set => SetValue(TitleHorizontalAlignmentProperty, value);
        }

        /// <summary>
        /// 标题区域内边距。
        /// </summary>
        public Thickness TitlePadding
        {
            get => (Thickness)GetValue(TitlePaddingProperty);
            set => SetValue(TitlePaddingProperty, value);
        }

        /// <summary>
        /// 标题前景色。
        /// </summary>
        public Brush TitleForeground
        {
            get => (Brush)GetValue(TitleForegroundProperty);
            set => SetValue(TitleForegroundProperty, value);
        }

        /// <summary>
        /// 标题字体大小。
        /// </summary>
        public double TitleFontSize
        {
            get => (double)GetValue(TitleFontSizeProperty);
            set => SetValue(TitleFontSizeProperty, value);
        }

        /// <summary>
        /// 标题字体粗细。
        /// </summary>
        public FontWeight TitleFontWeight
        {
            get => (FontWeight)GetValue(TitleFontWeightProperty);
            set => SetValue(TitleFontWeightProperty, value);
        }

        /// <summary>
        /// 内容区域内边距。
        /// </summary>
        public Thickness ContentPadding
        {
            get => (Thickness)GetValue(ContentPaddingProperty);
            set => SetValue(ContentPaddingProperty, value);
        }

        public FormItem()
        {
            // 默认绑定主题色
            if (TitleForeground == null)
            {
                SetResourceReference(TitleForegroundProperty, "TextFillColorPrimaryBrush");
            }
            // 默认 Padding
            if (TitlePadding == new Thickness(0))
            {
                SetResourceReference(TitlePaddingProperty, "ControlPadding");
            }
            if (ContentPadding == new Thickness(0))
            {
                SetResourceReference(ContentPaddingProperty, "ControlPadding");
            }
        }
    }
}
