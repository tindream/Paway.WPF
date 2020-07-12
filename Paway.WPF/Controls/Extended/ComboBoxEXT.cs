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
        public static readonly DependencyProperty ForegroundRoundProperty =
            DependencyProperty.RegisterAttached(nameof(ForegroundRound), typeof(BrushEXT), typeof(ComboBoxEXT),
            new PropertyMetadata(new BrushEXT(Color.FromArgb(255, 34, 34, 34), Color.FromArgb(255, 34, 34, 34), Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BackgroundRoundProperty =
            DependencyProperty.RegisterAttached(nameof(BackgroundRound), typeof(BrushEXT), typeof(ComboBoxEXT),
            new PropertyMetadata(new BrushEXT(null, Color.FromArgb(85, 35, 175, 255), alpha: 85)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BorderFocusedBrushProperty =
            DependencyProperty.RegisterAttached(nameof(BorderFocusedBrush), typeof(BrushEXT), typeof(ComboBoxEXT),
                new PropertyMetadata(new BrushEXT(Colors.LightGray, Color.FromArgb(170, 35, 175, 255), null, 85)));

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
        /// 项的字体颜色
        /// </summary>
        [Category("扩展")]
        [Description("项的字体颜色")]
        public BrushEXT ForegroundRound
        {
            get { return (BrushEXT)GetValue(ForegroundRoundProperty); }
            set { SetValue(ForegroundRoundProperty, value); }
        }
        /// <summary>
        /// 项的边框背景颜色
        /// </summary>
        [Category("扩展")]
        [Description("项的边框背景颜色")]
        public BrushEXT BackgroundRound
        {
            get { return (BrushEXT)GetValue(BackgroundRoundProperty); }
            set { SetValue(BackgroundRoundProperty, value); }
        }
        /// <summary>
        /// 外边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("外边框颜色")]
        public BrushEXT BorderFocusedBrush
        {
            get { return (BrushEXT)GetValue(BorderFocusedBrushProperty); }
            set { SetValue(BorderFocusedBrushProperty, value); }
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
