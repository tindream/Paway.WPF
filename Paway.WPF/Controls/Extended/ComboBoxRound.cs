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
    public partial class ComboBoxRound : ComboBox
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ComboBoxRound), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ForegroundRoundProperty =
            DependencyProperty.RegisterAttached(nameof(ForegroundRound), typeof(BrushRound), typeof(ComboBoxRound),
            new PropertyMetadata(new BrushRound(Colors.Black, Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BackgroundRoundProperty =
            DependencyProperty.RegisterAttached(nameof(BackgroundRound), typeof(BrushRound), typeof(ComboBoxRound),
            new PropertyMetadata(new BrushRound(Colors.LightGray, Color.FromArgb(149, 35, 175, 255))));

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
        public BrushRound ForegroundRound
        {
            get { return (BrushRound)GetValue(ForegroundRoundProperty); }
            set { SetValue(ForegroundRoundProperty, value); }
        }
        /// <summary>
        /// 项的边框背景颜色
        /// </summary>
        [Category("扩展")]
        [Description("项的边框背景颜色")]
        public BrushRound BackgroundRound
        {
            get { return (BrushRound)GetValue(BackgroundRoundProperty); }
            set { SetValue(BackgroundRoundProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ComboBoxRound() { }
    }
}
