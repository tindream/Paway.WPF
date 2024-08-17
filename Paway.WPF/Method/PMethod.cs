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
        #region 绘制 三次贝塞尔曲线，及箭头
        /// <summary>
        /// 计算路径： 三次贝塞尔曲线
        /// </summary>
        /// <param name="startPoint">起点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="arrow">是否添加箭头</param>
        /// <returns></returns>
        public static PathGeometry CubicBezier(Point startPoint, Point endPoint, bool arrow = true)
        {
            PathGeometry geo = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = startPoint;
            pathFigure.Segments.Add(new BezierSegment(CubicBezierControlPoint(startPoint, endPoint, true), CubicBezierControlPoint(startPoint, endPoint, false), endPoint, true));
            geo.Figures.Add(pathFigure);
            if (arrow)
            {
                pathFigure = new PathFigure();
                if (endPoint.X > startPoint.X)
                {
                    pathFigure.StartPoint = new Point(endPoint.X - 7, endPoint.Y - 4);
                    pathFigure.Segments.Add(new PolyLineSegment(new List<Point> { new Point(endPoint.X + 10 - 7, endPoint.Y), new Point(endPoint.X - 7, endPoint.Y + 4) }, true));
                }
                else
                {
                    pathFigure.StartPoint = new Point(endPoint.X + 7, endPoint.Y - 4);
                    pathFigure.Segments.Add(new PolyLineSegment(new List<Point> { new Point(endPoint.X - 10 + 7, endPoint.Y), new Point(endPoint.X + 7, endPoint.Y + 4) }, true));
                }
                geo.Figures.Add(pathFigure);
            }
            return geo;
        }
        /// <summary>
        /// 计算路径范围： 三次贝塞尔曲线上下区域
        /// </summary>
        /// <param name="startPoint">起点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="interval">区域范围</param>
        public static PathGeometry CubicBezierRect(Point startPoint, Point endPoint, double interval = 8)
        {
            PathGeometry geo = new PathGeometry();
            var point1 = new Point(startPoint.X, startPoint.Y - interval);
            var point2 = new Point(endPoint.X, endPoint.Y - interval);
            var point3 = new Point(startPoint.X, startPoint.Y + interval);
            var point4 = new Point(endPoint.X, endPoint.Y + interval);
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = point1;
            //pathFigure.IsClosed = true;
            pathFigure.Segments.Add(new BezierSegment(CubicBezierControlPoint(point1, point2, true), CubicBezierControlPoint(point1, point2, false), point2, true));
            pathFigure.Segments.Add(new PolyLineSegment(new List<Point> { point2, point4 }, true));

            pathFigure.Segments.Add(new PolyLineSegment(new List<Point> { point4, point1 }, true));
            pathFigure.Segments.Add(new PolyLineSegment(new List<Point> { point1, point3 }, true));
            pathFigure.Segments.Add(new BezierSegment(CubicBezierControlPoint(point3, point4, true), CubicBezierControlPoint(point3, point4, false), point4, true));
            geo.Figures.Add(pathFigure);

            return geo;
        }
        /// <summary>
        /// 三次贝塞尔曲线 控制点
        /// </summary>
        /// <param name="startPoint">起点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="isFirstControlPoint">是否第一个控制点</param>
        public static Point CubicBezierControlPoint(Point startPoint, Point endPoint, bool isFirstControlPoint)
        {
            double distanceX = Math.Abs(endPoint.X - startPoint.X);
            double controlPointX = (startPoint.X + endPoint.X) / 2;
            double controlPointY = isFirstControlPoint ? startPoint.Y - distanceX / 12 : endPoint.Y + distanceX / 12;
            return new Point(controlPointX, controlPointY);
        }

        #endregion

        #region Menu
        /// <summary>
        /// Menu菜单绑定多语言
        /// </summary>
        public static void LanguageMenuBinding(MenuItem menu, List<string> languageList, object model, ICommand command, string languageName)
        {
            menu.Items.Clear();
            foreach (var language in languageList)
            {
                var menuItem = new MenuItem() { Header = language };
                menuItem.Command = command;
                menuItem.CommandParameter = language;
                {  //实例化绑定对象
                    var isCheckedBinding = new Binding();
                    //设置要绑定源
                    isCheckedBinding.Source = model;//绑定ViewModel类
                    isCheckedBinding.Path = new PropertyPath(languageName);//绑定MainWindow类下的Language属性。
                    isCheckedBinding.Mode = BindingMode.TwoWay;//绑定模式双向绑定
                    isCheckedBinding.Converter = PConfig.Window.FindResource("valueToTrue") as IValueConverter;
                    isCheckedBinding.ConverterParameter = language;
                    menuItem.SetBinding(MenuItem.IsCheckedProperty, isCheckedBinding);//设置绑定到要绑定的控件
                }
                menu.Items.Add(menuItem);
            }
        }
        #endregion

        #region 颜色
        /// <summary>
        /// 取颜色拾取器中的颜色值
        /// </summary>
        /// <param name="offset"></param>
        public static Color ColorSelector(double offset)
        {
            if (!(Application.Current.FindResource("ColorSelector") is LinearGradientBrush linearGradientBrush)) return Colors.Transparent;

            var stops = linearGradientBrush.GradientStops.ToArray();
            if (offset <= 0) return stops[0].Color;
            if (offset >= 1) return stops[stops.Length - 1].Color;
            var left = stops.Where(s => s.Offset <= offset).Last();
            var right = stops.Where(s => s.Offset > offset).First();
            offset = Math.Round((offset - left.Offset) / (right.Offset - left.Offset), 2);
            var a = (byte)((right.Color.A - left.Color.A) * offset + left.Color.A);
            var r = (byte)((right.Color.R - left.Color.R) * offset + left.Color.R);
            var g = (byte)((right.Color.G - left.Color.G) * offset + left.Color.G);
            var b = (byte)((right.Color.B - left.Color.B) * offset + left.Color.B);
            return Color.FromArgb(a, r, g, b);
        }

        #endregion
    }
}
