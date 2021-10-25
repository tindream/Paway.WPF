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
