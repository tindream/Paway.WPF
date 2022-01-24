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
        }

        private void ButtonEXT_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = $"选择要导入的 图标 文件",
                Filter = "JPG图像|*.jpg;*.jpeg",
            };
            if (ofd.ShowDialog() == true)
            {
                var file = ofd.FileName;
                var imageConverter = new ImageSourceConverter();
                var value = (ImageSource)imageConverter.ConvertFrom(null, CultureInfo.InvariantCulture, file);
                //image.Source = value;// new BitmapImage(new Uri(ofd.FileName));
            }
        }

        private FrameworkElement current;
        private int reverse;
        private Point lastPoint;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (current == null && PMethod.Child(canvas, out Border border))
            {
                var location = e.GetPosition(border);
                if (location.X > 0 && location.X < border.ActualWidth && location.Y > 0 && location.Y < border.ActualHeight)
                {
                    this.Cursor = Cursors.Hand;
                    this.current = border;
                }
            }
            if (current != null)
            {
                lastPoint = e.GetPosition(canvas);
                this.current.CaptureMouse();
                e.Handled = true;
            }
            base.OnMouseLeftButtonDown(e);
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (this.current != null)
            {
                this.current.ReleaseMouseCapture();
                this.current = null;
            }
            this.Cursor = Cursors.Arrow;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed && current != null)
            {
                var movePoint = e.GetPosition(canvas);
                if (this.Cursor == Cursors.SizeNWSE)
                {
                    AddX(ref movePoint);
                    AddY(ref movePoint);
                }
                else if (this.Cursor == Cursors.SizeWE)
                {
                    AddX(ref movePoint);
                }
                else if (this.Cursor == Cursors.SizeNS)
                {
                    AddY(ref movePoint);
                }
                else if (this.Cursor == Cursors.Hand)
                {
                    MoveXY(ref movePoint);
                }
                lastPoint = movePoint;
            }
            else
            {
                current = CalcLocation(e);
            }
        }
        private void MoveXY(ref Point move)
        {
            var left = Canvas.GetLeft(current);
            var toLeft = left + move.X - lastPoint.X;
            if (toLeft < 0)
            {
                toLeft = 0;
                if (move.X < 0) move.X = 0;
            }
            else if (toLeft > canvas.ActualWidth - current.Width)
            {
                toLeft = canvas.ActualWidth - current.Width;
                if (move.X > canvas.ActualWidth) move.X = canvas.ActualWidth;
            }
            Canvas.SetLeft(current, toLeft);

            var top = Canvas.GetTop(current);
            var toTop = top + move.Y - lastPoint.Y;
            if (toTop < 0)
            {
                toTop = 0;
                if (move.Y < 0) move.Y = 0;
            }
            else if (toTop > canvas.ActualHeight - current.Height)
            {
                toTop = canvas.ActualHeight - current.Height;
                if (move.Y > canvas.ActualHeight) move.Y = canvas.ActualHeight;
            }
            Canvas.SetTop(current, toTop);
        }
        private void AddX(ref Point move)
        {
            var left = Canvas.GetLeft(current);
            if (reverse == -1)
            {
                var toLeft = left + move.X - lastPoint.X;
                if (toLeft < 0)
                {
                    toLeft = 0;
                    move.X = toLeft + lastPoint.X - left;
                }
                else if (toLeft > left + current.Width - 20)
                {
                    toLeft = left + current.Width - 20;
                    move.X = toLeft + lastPoint.X - left;
                }
                var width = current.Width + reverse * (toLeft - left);
                current.Width = width;
                Canvas.SetLeft(current, toLeft);
            }
            else
            {
                var widthAdd = move.X - lastPoint.X;
                var width = current.Width + widthAdd;
                if (left + current.Width + widthAdd > canvas.ActualWidth)
                {
                    width = canvas.ActualWidth - left;
                    move.X = lastPoint.X - width + current.Width;
                }
                if (width < 20)
                {
                    width = 20;
                    move.X = lastPoint.X - width + current.Width;
                }
                current.Width = width;
            }
        }
        private void AddY(ref Point move)
        {
            var top = Canvas.GetTop(current);
            if (reverse == -1)
            {
                var toTop = top + move.Y - lastPoint.Y;
                if (toTop < 0)
                {
                    toTop = 0;
                    move.Y = toTop + lastPoint.Y - top;
                }
                else if (toTop > top + current.Height - 20)
                {
                    toTop = top + current.Height - 20;
                    move.Y = toTop + lastPoint.Y - top;
                }
                var height = current.Height + reverse * (toTop - top);
                current.Height = height;
                Canvas.SetTop(current, toTop);
            }
            else
            {
                var heightAdd = move.Y - lastPoint.Y;
                var height = current.Height + heightAdd;
                if (top + current.Height + heightAdd > canvas.ActualHeight)
                {
                    height = canvas.ActualHeight - top;
                    move.Y = lastPoint.Y - height + current.Height;
                }
                if (height < 20)
                {
                    height = 20;
                    move.Y = lastPoint.Y - height + current.Height;
                }
                current.Height = height;
            }
        }
        private FrameworkElement CalcLocation(MouseEventArgs e)
        {
            if (PMethod.Child(canvas, out Border border))
            {
                var location = e.GetPosition(border);
                reverse = 1;
                if (Math.Abs(location.X) < 5)
                {
                    reverse = -1;
                    if (Math.Abs(location.Y) < 5)
                    {
                        this.Cursor = Cursors.SizeNWSE;
                        return border;
                    }
                    else if (Math.Abs(location.Y - border.ActualHeight) < 5)
                    {
                        this.Cursor = Cursors.SizeNESW;
                        return border;
                    }
                    else if (location.Y > 0 && location.Y < border.ActualHeight)
                    {
                        this.Cursor = Cursors.SizeWE;
                        return border;
                    }
                }
                else if (Math.Abs(location.Y) < 5)
                {
                    if (Math.Abs(location.X - border.ActualWidth) < 5)
                    {
                        this.Cursor = Cursors.SizeNESW;
                        return border;
                    }
                    else if (location.X > 0 && location.X < border.ActualWidth)
                    {
                        reverse = -1;
                        this.Cursor = Cursors.SizeNS;
                        return border;
                    }
                }
                else if (Math.Abs(location.X - border.ActualWidth) < 5)
                {
                    if (Math.Abs(location.Y - border.ActualHeight) < 5)
                    {
                        this.Cursor = Cursors.SizeNWSE;
                        return border;
                    }
                    else if (location.Y > 0 && location.Y < border.ActualHeight)
                    {
                        this.Cursor = Cursors.SizeWE;
                        return border;
                    }
                }
                else if (Math.Abs(location.Y - border.ActualHeight) < 5)
                {
                    if (location.X > 0 && location.X < border.ActualWidth)
                    {
                        this.Cursor = Cursors.SizeNS;
                        return border;
                    }
                }
                this.Cursor = Cursors.Arrow;
            }
            return null;
        }
    }
}
