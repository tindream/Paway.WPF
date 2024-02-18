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
    /// CheckBox扩展-盒子样式
    /// </summary>
    public partial class CheckBoxEXT : CheckBox
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(CheckBoxEXT), new PropertyMetadata(new CornerRadius(2)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(CheckBoxEXT),
                new PropertyMetadata(new BrushEXT() { Normal = Colors.DarkGray.ToBrush() }));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// <para>默认值：2</para>
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
        /// <para>默认值：DarkGray, 默认, 默认</para>
        /// </summary>
        [Category("扩展")]
        [Description("边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public CheckBoxEXT()
        {
            DefaultStyleKey = typeof(CheckBoxEXT);
        }
    }
}
