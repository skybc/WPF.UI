
using System.Windows.Markup;

namespace Wpf.Ui;

/// <summary>
/// 本地化扩展
/// </summary>


public class LocalizeExtension : MarkupExtension
{
    private DependencyObject targetObject;
    private DependencyProperty targetProperty;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizeExtension" /> class.
    /// </summary>
    public LocalizeExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizeExtension" /> class.
    /// </summary>
    /// <param name="text"> </param>
    public LocalizeExtension(string text)
    {

        Text = text;
    }

    /// <summary>
    /// 文本
    /// </summary>
    public string Text { get; set; }


    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (targetObject == null)
        {
            var targetHelper = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            targetObject = targetHelper.TargetObject as DependencyObject;
            targetProperty = targetHelper.TargetProperty as DependencyProperty;
        }

        return Text.ToLanguage();
    }

    private void LocalizationManager_CultureChanged(object sender, EventArgs e)
    {
        if (targetObject != null && targetProperty != null)
        {
            var localizedText = Text.ToLanguage();
            targetObject.SetValue(targetProperty, localizedText);
        }
    }
}