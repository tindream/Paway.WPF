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
            var pg = new PathGeometry();
            //设置矩形区域大小
            var rg = new RectangleGeometry();
            for (var i = 0; i < element.ActualHeight / height; i++)
            {
                rg.Rect = new Rect(0, start + i * height, element.ActualWidth, black);
                //合并几何图形
                pg = Geometry.Combine(pg, rg, GeometryCombineMode.Union, null);

            }
            element.Clip = pg;
        }
    }
}
