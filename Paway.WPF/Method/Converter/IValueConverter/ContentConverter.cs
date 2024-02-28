using Paway.Helper;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 从内容中获取Text
    /// <para>从DataGridCell中获取绑定列的值</para>
    /// </summary>
    internal class ContentTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataGridCell gridCell)
            {
                if (gridCell.Column.ClipboardContentBinding is Binding binding)
                {
                    return gridCell.DataContext.GetValue(binding.Path.Path);
                }
                if (gridCell.Column.Header is FrameworkElement obj)
                {
                    return gridCell.DataContext.GetValue(obj.Name);
                }
                if (gridCell.Column.Header is string header)
                {
                    return gridCell.DataContext.GetValue(header);
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
