using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

namespace Paway.WPF
{
    /// <summary>
    /// PImage.xaml 的交互逻辑
    /// </summary>
    public partial class PImage
    {
        #region 扩展参数
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ICloseProperty =
            DependencyProperty.Register(nameof(IClose), typeof(bool), typeof(PImage));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(PImage), new PropertyMetadata(null, OnTitleChanged));
        private static void OnTitleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is PImage view)
            {
                view.tbTitle.Text = view.Title;
            }
        }
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(BitmapSource), typeof(PImage), new PropertyMetadata(null, OnSourceChanged));
        private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is PImage view)
            {
                view.image.Source = view.Source;
                view.bitmap = view.Source.ToBitmap();
                view.Init();
            }
        }
        /// <summary>
        /// 显示关闭按钮
        /// </summary>
        [Category("扩展")]
        [Description("显示关闭按钮")]
        public bool IClose
        {
            get { return (bool)GetValue(ICloseProperty); }
            set { SetValue(ICloseProperty, value); }
        }
        /// <summary>
        /// 获取或设置标题
        /// </summary>
        [Category("扩展")]
        [Description("获取或设置标题")]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        /// <summary>
        /// 获取或设置图像
        /// </summary>
        [Category("扩展")]
        [Description("获取或设置图像")]
        public BitmapSource Source
        {
            get { return (BitmapSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IPointProperty =
            DependencyProperty.Register(nameof(IPoint), typeof(bool), typeof(PImage), new PropertyMetadata(true));
        /// <summary>
        /// 显示坐标
        /// </summary>
        [Category("扩展")]
        [Description("显示坐标")]
        public bool IPoint
        {
            get { return (bool)GetValue(IPointProperty); }
            set { SetValue(IPointProperty, value); }
        }
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IResetProperty =
            DependencyProperty.Register(nameof(IReset), typeof(bool), typeof(PImage), new PropertyMetadata(false));
        /// <summary>
        /// 显示重置按钮
        /// </summary>
        [Category("扩展")]
        [Description("显示重置按钮")]
        public bool IReset
        {
            get { return (bool)GetValue(IResetProperty); }
            set { SetValue(IResetProperty, value); }
        }
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ISaveProperty =
            DependencyProperty.Register(nameof(ISave), typeof(bool), typeof(PImage), new PropertyMetadata(false));
        /// <summary>
        /// 显示保存按钮
        /// </summary>
        [Category("扩展")]
        [Description("显示保存按钮")]
        public bool ISave
        {
            get { return (bool)GetValue(ISaveProperty); }
            set { SetValue(ISaveProperty, value); }
        }

        /// <summary>
        /// 关闭路由事件
        /// </summary>
        public event EventHandler<RoutedEventArgs> CloseEvent;

        #endregion

        #region 字段与属性
        /// <summary>
        /// 图片显示位置
        /// </summary>
        private Point imagePoint = new Point(0, 0);
        /// <summary>
        /// 图片显示大小
        /// </summary>
        private Size imageSize = new Size(0, 0);
        /// <summary>
        /// 图片宽高比
        /// </summary>
        private double imageRatio;

        /// <summary>
        /// 正在拖动标记
        /// </summary>
        private bool iMoving;
        /// <summary>
        /// 拖拽起点
        /// </summary>
        private Point moveStart = new Point(0, 0);

        /// <summary>
        /// 放大缩小时鼠标位置
        /// </summary>
        private Point zoomPoint = new Point(0, 0);
        /// <summary>
        /// 放大缩小时比例
        /// </summary>
        private double zoomRatioX, zoomRatioY;

        /// <summary>
        /// 双击重置判断 - 位置
        /// </summary>
        private Point clickPoint;
        /// <summary>
        /// 双击重置判断 - 时间
        /// </summary>
        private DateTime clickTime;

        private System.Drawing.Bitmap bitmap;

        #endregion

        /// <summary>
        /// </summary>
        public PImage()
        {
            DefaultStyleKey = typeof(PImage);
            InitializeComponent();
        }
        /// <summary>
        /// </summary>
        public PImage(BitmapSource source, string title = null) : this()
        {
            this.Source = source;
            this.Title = title;
        }

        /// <summary>
        /// 双击重置
        /// </summary>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (DateTime.Now.Subtract(clickTime).TotalMilliseconds < PConfig.DoubleInterval)
            {
                var point = e.GetPosition(this);
                if (clickPoint.X == point.X && clickPoint.Y == point.Y)
                {
                    this.Reset();
                }
                else
                {
                    clickPoint = point;
                    clickTime = DateTime.Now;
                }
            }
            else
            {
                clickPoint = e.GetPosition(this);
                clickTime = DateTime.Now;
            }
            base.OnPreviewMouseDown(e);
        }

        #region Method
        private void Init()
        {
            if (Source == null)
            {
                imagePoint = new Point(0, 0);
                imageSize = new Size(0, 0);
            }
            else if (imagePoint == new Point(0, 0) && imageSize == new Size(0, 0))
            {
                imageRatio = Source.Width * 1.0 / Source.Height;
                imageSize = new Size(Source.Width, Source.Height);
                {
                    var w = ActualWidth * 1.0 / imageSize.Width;
                    var h = ActualHeight * 1.0 / imageSize.Height;
                    if (w > h)
                    {
                        imageSize.Height = ActualHeight;
                        imageSize.Width = h * imageSize.Width;
                    }
                    else
                    {
                        imageSize.Width = ActualWidth;
                        imageSize.Height = w * imageSize.Height;
                    }
                }
                imagePoint = new Point((ActualWidth - imageSize.Width) / 2, (ActualHeight - imageSize.Height) / 2);
            }
            this.LoadImage();
        }
        private void LoadImage()
        {
            this.image.Width = imageSize.Width;
            this.image.Height = imageSize.Height;
            Canvas.SetLeft(image, imagePoint.X);
            Canvas.SetTop(image, imagePoint.Y);
        }
        /// <summary>
        /// 放大或缩小
        /// </summary>
        /// <param name="steps">鼠标滚轮步进数</param>
        private void Zoom(int steps)
        {
            if (Source == null) return;
            double bit = 1;
            for (var i = 0; i < Math.Abs(steps); i++)
            {
                bit *= 1.2;
            }
            if (steps > 0)
            {
                var width = imageSize.Width * bit;
                var height = imageSize.Height * bit;
                if (width / imageSize.Width > 16.0)
                {
                    width = imageSize.Width * 16;
                    height = imageSize.Height * 16;
                }
                if (imageSize.Width == width) return;
                imageSize.Width = width;
                imageSize.Height = height;
            }
            else
            {
                var width = imageSize.Width / bit;
                var height = imageSize.Height / bit;
                if (imageSize.Width / width > 20.0)
                {
                    width = imageSize.Width / 20;
                    height = imageSize.Height / 20;
                }
                if (imageSize.Width == width) return;
                imageSize.Width = width;
                imageSize.Height = height;
            }
            if (imageSize.Width * 1.0 / imageSize.Width > 0.9 && imageSize.Width * 1.0 / imageSize.Width < 1.1)
            {
                imageSize.Width = imageSize.Width;
                imageSize.Height = imageSize.Height;
            }
            if (imageSize.Width < 3)
            {
                imageSize.Width = 3;
                imageSize.Height = imageSize.Width / imageRatio;
            }
            if (imageSize.Height < 3)
            {
                imageSize.Height = 3;
                imageSize.Width = imageSize.Height * imageRatio;
            }

            imagePoint.X = zoomPoint.X - imageSize.Width * zoomRatioX;
            imagePoint.Y = zoomPoint.Y - imageSize.Height * zoomRatioY;
            this.LoadImage();
        }
        /// <summary>
        /// 移动图片
        /// </summary>
        /// <param name="pMove">鼠标落点</param>
        private void Move(Point pMove)
        {
            var x = imagePoint.X + pMove.X - moveStart.X;
            {
                if (x > ActualWidth) x = ActualWidth;
                if (x < -imageSize.Width) x = -imageSize.Width;
            }
            var y = imagePoint.Y + pMove.Y - moveStart.Y;
            {
                if (y > ActualHeight) y = ActualHeight;
                if (y < -imageSize.Height) y = -imageSize.Height;
            }
            imagePoint.X = x;
            imagePoint.Y = y;
            this.LoadImage();
        }

        /// <summary>
        /// 大小变化时重置
        /// </summary>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            this.Reset();
        }
        /// <summary>
        /// 滚轮滚动时缩放
        /// </summary>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (Source != null)
            {
                zoomPoint = e.GetPosition(this);
                zoomRatioX = (zoomPoint.X - imagePoint.X) * 1.0 / imageSize.Width;
                zoomRatioY = (zoomPoint.Y - imagePoint.Y) * 1.0 / imageSize.Height;
                Zoom(e.Delta / 120);
            }
        }
        /// <summary>
        /// 按键抬起停止移动
        /// </summary>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            iMoving = false;
        }
        /// <summary>
        /// 按键按下开始移动
        /// </summary>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (Source != null && e.LeftButton == MouseButtonState.Pressed)
            {
                iMoving = true;
                moveStart = e.GetPosition(this);
                e.Handled = true;
            }
        }
        /// <summary>
        /// 鼠标移动时移动图像
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            dpDesc.Visibility = Visibility.Visible;
            if (Source != null)
            {
                var point = e.GetPosition(this);
                var normal = GetPoint(point, out bool exist);
                if (exist)
                {
                    var color = bitmap.GetPixel((int)normal.X, (int)normal.Y);
                    tbPoint.Text = $"{(int)normal.X,3}, {(int)normal.Y,3} [{color.R,3},{color.G,3},{color.B,3}]";
                }
                else
                {
                    tbPoint.Text = null;
                }
                if (iMoving)
                {
                    Move(point);
                    moveStart = point;
                }
            }
        }
        /// <summary>
        /// 鼠标离开时清空
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            dpDesc.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region public
        /// <summary>
        /// 获取原图坐标点
        /// </summary>
        public Point GetPoint(Point point, out bool exist)
        {
            exist = false;
            var temp = new Point(0, 0);
            if (Source != null)
            {
                exist = true;
                temp.X = (point.X - imagePoint.X) * Source.Width / imageSize.Width;
                temp.Y = (point.Y - imagePoint.Y) * Source.Width / imageSize.Width;
                if (temp.X < 0)
                {
                    temp.X = 0;
                    exist = false;
                }
                else if (temp.X > Source.Width - 1)
                {
                    temp.X = Source.Width - 1;
                    exist = false;
                }
                if (temp.Y < 0)
                {
                    temp.Y = 0;
                    exist = false;
                }
                else if (temp.Y > Source.Height - 1)
                {
                    temp.Y = Source.Height - 1;
                    exist = false;
                }
            }
            return temp;
        }
        /// <summary>
        /// 获取当前坐标点(原图)
        /// </summary>
        public Point ParsePoint(Point point)
        {
            var temp = new Point(0, 0);
            if (Source != null)
            {
                temp.X = point.X * imageSize.Width / Source.Width + imagePoint.X;
                temp.Y = point.Y * imageSize.Width / Source.Width + imagePoint.Y;
            }
            return temp;
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            imagePoint = new Point(0, 0);
            imageSize = new Size(0, 0);
            this.Init();
        }
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            this.Reset();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (this.Source is BitmapSource bitmapSource)
            {
                if (PMethod.SaveFile($"{DateTime.Now:yyMMddHHMMss}", out string file, "Jpeg|*.jpg;*.jpeg|Png|*.png|Bmp|*.bmp"))
                {
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                        encoder.Save(fileStream);
                    }
                }
            }
        }
        private void BtnCloset_Click(object sender, RoutedEventArgs e)
        {
            CloseEvent?.Invoke(sender, e);
        }

        #endregion
    }
}
