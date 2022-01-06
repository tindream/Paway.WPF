using Paway.Helper;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 更新Brush中颜色Alpha值
    /// </summary>
    internal class BrushAlphaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                var alpha = parameter.ToInt();
                if (alpha == 0) alpha = PConfig.Alpha;
                var color = PMethod.AlphaColor(alpha, brush.Color);
                return new SolidColorBrush(color);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// 更新Brush中颜色亮度值
    /// </summary>
    internal class BrushLightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                var light = parameter.ToInt();
                if (light == 0) light = 30;
                return new SolidColorBrush(brush.Color.AddLight(light));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Brush转Color
    /// </summary>
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
