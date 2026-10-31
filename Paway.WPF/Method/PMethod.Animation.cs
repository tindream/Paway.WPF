using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法
    /// </summary>
    public partial class PMethod : TMethod
    {
        /// <summary>
        /// 百叶窗样式
        /// </summary>
        public static void ClipWindowShades(FrameworkElement element, double start = 2, double black = 4, double height = 8)
        {
            if (element == null) return;
            //height 为 0 时 ActualHeight/height 为 +∞，原实现会在UI线程死循环
            if (height <= 0) return;
            var count = (int)(element.ActualHeight / height);
            if (count <= 0)
            {
                element.Clip = null;
                return;
            }
            //原实现每次 Geometry.Combine 都重建整个几何（O(n²)），条形多时明显掉帧；
            //矩形互不重叠，直接用 GeometryGroup 合并，O(n)
            var group = new GeometryGroup();
            var width = element.ActualWidth;
            for (var i = 0; i < count; i++)
            {
                group.Children.Add(new RectangleGeometry(new Rect(0, start + i * height, width, black)));
            }
            element.Clip = group;
        }
    }
}
