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
