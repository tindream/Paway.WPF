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
    internal class BoolToSelector : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Length == 3)
            {
                if (value[0] == null || value[0] == DependencyProperty.UnsetValue || (value[0] is bool b && !b))
                    return value[2];
                else return value[1];
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
}
