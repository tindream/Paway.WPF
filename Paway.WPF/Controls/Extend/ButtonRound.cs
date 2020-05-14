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
        #region 属性
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
            new PropertyMetadata(Color.FromArgb(254, 254, 254, 254), OnNormalBackgroundStart));
        public static readonly DependencyProperty NormalBackgroundEndProperty =
            DependencyProperty.RegisterAttached("NormalBackgroundEnd", typeof(Color), typeof(ButtonRound),
            new PropertyMetadata(Color.FromArgb(254, 220, 220, 220), OnNormalBackgroundEnd));
        public static readonly DependencyProperty MouseBackgroundStartProperty =
            DependencyProperty.RegisterAttached("MouseBackgroundStart", typeof(Color), typeof(ButtonRound),
            new PropertyMetadata(Color.FromArgb(15, 35, 175, 255), OnMouseBackgroundStart));
        public static readonly DependencyProperty MouseBackgroundEndProperty =
            DependencyProperty.RegisterAttached("MouseBackgroundEnd", typeof(Color), typeof(ButtonRound),
            new PropertyMetadata(Color.FromArgb(210, 35, 175, 255), OnMouseBackgroundEnd));
        public static readonly DependencyProperty PressedBackgroundStartProperty =
            DependencyProperty.RegisterAttached("PressedBackgroundStart", typeof(Color), typeof(ButtonRound),
            new PropertyMetadata(Color.FromArgb(65, 35, 175, 255)));
        public static readonly DependencyProperty PressedBackgroundEndProperty =
            DependencyProperty.RegisterAttached("PressedBackgroundEnd", typeof(Color), typeof(ButtonRound),
            new PropertyMetadata(Color.FromArgb(255, 35, 175, 255)));

        #endregion
        #region 背景图片
        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.RegisterAttached("BackgroundImage", typeof(ImageSource), typeof(ButtonRound), new UIPropertyMetadata(null, OnBackgroundImage));
        public static readonly DependencyProperty BackgroundFocusedImageProperty =
            DependencyProperty.RegisterAttached("BackgroundFocusedImage", typeof(ImageSource), typeof(ButtonRound));
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.RegisterAttached("Stretch", typeof(Stretch), typeof(ButtonRound),
            new PropertyMetadata(Stretch.Fill));
        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.RegisterAttached("Viewport", typeof(Rect), typeof(ButtonRound),
            new PropertyMetadata(new Rect(0, 0, 1, 1)));

        #endregion

        #endregion

        #region 扩展定义
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
        [Category("扩展")]
        [Description("背景图片的位置")]
        public Rect Viewport
        {
            get { return (Rect)GetValue(ViewportProperty); }
            set { SetValue(ViewportProperty, value); }
        }

        #endregion

        #endregion

        #region 自动设置
        private static void OnBackgroundImage(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ButtonRound btn)
            {
                btn.Loaded += delegate
                {
                    if (btn.BackgroundImage != null)
                    {
                        if (btn.BackgroundFocusedImage == null)
                        {
                            btn.BackgroundFocusedImage = btn.BackgroundImage;
                        }
                        if (btn.NormalBackgroundStart == Color.FromArgb(254, 254, 254, 254))
                        {
                            btn.NormalBackgroundStart = Colors.Transparent;
                            btn.NormalBackgroundEnd = Colors.Transparent;
                            btn.MouseBackgroundStart = Colors.Transparent;
                            btn.MouseBackgroundEnd = Colors.Transparent;
                            btn.PressedBackgroundStart = Colors.Transparent;
                            btn.PressedBackgroundEnd = Colors.Transparent;
                        }
                    }
                };
            }
        }
        private static void OnNormalBackgroundStart(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ButtonRound btn)
            {
                btn.Loaded += delegate
                {
                    if (btn.NormalBackgroundStart != Color.FromArgb(254, 254, 254, 254) && btn.NormalBackgroundStart != Colors.Transparent)
                    {
                        if (btn.MouseBackgroundStart == Color.FromArgb(210, 56, 192, 200))
                        {
                            btn.MouseBackgroundStart = Color.FromArgb((byte)(btn.NormalBackgroundStart.A + (255 - btn.NormalBackgroundStart.A) / 2), btn.NormalBackgroundStart.R, btn.NormalBackgroundStart.G, btn.NormalBackgroundStart.B);
                        }
                        btn.PressedBackgroundStart = Color.FromArgb(255, btn.NormalBackgroundStart.R, btn.NormalBackgroundStart.G, btn.NormalBackgroundStart.B);
                    }
                };
            }
        }
        private static void OnNormalBackgroundEnd(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ButtonRound btn)
            {
                btn.Loaded += delegate
                {
                    if (btn.NormalBackgroundEnd != Color.FromArgb(254, 220, 220, 220) && btn.NormalBackgroundEnd != Colors.Transparent)
                    {
                        if (btn.MouseBackgroundEnd == Color.FromArgb(210, 35, 175, 255))
                        {
                            btn.MouseBackgroundEnd = Color.FromArgb((byte)(btn.NormalBackgroundEnd.A + (255 - btn.NormalBackgroundEnd.A) / 2), btn.NormalBackgroundEnd.R, btn.NormalBackgroundEnd.G, btn.NormalBackgroundEnd.B);
                        }
                        btn.PressedBackgroundEnd = Color.FromArgb(255, btn.NormalBackgroundEnd.R, btn.NormalBackgroundEnd.G, btn.NormalBackgroundEnd.B);
                    }
                };
            }
        }
        private static void OnMouseBackgroundStart(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ButtonRound btn)
            {
                btn.Loaded += delegate
                {
                    if (btn.MouseBackgroundStart != Color.FromArgb(210, 56, 192, 200) && btn.MouseBackgroundStart != Colors.Transparent)
                    {
                        btn.PressedBackgroundStart = Color.FromArgb(255, btn.MouseBackgroundStart.R, btn.MouseBackgroundStart.G, btn.MouseBackgroundStart.B);
                    }
                };
            }
        }
        private static void OnMouseBackgroundEnd(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ButtonRound btn)
            {
                btn.Loaded += delegate
                {
                    if (btn.MouseBackgroundEnd != Color.FromArgb(210, 35, 175, 255) && btn.MouseBackgroundEnd != Colors.Transparent)
                    {
                        btn.PressedBackgroundEnd = Color.FromArgb(255, btn.MouseBackgroundEnd.R, btn.MouseBackgroundEnd.G, btn.MouseBackgroundEnd.B);
                    }
                };
            }
        }

        #endregion

        public ButtonRound() { }
    }
}
