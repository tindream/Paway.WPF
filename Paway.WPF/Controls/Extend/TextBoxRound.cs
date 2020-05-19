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
    /// TextBox扩展
    /// </summary>
    public partial class TextBoxRound : TextBox
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty WaterProperty =
            DependencyProperty.RegisterAttached("Water", typeof(string), typeof(TextBoxRound), new PropertyMetadata("请输入.."));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty WaterSizeProperty =
            DependencyProperty.RegisterAttached("WaterSize", typeof(double), typeof(TextBoxRound), new PropertyMetadata());
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached("Radius", typeof(CornerRadius), typeof(TextBoxRound), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BorderFocusedBrushProperty =
            DependencyProperty.RegisterAttached("BorderFocusedBrush", typeof(Brush), typeof(TextBoxRound),
                new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 35, 175, 255))));

        #endregion

        #region 扩展
        /// <summary>
        /// 水印内容
        /// </summary>
        [Category("扩展")]
        [Description("水印内容")]
        public string Water
        {
            get { return (string)GetValue(WaterProperty); }
            set { SetValue(WaterProperty, value); }
        }
        /// <summary>
        /// 水印字体大小
        /// </summary>
        [Category("扩展")]
        [Description("水印字体大小")]
        public double WaterSize
        {
            get { return (double)GetValue(WaterSizeProperty); }
            set { SetValue(WaterSizeProperty, value); }
        }
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
        /// 文本框激活时的边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("文本框激活时的边框颜色")]
        public Brush BorderFocusedBrush
        {
            get { return (Brush)GetValue(BorderFocusedBrushProperty); }
            set { SetValue(BorderFocusedBrushProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public TextBoxRound() { }
    }
}
