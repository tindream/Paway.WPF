﻿using Microsoft.Win32;
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
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Config.LanguageList.Clear();
            var directoryInfo = new System.IO.DirectoryInfo(Config.LanguagePath);
            var fileInfos = directoryInfo.GetFiles("*.xml");
            foreach (var fileInfo in fileInfos) Config.LanguageList.Add(System.IO.Path.GetFileNameWithoutExtension(fileInfo.Name));

            PMethod.LanguageMenuBinding(menu, Config.LanguageList, ViewModelLocator.Default.Test, ViewModelLocator.Default.Test.ItemClickCommand, nameof(ViewModelLocator.Default.Test.Language));
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
            frame.Content = ViewlLocator.GetInstance<Test3DPage>();
        }
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            //Method.WaterAdorner(e);
            base.OnPreviewMouseDown(e);
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            //if (this.Content is Panel panel)
            //{
            //    Method.WaterAdorner(e, maxWidth: 100);
            //    Method.WaterAdornerFixed(panel, e);
            //}
            base.OnPreviewMouseMove(e);
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            frame.Content = ViewlLocator.GetInstance<LoginPage>();
        }
    }
}
