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
    /// RadioButton扩展
    /// </summary>
    public partial class RadioButtonEXT : RadioButton
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(RadioButtonEXT),
                new PropertyMetadata(new BrushEXT() { Normal = Colors.DarkGray.ToBrush() }));

        #endregion

        #region 扩展
        /// <summary>
        /// 边框颜色
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
        public RadioButtonEXT()
        {
            DefaultStyleKey = typeof(RadioButtonEXT);
        }
    }
}
