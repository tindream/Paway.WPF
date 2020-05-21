using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// ButtonRound扩展
    /// </summary>
    public partial class ButtonRound : Button
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
                DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ButtonRound), new PropertyMetadata(new CornerRadius(7)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty EffectRadiusProperty =
            DependencyProperty.RegisterAttached(nameof(EffectRadius), typeof(double), typeof(ButtonRound), new PropertyMetadata(0d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ForegroundRoundProperty =
            DependencyProperty.RegisterAttached(nameof(ForegroundRound), typeof(BrushRound), typeof(ButtonRound),
            new PropertyMetadata(new BrushRound(Color.FromArgb(255, 33, 33, 33), Color.FromArgb(255, 33, 33, 33), Colors.White)));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BackgroundStartProperty =
            DependencyProperty.RegisterAttached(nameof(BackgroundStart), typeof(ColorRound), typeof(ButtonRound),
            new PropertyMetadata(new ColorRound(Color.FromArgb(254, 254, 254, 254), Color.FromArgb(16, 35, 175, 255))));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BackgroundEndProperty =
            DependencyProperty.RegisterAttached(nameof(BackgroundEnd), typeof(ColorRound), typeof(ButtonRound),
            new PropertyMetadata(new ColorRound(Color.FromArgb(254, 220, 220, 220), Color.FromArgb(211, 35, 175, 255))));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.RegisterAttached(nameof(Viewport), typeof(Rect), typeof(ButtonRound),
            new PropertyMetadata(new Rect(0, 0, 1, 1)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.RegisterAttached(nameof(Stretch), typeof(Stretch), typeof(ButtonRound),
            new PropertyMetadata(Stretch.Fill));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.RegisterAttached(nameof(BackgroundImage), typeof(ImageRound), typeof(ButtonRound));

        #endregion

        #region 扩展.前景
        /// <summary>
        /// 自定义边框圆角
        /// </summary>
        [Category("扩展.前景")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 自定义边框圆角阴影宽度
        /// </summary>
        [Category("扩展.前景")]
        [Description("自定义边框圆角阴影宽度")]
        public double EffectRadius
        {
            get { return (double)GetValue(EffectRadiusProperty); }
            set { SetValue(EffectRadiusProperty, value); }
        }
        /// <summary>
        /// 自定义文本颜色
        /// </summary>
        [Category("扩展.前景")]
        [Description("自定义文本颜色")]
        public BrushRound ForegroundRound
        {
            get { return (BrushRound)GetValue(ForegroundRoundProperty); }
            set { SetValue(ForegroundRoundProperty, value); }
        }

        #endregion
        #region 扩展.背景颜色
        /// <summary>
        /// 自定义背景颜色(起始颜色)
        /// </summary>
        [Category("扩展.背景颜色")]
        [Description("自定义背景颜色(起始颜色)")]
        public ColorRound BackgroundStart
        {
            get { return (ColorRound)GetValue(BackgroundStartProperty); }
            set { SetValue(BackgroundStartProperty, value); }
        }
        /// <summary>
        /// 自定义背景颜色(终点颜色)
        /// </summary>
        [Category("扩展.背景颜色")]
        [Description("自定义背景颜色(终点颜色)")]
        public ColorRound BackgroundEnd
        {
            get { return (ColorRound)GetValue(BackgroundEndProperty); }
            set { SetValue(BackgroundEndProperty, value); }
        }

        #endregion
        #region 扩展.背景图片
        /// <summary>
        /// 背景图片位置
        /// </summary>
        [Category("扩展.背景图片")]
        [Description("背景图片位置")]
        public Rect Viewport
        {
            get { return (Rect)GetValue(ViewportProperty); }
            set { SetValue(ViewportProperty, value); }
        }
        /// <summary>
        /// 背景图片的内容如何拉伸才适合其磁贴
        /// </summary>
        [Category("扩展.背景图片")]
        [Description("背景图片的内容如何拉伸才适合其磁贴")]
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        /// <summary>
        /// 背景图片
        /// </summary>
        [Category("扩展.背景图片")]
        [Description("背景图片")]
        public ImageRound BackgroundImage
        {
            get { return (ImageRound)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ButtonRound() { }
    }
}
