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
    /// 多颜色值判断选择(跳过空、透明)
    /// </summary>
    public class EmptyColorConverter : IMultiValueConverter
    {
        /// <summary>
        /// 多颜色值判断选择(跳过空、透明)
        /// </summary>
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = 0;
            while (value.Length > index && (value[index] == null || value[index] == DependencyProperty.UnsetValue || (value[index] is SolidColorBrush solid && solid.Color == Colors.Transparent)))
            {
                index++;
            }
            if (value.Length > index) return value[index];
            else return value[value.Length - 1];
        }
        /// <summary>
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
