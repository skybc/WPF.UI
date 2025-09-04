using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace OhmStudio.UI.Converters
{
    public class AllTrueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.OfType<bool>().All(value => value);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { value };
        }
    }

    public class AllFalseConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.OfType<bool>().All(value => !value);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { value };
        }
    }
}