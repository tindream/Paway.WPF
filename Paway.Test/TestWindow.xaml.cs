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
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            menu.Items.Clear();
            Config.LanguageList.Clear();
            var directoryInfo = new System.IO.DirectoryInfo(Config.LanguagePath);
            var fileInfos = directoryInfo.GetFiles("*.xml");
            foreach (var fileInfo in fileInfos)
            {
                var language = System.IO.Path.GetFileNameWithoutExtension(fileInfo.Name);
                Config.LanguageList.Add(language);
                var menuItem = new MenuItem() { Header = language };
                menuItem.Command = ViewModelLocator.Default.Test.MenuItemClickCommand;
                menuItem.CommandParameter = language;
                {  //实例化绑定对象
                    var isCheckedBinding = new Binding();
                    //设置要绑定源
                    isCheckedBinding.Source = ViewModelLocator.Default.Test;//绑定MainWindow类
                    isCheckedBinding.Path = new PropertyPath(nameof(ViewModelLocator.Default.Test.Language));//绑定MainWindow类下的Language属性。
                    isCheckedBinding.Mode = BindingMode.TwoWay;//绑定模式双向绑定
                    isCheckedBinding.Converter = Config.Window.FindResource("valueToTrue") as IValueConverter;
                    isCheckedBinding.ConverterParameter = language;
                    menuItem.SetBinding(MenuItem.IsCheckedProperty, isCheckedBinding);//设置绑定到要绑定的控件
                }
                menu.Items.Add(menuItem);
            }
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
            if (this.Content is Panel panel)
            {
                //Method.WaterAdorner(e, maxWidth: 100);
                //Method.WaterAdornerFixed(panel, e);
            }
            base.OnPreviewMouseMove(e);
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            Method.ShowWindow(this, new ThemeWindow());
        }
    }
}
