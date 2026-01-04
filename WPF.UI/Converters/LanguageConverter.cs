using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace Wpf.Ui.Converters
{
    public class LanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 设计时直接返回原值，不进行转换
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return value?.ToString() ?? string.Empty;
            }
            return value?.ToString().ToLanguage();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
