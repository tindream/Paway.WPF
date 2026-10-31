using System;
using System.Collections.Generic;
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
    /// 树内边距转换器
    /// </summary>
    internal class TreeIndentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            const double colunwidth = 10;
            var left = 0.0;

            //容器生成初期 value 可能为 null 或 UnsetValue，原实现在此处必 NRE（TreeView 模板高频路径）
            DependencyObject element = value as DependencyObject;
            //从父级开始累计，起始项自身不计入缩进（与原逻辑一致）
            if (element != null) element = VisualTreeHelper.GetParent(element);
            while (element != null && !(element is TreeView))
            {
                if (element is TreeViewItem) left += colunwidth;
                element = VisualTreeHelper.GetParent(element);
            }
            return new Thickness(left, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
