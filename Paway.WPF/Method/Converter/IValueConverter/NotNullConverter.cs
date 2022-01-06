using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Paway.WPF
{
    /// <summary>
    /// 非空检查(转换为bool)
    /// </summary>
    internal class IsNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value != null && value != DependencyProperty.UnsetValue;
            if (value is double dValue) result &= dValue != 0;
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 非空检查(多选)(转换为bool)
    /// </summary>
    internal class IsNotNullAllConverter : IMultiValueConverter
    {
        /// <summary>
        /// 多字段非空判断
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (value == null || value == DependencyProperty.UnsetValue) return false;
            }
            return true;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 非空检查(多选)(转换为Visible)
    /// </summary>
    internal class AnyNotNullToVisible : IMultiValueConverter
    {
        /// <summary>
        /// 多字段非空判断
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (value != null && value != DependencyProperty.UnsetValue) return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 非空检查(多选)(转换为Collapsed)
    /// </summary>
    internal class AnyNotNullToCollapsed : IMultiValueConverter
    {
        /// <summary>
        /// 多字段非空判断
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (value != null && value != DependencyProperty.UnsetValue) return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
