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
            while (value.Length > index && (value[index] == null ||
                (value[index] == DependencyProperty.UnsetValue) ||
                (value[index] is string str && str.Equals(string.Empty)) ||
                //Thickness 逐字段用 == 比较，NaN == NaN 恒为 false，原写法永远不成立
                (value[index] is Thickness thickness && IsNaN(thickness)) ||
                (value[index] is double dValue && double.IsNaN(dValue)) ||
                (value[index] is SolidColorBrush solid && solid.Color == Colors.Transparent)))
            {
                index++;
            }
            if (value.Length > index) return value[index];
            else if (value.Length > 0) return value[value.Length - 1];
            else return null;
        }
        /// <summary>
        /// 判断是否为未设置的 Thickness（任一分量为 NaN）
        /// </summary>
        private static bool IsNaN(Thickness thickness)
        {
            return double.IsNaN(thickness.Left) || double.IsNaN(thickness.Top) ||
                double.IsNaN(thickness.Right) || double.IsNaN(thickness.Bottom);
        }
        /// <summary>
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
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
    /// null转True
    /// </summary>
    internal class NullToTrue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value == null || value.Equals(string.Empty) || value == DBNull.Value;
            if (parameter != null || (parameter is bool p && !p)) result = !result;
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// null转False
    /// </summary>
    internal class NullToFalse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value == null || value.Equals(string.Empty) || value == DBNull.Value;
            if (parameter != null || (parameter is bool p && !p)) result = !result;
            return !result;
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
