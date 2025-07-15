using Paway.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paway.WPF
{
    /// <summary>
    /// ImageEXT扩展
    /// </summary>
    public partial class ImageEXT : ContentControl, IWindowAdorner
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(ImageEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(BitmapSource), typeof(ImageEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(ImageEXT), new PropertyMetadata(Stretch.Uniform));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty StretchDirectionProperty =
            DependencyProperty.Register(nameof(StretchDirection), typeof(StretchDirection), typeof(ImageEXT), new PropertyMetadata(StretchDirection.Both));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ICloseProperty =
            DependencyProperty.Register(nameof(IClose), typeof(bool), typeof(ImageEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IMoveProperty =
            DependencyProperty.Register(nameof(IMove), typeof(bool), typeof(ImageEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IZoomProperty =
            DependencyProperty.Register(nameof(IZoom), typeof(bool), typeof(ImageEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IPointProperty =
            DependencyProperty.Register(nameof(IPoint), typeof(bool), typeof(ImageEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IResetProperty =
            DependencyProperty.Register(nameof(IReset), typeof(bool), typeof(ImageEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IDoubleViewProperty =
            DependencyProperty.Register(nameof(IDoubleView), typeof(bool), typeof(ImageEXT), new PropertyMetadata(true));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ISaveProperty =
            DependencyProperty.Register(nameof(ISave), typeof(bool), typeof(ImageEXT));

        /// <summary>
        /// </summary>
        internal static readonly DependencyProperty IShowProperty =
            DependencyProperty.Register(nameof(IShow), typeof(bool), typeof(ImageEXT));
        /// <summary>
        /// </summary>
        internal static readonly DependencyProperty IShowTextProperty =
            DependencyProperty.Register(nameof(IShowText), typeof(string), typeof(ImageEXT));

        #endregion

        #region 扩展
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
        /// 获取或设置图像填充方式
        /// <para>默认值：Uniform</para>
        /// </summary>
        [Category("扩展")]
        [Description("获取或设置图像填充方式")]
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        /// <summary>
        /// 获取或设置图像缩放方向
        /// <para>默认值：Both</para>
        /// </summary>
        [Category("扩展")]
        [Description("获取或设置图像缩放方向")]
        public StretchDirection StretchDirection
        {
            get { return (StretchDirection)GetValue(StretchDirectionProperty); }
            set { SetValue(StretchDirectionProperty, value); }
        }

        /// <summary>
        /// 允许移动
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("允许移动")]
        public bool IMove
        {
            get { return (bool)GetValue(IMoveProperty); }
            set { SetValue(IMoveProperty, value); }
        }
        /// <summary>
        /// 支持缩放
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("支持缩放")]
        public bool IZoom
        {
            get { return (bool)GetValue(IZoomProperty); }
            set { SetValue(IZoomProperty, value); }
        }
        /// <summary>
        /// 显示关闭按钮
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("显示关闭按钮")]
        public bool IClose
        {
            get { return (bool)GetValue(ICloseProperty); }
            set { SetValue(ICloseProperty, value); }
        }
        /// <summary>
        /// 显示坐标
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("显示坐标")]
        public bool IPoint
        {
            get { return (bool)GetValue(IPointProperty); }
            set { SetValue(IPointProperty, value); }
        }
        /// <summary>
        /// 显示重置按钮
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("显示重置按钮")]
        public bool IReset
        {
            get { return (bool)GetValue(IResetProperty); }
            set { SetValue(IResetProperty, value); }
        }
        /// <summary>
        /// 双击显示视图
        /// <para>默认值：true</para>
        /// </summary>
        [Category("扩展")]
        [Description("双击显示视图")]
        public bool IDoubleView
        {
            get { return (bool)GetValue(IDoubleViewProperty); }
            set { SetValue(IDoubleViewProperty, value); }
        }
        /// <summary>
        /// 显示保存按钮
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("显示保存按钮")]
        public bool ISave
        {
            get { return (bool)GetValue(ISaveProperty); }
            set { SetValue(ISaveProperty, value); }
        }

        /// <summary>
        /// 显示操作栏
        /// <para>默认值：false</para>
        /// </summary>
        [Browsable(false)]
        [Category("扩展")]
        [Description("显示操作栏")]
        public bool IShow
        {
            get { return (bool)GetValue(IShowProperty); }
            set { SetValue(IShowProperty, value); }
        }
        /// <summary>
        /// 显示文本
        /// <para>默认值：null</para>
        /// </summary>
        [Browsable(false)]
        [Category("扩展")]
        [Description("显示文本")]
        public string IShowText
        {
            get { return (string)GetValue(IShowTextProperty); }
            set { SetValue(IShowTextProperty, value); }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 行双击路由事件
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> DoubleEvent;
        /// <summary>
        /// 关闭路由事件
        /// </summary>
        public event EventHandler<RoutedEventArgs> CloseEvent;

        #endregion

        #region 字段与属性 - 缩放与移动
        /// <summary>
        /// 图片显示位置
        /// </summary>
        private Point imagePoint = new Point(0, 0);
        /// <summary>
        /// 图片显示位置(默认)
        /// </summary>
        private Point imagePointNormal = new Point(0, 0);
        /// <summary>
        /// 图片显示大小
        /// </summary>
        private Size imageSize = new Size(0, 0);
        /// <summary>
        /// 图片显示大小(默认)
        /// </summary>
        private Size imageSizeNomral = new Size(0, 0);
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

        #endregion

        private Image image;
        /// <summary>
        /// </summary>
        public ImageEXT()
        {
            DefaultStyleKey = typeof(ImageEXT);
            DependencyPropertyDescriptor.FromProperty(SourceProperty, typeof(ImageEXT)).AddValueChanged(this, OnSourceChanged);
        }
        /// <summary>
        /// </summary>
        public ImageEXT(BitmapSource source, string title = null) : this()
        {
            this.Source = source;
            this.Title = title;
        }
        /// <summary>
        /// PImage装饰器显示到Window
        /// </summary>
        public void Adorner(Window window)
        {
            PMethod.PageFullAdorner(window, this);
        }
        private void OnSourceChanged(object sender, EventArgs e)
        {
            if (this.image != null) this.Init();
        }
        /// <summary>
        /// 获取模板控件
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            image = Template.FindName("image", this) as Image;
        }

        #region 响应鼠标事件
        /// <summary>
        /// 双击响应事件与重置 或 显示视图
        /// </summary>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && DateTime.Now.Subtract(clickTime).TotalMilliseconds < PConfig.DoubleInterval)
            {
                if (DoubleEvent != null) DoubleEvent.Invoke(this, e);
                else if (IDoubleView)
                {
                    if (PMethod.Parent(this, out Window window))
                    {
                        var imageEXT = new ImageEXT();
                        imageEXT.Title = this.Title;
                        imageEXT.Source = this.Source;
                        imageEXT.IClose = true;
                        imageEXT.ISave = true;
                        imageEXT.IMove = true;
                        imageEXT.IZoom = true;
                        imageEXT.IPoint = false;
                        imageEXT.IDoubleView = false;
                        PMethod.PageFullAdorner(window, imageEXT);
                    }
                    return;
                }
                if (e.Handled) return;
                var point = e.GetPosition(this);
                if (clickPoint.X == point.X && clickPoint.Y == point.Y)
                {
                    if (this.imagePoint != this.imagePointNormal || this.imageSize != this.imageSizeNomral)
                    {
                        this.Reset();
                        e.Handled = true;
                    }
                }
                else
                {
                    clickPoint = point;
                    clickTime = DateTime.Now;
                }
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                clickPoint = e.GetPosition(this);
                clickTime = DateTime.Now;
            }
            base.OnPreviewMouseDown(e);
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
            if (Source != null && IZoom)
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
            if (IMove && Source != null && e.LeftButton == MouseButtonState.Pressed)
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
            IShow = true;
            if (Source != null && (IMove || IPoint))
            {
                var point = e.GetPosition(this);
                if (IPoint && GetPoint(point, out Point normal))
                {
                    var color = this.Source.PixelColor((int)normal.X, (int)normal.Y);
                    IShowText = $"{(int)normal.X}, {(int)normal.Y}[{color.R},{color.G},{color.B}]";
                }
                else
                {
                    IShowText = null;
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
            IShow = false;
        }

        #endregion
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
                imageRatio = Source.PixelWidth * 1.0 / Source.PixelHeight;
                imageSize = new Size(Source.PixelWidth, Source.PixelHeight);
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
                this.imagePointNormal = imagePoint;
                this.imageSizeNomral = imageSize;
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
                if (width / Source.PixelWidth > 32.0)
                {
                    width = Source.PixelWidth * 32;
                    height = Source.PixelHeight * 32;
                }
                if (imageSize.Width == width) return;
                imageSize.Width = width;
                imageSize.Height = height;
            }
            else
            {
                var width = imageSize.Width / bit;
                var height = imageSize.Height / bit;
                if (Source.PixelWidth / width > 10.0)
                {
                    width = Source.PixelWidth / 10;
                    height = Source.PixelHeight / 10;
                }
                if (imageSize.Width == width) return;
                imageSize.Width = width;
                imageSize.Height = height;
            }
            if (Source.PixelWidth * 1.0 / imageSize.Width > 0.9 && Source.PixelWidth * 1.0 / imageSize.Width < 1.1)
            {
                imageSize.Width = Source.PixelWidth;
                imageSize.Height = Source.PixelHeight;
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

        #endregion
        #region public
        /// <summary>
        /// 获取原图坐标点
        /// </summary>
        public bool GetPoint(Point point, out Point result)
        {
            result = new Point(0, 0);
            if (Source != null)
            {
                var exist = true;
                result.X = (point.X - imagePoint.X) * Source.PixelWidth / imageSize.Width;
                result.Y = (point.Y - imagePoint.Y) * Source.PixelWidth / imageSize.Width;
                if (result.X < 0)
                {
                    result.X = 0;
                    exist = false;
                }
                else if (result.X > Source.PixelWidth - 1)
                {
                    result.X = Source.PixelWidth - 1;
                    exist = false;
                }
                if (result.Y < 0)
                {
                    result.Y = 0;
                    exist = false;
                }
                else if (result.Y > Source.PixelHeight - 1)
                {
                    result.Y = Source.PixelHeight - 1;
                    exist = false;
                }
                return exist;
            }
            return false;
        }
        /// <summary>
        /// 获取当前坐标点(从原图坐标转换)
        /// </summary>
        public Point ParsePoint(Point point)
        {
            var temp = new Point(0, 0);
            if (Source != null)
            {
                temp.X = point.X * imageSize.Width / Source.PixelWidth + imagePoint.X;
                temp.Y = point.Y * imageSize.Width / Source.PixelWidth + imagePoint.Y;
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
        /// <summary>
        /// 点击按钮
        /// </summary>
        public ICommand ButtonClickCommand => new RelayCommand<string>(str =>
        {
            switch (str)
            {
                case "关闭": CloseEvent?.Invoke(this, new RoutedEventArgs()); break;
                case "重置": this.Reset(); break;
                case "保存":
                    if (this.Source is BitmapSource bitmapSource)
                    {
                        if (PMethod.SaveFile(out string file, "另存为", "Jpeg|*.jpg|Png|*.png"))
                        {
                            bitmapSource.Save(file);
                        }
                    }
                    break;
            }
        });

        #endregion
    }
}
