using Microsoft.Win32;
using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Animation;
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

            //var pg = new PathGeometry();
            ////设置矩形区域大小
            //var rg = new RectangleGeometry();
            //for (var i = 0; i < p1.ActualHeight / 4; i++)
            //{
            //    rg.Rect = new Rect(0, 1 + i * 4, p1.ActualWidth, 2);
            //    //合并几何图形
            //    pg = Geometry.Combine(pg, rg, GeometryCombineMode.Union, null);

            //}
            //p1.Clip = pg;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var xml = Method.GetTemplateXaml(paragraph1);
            //Debug.WriteLine(xml);
            frame.Content = ViewlLocator.GetInstance<TestPage>();
        }
    }
}
