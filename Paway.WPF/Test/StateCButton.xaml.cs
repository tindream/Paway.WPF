using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// MKStateCButton.xaml 的交互逻辑
    /// </summary>
    public partial class StateCButton : UserControl
    {
        public static readonly DependencyProperty FocusedBrushProperty =
            DependencyProperty.RegisterAttached("FocusedBrush", typeof(Brush), typeof(StateCButton),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 95, 189, 242))));
        public static readonly DependencyProperty FocusedBrushDownProperty =
            DependencyProperty.RegisterAttached("FocusedBrushDown", typeof(Brush), typeof(StateCButton),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 52, 178, 250))));

        [Category("扩展")]
        [Description("鼠标划过时背景颜色")]
        public Brush FocusedBrush
        {
            get { return (Brush)GetValue(FocusedBrushProperty); }
            set { SetValue(FocusedBrushProperty, value); }
        }
        [Category("扩展")]
        [Description("鼠标点击时背景颜色")]
        public Brush FocusedBrushDown
        {
            get { return (Brush)GetValue(FocusedBrushDownProperty); }
            set { SetValue(FocusedBrushDownProperty, value); }
        }


        public StateCButton(int index)
        {
            InitializeComponent();
            lbText.Text = index.ToString();
            this.Width = 50;
            this.Height = 100;
            this.Margin = new Thickness(400, 125, 0, 0);
            this.RenderTransform = new RotateTransform((index - 1) * 10, 25, 300);
        }

        private void Path_MouseDown(object sender, MouseButtonEventArgs e)
        {
            path.Stroke = FocusedBrushDown;
            path.Fill = FocusedBrushDown;
            lbText.Foreground = new SolidColorBrush(Colors.White);
        }

        private void Path_MouseMove(object sender, MouseEventArgs e)
        {
            path.Stroke = FocusedBrush;
            path.Fill = FocusedBrush;
            lbText.Foreground = new SolidColorBrush(Colors.White);
        }

        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            path.Stroke = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
            path.Fill = new SolidColorBrush(Color.FromArgb(255, 250, 250, 250));
            lbText.Foreground = new SolidColorBrush(Colors.Black);
        }
    }
}
