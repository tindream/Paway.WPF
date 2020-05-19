using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    public partial class ButtonRound : Button
    {
        #region 属性定义
        #region 前景
        public static readonly DependencyProperty RadiusProperty =
                DependencyProperty.RegisterAttached("Radius", typeof(CornerRadius), typeof(ButtonRound), new PropertyMetadata(new CornerRadius(7)));
        public static readonly DependencyProperty EffectRadiusProperty =
            DependencyProperty.RegisterAttached("EffectRadius", typeof(double), typeof(ButtonRound), new PropertyMetadata(0d));
        public static readonly DependencyProperty MouseForegroundProperty =
            DependencyProperty.RegisterAttached("MouseForeground", typeof(Brush), typeof(ButtonRound),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 33, 33, 33))));
        public static readonly DependencyProperty PressedForegroundProperty =
            DependencyProperty.RegisterAttached("PressedForeground", typeof(Brush), typeof(ButtonRound),
            new PropertyMetadata(new SolidColorBrush(Colors.White)));

        #endregion
        #region 背景颜色
        public static readonly DependencyProperty NormalBackgroundStartProperty =
            DependencyProperty.RegisterAttached("NormalBackgroundStart", typeof(Color), typeof(ButtonRound),
            new PropertyMetadata(Color.FromArgb(254, 254, 254, 254)));
        public static readonly DependencyProperty NormalBackgroundEndProperty =
            DependencyProperty.RegisterAttached("NormalBackgroundEnd", typeof(Color), typeof(ButtonRound),
            new PropertyMetadata(Color.FromArgb(254, 220, 220, 220)));
        public static readonly DependencyProperty MouseBackgroundStartProperty =
            DependencyProperty.RegisterAttached("MouseBackgroundStart", typeof(Color), typeof(ButtonRound),
            new PropertyMetadata(Color.FromArgb(16, 35, 175, 255)));
        public static readonly DependencyProperty MouseBackgroundEndProperty =
            DependencyProperty.RegisterAttached("MouseBackgroundEnd", typeof(Color), typeof(ButtonRound),
            new PropertyMetadata(Color.FromArgb(211, 35, 175, 255)));
        public static readonly DependencyProperty PressedBackgroundStartProperty =
            DependencyProperty.RegisterAttached("PressedBackgroundStart", typeof(Color), typeof(ButtonRound),
            new PropertyMetadata(Color.FromArgb(65, 35, 175, 255)));
        public static readonly DependencyProperty PressedBackgroundEndProperty =
            DependencyProperty.RegisterAttached("PressedBackgroundEnd", typeof(Color), typeof(ButtonRound),
            new PropertyMetadata(Color.FromArgb(255, 35, 175, 255)));

        #endregion
        #region 背景图片
        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.RegisterAttached("Viewport", typeof(Rect), typeof(ButtonRound),
            new PropertyMetadata(new Rect(0, 0, 1, 1)));
        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.RegisterAttached("BackgroundImage", typeof(ImageSource), typeof(ButtonRound));
        public static readonly DependencyProperty BackgroundFocusedImageProperty =
            DependencyProperty.RegisterAttached("BackgroundFocusedImage", typeof(ImageSource), typeof(ButtonRound));
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.RegisterAttached("Stretch", typeof(Stretch), typeof(ButtonRound),
            new PropertyMetadata(Stretch.Fill));

        #endregion

        #endregion

        #region 属性
        #region 前景
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        [Category("扩展")]
        [Description("自定义边框圆角阴影宽度")]
        public double EffectRadius
        {
            get { return (double)GetValue(EffectRadiusProperty); }
            set { SetValue(EffectRadiusProperty, value); }
        }
        [Category("扩展")]
        [Description("鼠标划过时的文本颜色")]
        public Brush MouseForeground
        {
            get { return (Brush)GetValue(MouseForegroundProperty); }
            set { SetValue(MouseForegroundProperty, value); }
        }
        [Category("扩展")]
        [Description("鼠标点击时的文本颜色")]
        public Brush PressedForeground
        {
            get { return (Brush)GetValue(PressedForegroundProperty); }
            set { SetValue(PressedForegroundProperty, value); }
        }

        #endregion
        #region 背景颜色
        [Category("扩展")]
        [Description("默认的背景颜色(起始颜色)")]
        public Color NormalBackgroundStart
        {
            get { return (Color)GetValue(NormalBackgroundStartProperty); }
            set { SetValue(NormalBackgroundStartProperty, value); }
        }
        [Category("扩展")]
        [Description("默认的背景颜色")]
        public Color NormalBackgroundEnd
        {
            get { return (Color)GetValue(NormalBackgroundEndProperty); }
            set { SetValue(NormalBackgroundEndProperty, value); }
        }
        [Category("扩展")]
        [Description("鼠标划过时的背景颜色(起始颜色)")]
        public Color MouseBackgroundStart
        {
            get { return (Color)GetValue(MouseBackgroundStartProperty); }
            set { SetValue(MouseBackgroundStartProperty, value); }
        }
        [Category("扩展")]
        [Description("鼠标划过时的背景颜色")]
        public Color MouseBackgroundEnd
        {
            get { return (Color)GetValue(MouseBackgroundEndProperty); }
            set { SetValue(MouseBackgroundEndProperty, value); }
        }
        [Category("扩展")]
        [Description("鼠标点击时的背景颜色(起始颜色)")]
        public Color PressedBackgroundStart
        {
            get { return (Color)GetValue(PressedBackgroundStartProperty); }
            set { SetValue(PressedBackgroundStartProperty, value); }
        }
        [Category("扩展")]
        [Description("鼠标点击时的背景颜色")]
        public Color PressedBackgroundEnd
        {
            get { return (Color)GetValue(PressedBackgroundEndProperty); }
            set { SetValue(PressedBackgroundEndProperty, value); }
        }

        #endregion
        #region 背景图片
        [Category("扩展")]
        [Description("背景图片位置")]
        public Rect Viewport
        {
            get { return (Rect)GetValue(ViewportProperty); }
            set { SetValue(ViewportProperty, value); }
        }
        [Category("扩展")]
        [Description("默认的背景图片")]
        public ImageSource BackgroundImage
        {
            get { return (ImageSource)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }
        [Category("扩展")]
        [Description("获取焦点时的背景图片")]
        public ImageSource BackgroundFocusedImage
        {
            get { return (ImageSource)GetValue(BackgroundFocusedImageProperty); }
            set { SetValue(BackgroundFocusedImageProperty, value); }
        }
        [Category("扩展")]
        [Description("背景图片的内容如何拉伸才适合其磁贴")]
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(BackgroundFocusedImageProperty); }
            set { SetValue(BackgroundFocusedImageProperty, value); }
        }

        #endregion

        #endregion

        public ButtonRound()
        {
            this.Loaded += ButtonRound_Loaded;
        }
        private void ButtonRound_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.BackgroundImage != null)
            {
                if (this.BackgroundFocusedImage == null)
                {
                    this.BackgroundFocusedImage = this.BackgroundImage;
                }
                if (this.NormalBackgroundStart == Color.FromArgb(254, 254, 254, 254))
                {
                    this.NormalBackgroundStart = Colors.Transparent;
                    this.NormalBackgroundEnd = Colors.Transparent;
                    this.MouseBackgroundStart = Colors.Transparent;
                    this.MouseBackgroundEnd = Colors.Transparent;
                    this.PressedBackgroundStart = Colors.Transparent;
                    this.PressedBackgroundEnd = Colors.Transparent;
                }
            }
            if (this.NormalBackgroundStart != Color.FromArgb(254, 254, 254, 254) && this.NormalBackgroundStart != Colors.Transparent)
            {
                this.PressedBackgroundStart = Color.FromArgb(255, this.NormalBackgroundStart.R, this.NormalBackgroundStart.G, this.NormalBackgroundStart.B);
                if (this.MouseBackgroundStart == Color.FromArgb(16, 35, 175, 255))
                {
                    this.MouseBackgroundStart = Color.FromArgb((byte)(this.NormalBackgroundStart.A + (255 - this.NormalBackgroundStart.A) / 2), this.NormalBackgroundStart.R, this.NormalBackgroundStart.G, this.NormalBackgroundStart.B);
                }
                else if (this.MouseBackgroundStart != Colors.Transparent)
                {
                    this.PressedBackgroundStart = Color.FromArgb(255, this.MouseBackgroundStart.R, this.MouseBackgroundStart.G, this.MouseBackgroundStart.B);
                }
            }
            if (this.NormalBackgroundEnd != Color.FromArgb(254, 220, 220, 220) && this.NormalBackgroundEnd != Colors.Transparent)
            {
                this.PressedBackgroundEnd = Color.FromArgb(255, this.NormalBackgroundEnd.R, this.NormalBackgroundEnd.G, this.NormalBackgroundEnd.B);
                if (this.MouseBackgroundEnd == Color.FromArgb(211, 35, 175, 255))
                {
                    this.MouseBackgroundEnd = Color.FromArgb((byte)(this.NormalBackgroundEnd.A + (255 - this.NormalBackgroundEnd.A) / 2), this.NormalBackgroundEnd.R, this.NormalBackgroundEnd.G, this.NormalBackgroundEnd.B);
                }
                else if (this.MouseBackgroundEnd != Colors.Transparent)
                {
                    this.PressedBackgroundEnd = Color.FromArgb(255, this.MouseBackgroundEnd.R, this.MouseBackgroundEnd.G, this.MouseBackgroundEnd.B);
                }
            }
        }
    }
}
