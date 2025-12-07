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
            return value?.ToString().ToLanguage();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
