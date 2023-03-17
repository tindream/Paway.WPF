using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Paway.WPF
{
    /// <summary>
    /// 格式化
    /// <para>StringFormat只能对字符串进行操作，而有些Content是Object类型的，但ContentStringFormat也没生效</para>
    /// </summary>
    [ValueConversion(typeof(object), typeof(string))]
    internal class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, DependencyProperty.UnsetValue)) return DependencyProperty.UnsetValue;
            return string.Format(culture, (string)parameter, value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
