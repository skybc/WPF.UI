using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TestGrowlApp
{
    /// <summary>
    /// Interaction logic for TestSymbolIconForeground.xaml
    /// </summary>
    public partial class TestSymbolIconForeground : Window
    {
        public TestSymbolIconForeground()
        {
            InitializeComponent();
            
            // Add a value converter for the slider to brush conversion
            Resources.Add("BrushConverter", new IntensityToBrushConverter());
        }
    }
    
    /// <summary>
    /// Converts slider value to a red brush with varying intensity
    /// </summary>
    public class IntensityToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double intensity)
            {
                byte alpha = (byte)Math.Max(0, Math.Min(255, intensity));
                return new SolidColorBrush(Color.FromArgb(255, alpha, 0, 0));
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}