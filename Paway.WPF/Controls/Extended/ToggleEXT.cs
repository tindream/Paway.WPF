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
    /// CheckBox扩展-开关
    /// </summary>
    public partial class ToggleEXT : CheckBox
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ToggleEXT), new PropertyMetadata(new CornerRadius(7)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BorderFocusedBrushProperty =
            DependencyProperty.RegisterAttached(nameof(BorderFocusedBrush), typeof(BrushEXT), typeof(ToggleEXT),
                new PropertyMetadata(new BrushEXT(null, Color.FromArgb(170, 35, 175, 255), null, 85)));

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
        /// 边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("边框颜色")]
        public BrushEXT BorderFocusedBrush
        {
            get { return (BrushEXT)GetValue(BorderFocusedBrushProperty); }
            set { SetValue(BorderFocusedBrushProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ToggleEXT() { }
    }
}
