using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            InitializeComponent();
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }
        private void ButtonEXT_Click(object sender, RoutedEventArgs e)
        {
            Method.Show(this, new Window() { Foreground = new SolidColorBrush(Colors.White), Title = "Test123" });
        }
    }
}
