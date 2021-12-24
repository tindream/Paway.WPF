using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Paway.WPF
{
    internal class BrushToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DBNull.Value) return Colors.Transparent;
            if (value is SolidColorBrush solid) return solid.Color;
            else if (value is LinearGradientBrush linear) return linear.GradientStops[0].Color;
            else if (value is RadialGradientBrush radial) return radial.GradientStops[0].Color;
            else return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
