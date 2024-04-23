using Paway.Helper;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 更新Color中颜色Alpha值
    /// </summary>
    internal class ColorAlphaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                var alpha = parameter.ToInt();
                if (alpha == 0) alpha = PConfig.Alpha;
                return PMethod.AlphaColor(alpha, color);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// 更新Color中颜色亮度值
    /// </summary>
    internal class ColorLightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                var dLight = parameter.ToDouble();
                if (dLight > 0 && dLight < 1) return color.AddLight(dLight);
                var iLight = parameter.ToInt();
                if (iLight == 0) iLight = 30;
                return color.AddLight(iLight);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Color转Brush
    /// </summary>
    internal class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DBNull.Value) return value;
            if (value is Color color) return color.ToBrush();
            return Colors.Transparent.ToBrush();

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
