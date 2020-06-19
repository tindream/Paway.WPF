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
    /// </summary>
    internal class EmptyObjectConverter : DependencyObject, IValueConverter
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty EmptyValueProperty =
            DependencyProperty.Register(nameof(EmptyValue), typeof(Visibility), typeof(EmptyObjectConverter), new PropertyMetadata(Visibility.Collapsed));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty NotEmptyValueProperty =
            DependencyProperty.Register(nameof(NotEmptyValue), typeof(Visibility), typeof(EmptyObjectConverter), new PropertyMetadata(Visibility.Visible));

        #endregion

        /// <summary>
        /// 获取或设置EmptyValue的值
        /// </summary>
        public Visibility EmptyValue
        {
            get => (Visibility)GetValue(EmptyValueProperty);
            set => SetValue(EmptyValueProperty, value);
        }
        /// <summary>
        /// 获取或设置NotEmptyValue的值
        /// </summary>
        public Visibility NotEmptyValue
        {
            get => (Visibility)GetValue(NotEmptyValueProperty);
            set => SetValue(NotEmptyValueProperty, value);
        }

        /// <summary>
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? EmptyValue : NotEmptyValue;
        }
        /// <summary>
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
