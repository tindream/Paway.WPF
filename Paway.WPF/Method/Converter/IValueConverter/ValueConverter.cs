using Paway.Helper;
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
    /// 值乘数转换(默认*0.85)
    /// </summary>
    internal class ValueMultiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = value.ToDouble();
            var param = parameter == null ? 0.85 : parameter.ToDouble();
            return temp * param;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = value.ToDouble();
            var param = parameter == null ? 0.85 : parameter.ToDouble();
            return temp / param;
        }
    }
    /// <summary>
    /// 值加减转换(默认-2)
    /// </summary>
    internal class ValueAddConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = value.ToDouble();
            var param = parameter == null ? -2 : parameter.ToDouble();
            return temp + param;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = value.ToDouble();
            var param = parameter == null ? -2 : parameter.ToDouble();
            return temp - param;
        }
    }
}
