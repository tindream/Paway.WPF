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
    /// RadioButton扩展-盒子样式
    /// </summary>
    public partial class RadioBox : RadioButton
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(RadioBox),
                new PropertyMetadata(new BrushEXT() { Normal = new ThemeForeground(Colors.DarkGray) }));

        #endregion

        #region 扩展
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
        public RadioBox()
        {
            DefaultStyleKey = typeof(RadioBox);
        }
    }
}
