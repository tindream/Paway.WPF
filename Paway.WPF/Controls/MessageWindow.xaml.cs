using Paway.Helper;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWindow : Window
    {
        /// <summary>
        /// 窗体消息
        /// </summary>
        public MessageWindow()
        {
            InitializeComponent();
        }

        private static MessageWindow window;
        private FrameworkElement parentElement;
        private Window parentWindow;
        private ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        /// <summary>
        /// 动画显示消息
        /// </summary>
        public static void Show(FrameworkElement element, string msg, LevelType level = LevelType.Debug, int timeout = 3, double fontSize = 15, Action<TextBlock> action = null)
        {
            if (window == null) window = new MessageWindow();
            else
            {
                window.manualResetEvent.Set();
                window = new MessageWindow();
            }
            window.parentElement = element;
            window.textBlock.Text = msg;
            window.textBlock.FontSize = fontSize;
            var type = ColorType.Color;
            switch (level)
            {
                default:
                case LevelType.Debug: break;
                case LevelType.Warn: type = ColorType.Warn; break;
                case LevelType.Error: type = ColorType.Error; break;
            }
            if (type == ColorType.Color)
            {
                window.textBlock.Foreground = type.Color().ToBrush();
                window.textBlock.Background = Colors.White.ToBrush();
            }
            else
            {
                window.textBlock.Foreground = Colors.White.ToBrush();
                window.textBlock.Background = type.Color().ToBrush();
            }
            action?.Invoke(window.textBlock);
            //window.manualResetEvent.Reset();
            window.LoadParent(element, timeout);
            window.Show();
            window.textBlock.Margin = new Thickness(0);
            {
                var storyboard = PMethod.HitStoryboard(window, Colors.Transparent, 3000);
                storyboard.Begin(window, true);
            }
        }
        private void LoadParent(FrameworkElement element, int timeout)
        {
            var point = element.PointToScreen(new Point(0, 0));
            this.Left = point.X + (element.ActualWidth - this.Width) / 2;
            this.Top = point.Y + (element.ActualHeight - this.Height) / 2;
            if (PMethod.Parent(element, out Window parentWindow))
            {
                this.parentWindow = parentWindow;
                parentWindow.Closing += ParentWindow_Closing;
                parentWindow.LocationChanged += ParentWindow_LocationChanged;
                parentWindow.SizeChanged += ParentWindow_LocationChanged;
            }
            Task.Run(() =>
            {
                var result = false;
                for (var i = 0; i < timeout; i++) result = manualResetEvent.WaitOne(1000);
                PMethod.Invoke(() => { this.ToClose(); });
            });
        }
        private void ToClose()
        {
            parentWindow.Closing -= ParentWindow_Closing;
            parentWindow.LocationChanged -= ParentWindow_LocationChanged;
            parentWindow.SizeChanged -= ParentWindow_LocationChanged;
            this.Close();
        }
        private void ParentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.ToClose();
        }
        private void ParentWindow_LocationChanged(object sender, EventArgs e)
        {
            var point = parentElement.PointToScreen(new Point(0, 0));
            this.Left = point.X + (parentElement.ActualWidth - this.Width) / 2;
            this.Top = point.Y + (parentElement.ActualHeight - this.Height) / 2;
        }
    }
}
