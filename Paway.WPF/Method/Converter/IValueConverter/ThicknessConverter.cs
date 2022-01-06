using Paway.Helper;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 无下边线
    /// </summary>
    internal class NoBottomThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
            {
                return new Thickness(thickness.Left, thickness.Top, thickness.Right, 0);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
