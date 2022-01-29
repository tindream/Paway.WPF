using Microsoft.Win32;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Paway.Test
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : WindowEXT
    {
        public TestWindow()
        {
            Config.Window = this;
            InitializeComponent();
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            //canvas.Width = SystemParameters.PrimaryScreenWidth;//得到屏幕整体宽度
            //canvas.Height = SystemParameters.PrimaryScreenHeight;//得到屏幕整体高度

            var pg = new PathGeometry();
            //设置矩形区域大小
            var rg = new RectangleGeometry();
            for (var i = 0; i < p1.ActualHeight / 4; i++)
            {
                rg.Rect = new Rect(0, 1 + i * 4, p1.ActualWidth, 2);
                //合并几何图形
                pg = Geometry.Combine(pg, rg, GeometryCombineMode.Union, null);

            }
            p1.Clip = pg;
        }

        private void ButtonEXT_Click(object sender, RoutedEventArgs e)
        {
            var hatchData = new byte[] { 0x00, 0x00, 0xff, 0xff, 0x00, 0x00, 0xff, 0xff };// Horizontal
            var foreGeometryGroup = new GeometryGroup();
            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                {
                    if ((hatchData[y] & (0x80 >> x)) > 0)
                    {
                        foreGeometryGroup.Children.Add(new RectangleGeometry(new Rect(x, y, 1, 1)));
                    }
                }
            }
            var drawingBrush = new DrawingBrush
            {
                Viewport = new Rect(0, 0, 8, 8),
                ViewportUnits = BrushMappingMode.Absolute,
                Stretch = Stretch.None,
                TileMode = TileMode.Tile,
                Drawing = new DrawingGroup
                {
                    Children =
                    {
                        new GeometryDrawing
                        {
                            Brush = new SolidColorBrush(Colors.Transparent),
                            Geometry = new RectangleGeometry(new Rect(0, 0, 8, 8))
                        },
                        new GeometryDrawing
                        {
                            Brush = new SolidColorBrush(Colors.Black),
                            Geometry = foreGeometryGroup
                        }
                    }
                }
            };
            RenderOptions.SetCachingHint(drawingBrush, CachingHint.Cache);
            b1.Background = drawingBrush;
        }
    }
}
