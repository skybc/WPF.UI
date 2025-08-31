using System;
using System.Globalization;
using System.Windows.Data;

namespace Wpf.Ui.Controls
{
    /// <summary>
    /// 大于比较转换器 - 用于按钮启用状态判断
    /// Greater than converter - used for button enable state judgment
    /// </summary>
    public class GreaterThanConverter : IValueConverter
    {
    /// <summary>
    /// 比较值
    /// Compare value
    /// </summary>
        public int CompareValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue > CompareValue;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 小于比较转换器 - 用于按钮启用状态判断
    /// Less than converter - used for button enable state judgment
    /// </summary>
    public class LessThanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is int currentPage && values[1] is int totalPages)
            {
                return currentPage < totalPages;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}