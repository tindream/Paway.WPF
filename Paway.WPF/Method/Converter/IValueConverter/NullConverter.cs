using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 多字段空值判断转换(取排在最前面的非空字段值)
    /// </summary>
    public class NullSelector : IMultiValueConverter
    {
        /// <summary>
        /// 多字段空值判断转换
        /// </summary>
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = 0;
            while (value.Length > index && (value[index] == null || value[index] == DependencyProperty.UnsetValue ||
                (value[index] is Thickness thickness && thickness == new Thickness(double.NaN)) ||
                (value[index] is double dValue && dValue.Equals(double.NaN)) ||
                (value[index] is SolidColorBrush solid && solid.Color == Colors.Transparent)))
            {
                index++;
            }
            if (value.Length > index) return value[index];
            else if (value.Length > 0) return value[value.Length - 1];
            else return null;
        }
        /// <summary>
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// null转Collapsed
    /// </summary>
    internal class NullToCollapsed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value == null || value.Equals(string.Empty) || value == DBNull.Value;
            if (parameter != null || (parameter is bool p && !p)) result = !result;
            return result ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// null转Visible
    /// </summary>
    internal class NullToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value == null || value.Equals(string.Empty) || value == DBNull.Value;
            if (parameter != null || (parameter is bool p && !p)) result = !result;
            return result ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// null转Double(0)
    /// </summary>
    internal class NullToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value as double?;
            return color ?? 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    /// <summary>
    /// null转Color(Transparent)
    /// </summary>
    internal class NullToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value as Color?;
            return color ?? Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
