using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// bool判断多选择器(true:选择1，false:选择2)
    /// </summary>
    internal class BoolSelector : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Length == 3)
            {
                var result = value[1];
                if (value[0] == null || value[0] == DependencyProperty.UnsetValue || (value[0] is bool b && !b))
                    return value[2];
                if (targetType.Name == nameof(Thickness) && !(result is Thickness))
                {
                    return new Thickness(result.ToDouble());
                }
                return result;
            }
            throw new WarningException("参数错误");
        }
        /// <summary>
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// bool值反转
    /// </summary>
    internal class BoolReverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b)
                return false;
            else
                return true;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }

    /// <summary>
    /// bool转Visibility
    /// </summary>
    internal class BoolToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value is bool b && b;
            if (parameter != null || (parameter is bool p && !p)) result = !result;
            return result ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value is Visibility visibility && visibility == Visibility.Visible;
            if (parameter != null || (parameter is bool p && !p)) result = !result;
            return result;
        }
    }
    /// <summary>
    /// bool转Collapsed
    /// </summary>
    internal class BoolToCollapsed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value is bool b && b;
            if (parameter != null || (parameter is bool p && !p)) result = !result;
            return result ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value is Visibility visibility && visibility == Visibility.Collapsed;
            if (parameter != null || (parameter is bool p && !p)) result = !result;
            return result;
        }
    }
}
