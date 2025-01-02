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
                if (gridCell.DataContext == null || gridCell.DataContext.GetType() == DependencyProperty.UnsetValue.GetType())
                {
                    return null;
                }
                if (gridCell.Column == null) return null;
                string proName = null;
                if (gridCell.Column.ClipboardContentBinding is Binding binding)
                {
                    proName = binding.Path.Path;
                }
                else if (gridCell.Column.Header is FrameworkElement obj)
                {
                    proName = obj.Name;
                }
                else if (gridCell.Column.Header is string header)
                {
                    proName = header;
                }
                if (proName != null)
                {
                    var pro = gridCell.DataContext.Property(proName);
                    return pro?.GetValue(gridCell.DataContext);
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
