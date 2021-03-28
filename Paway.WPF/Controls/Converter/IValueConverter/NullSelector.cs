using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
                (value[index] is SolidColorBrush solid && solid.Color == Colors.Transparent)))
            {
                index++;
            }
            if (value.Length > index) return value[index];
            else return null;
        }
        /// <summary>
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
