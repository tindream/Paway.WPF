using Paway.Helper;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 缩小圆角(-1)
    /// </summary>
    internal class LessRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius radius)
            {
                return new CornerRadius(Math.Max(0, radius.TopLeft - 1), Math.Max(0, radius.TopRight - 1), Math.Max(0, radius.BottomRight - 1), Math.Max(0, radius.BottomLeft - 1));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    /// <summary>
    /// 无下边圆角(左下、右下)
    /// </summary>
    internal class NoBottomRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius radius)
            {
                return new CornerRadius(radius.TopLeft, radius.TopRight, 0, 0);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
