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
    /// ComboBox扩展
    /// </summary>
    public partial class ComboBoxEXT : ComboBox
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ComboBoxEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemForeground), typeof(BrushEXT), typeof(ComboBoxEXT),
            new PropertyMetadata(new BrushEXT(Color.FromArgb(255, 34, 34, 34), Color.FromArgb(255, 34, 34, 34), Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBackground), typeof(BrushEXT), typeof(ComboBoxEXT),
            new PropertyMetadata(new BrushEXT(null, Color.FromArgb(85, Config.Color.R, Config.Color.G, Config.Color.B), alpha: 85)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(ComboBoxEXT),
                new PropertyMetadata(new BrushEXT(Colors.LightGray, Color.FromArgb(170, Config.Color.R, Config.Color.G, Config.Color.B), null, 85)));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 项字体颜色
        /// </summary>
        [Category("扩展")]
        [Description("项字体颜色")]
        public BrushEXT ItemForeground
        {
            get { return (BrushEXT)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }
        /// <summary>
        /// 项背景颜色
        /// </summary>
        [Category("扩展")]
        [Description("项背景颜色")]
        public BrushEXT ItemBackground
        {
            get { return (BrushEXT)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }
        /// <summary>
        /// 外边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("外边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ComboBoxEXT()
        {
            DefaultStyleKey = typeof(ComboBoxEXT);
        }
    }
}
