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
    /// 枚举描述转换
    /// </summary>
    internal class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum e)
            {
                return e.Description();
            }
            throw new NotImplementedException();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return targetType.ParseObj(value);
        }
    }
}
