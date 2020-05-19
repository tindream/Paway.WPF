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
    /// 多字段空值判断转换
    /// </summary>
    public class NullJudgeConverter : IMultiValueConverter
    {
        /// <summary>
        /// 多字段空值判断转换
        /// </summary>
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = 0;
            while (value.Length > index && value[index] == DependencyProperty.UnsetValue)
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
            throw new Exception("未写转化器反向转换。[Mode=TwoWay]");
        }
    }
}
