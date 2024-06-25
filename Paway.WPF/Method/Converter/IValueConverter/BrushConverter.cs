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
                var color = brush.Color.ToAlpha(alpha);
                return color.ToBrush();
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
                var dLight = parameter.ToDouble();
                if (dLight > 0 && dLight < 1) return brush.Color.AddLight(dLight).ToBrush();
                var iLight = parameter.ToInt();
                if (iLight == 0) iLight = 30;
                return brush.Color.AddLight(iLight).ToBrush();
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
            return (value as Brush).ToColor();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
