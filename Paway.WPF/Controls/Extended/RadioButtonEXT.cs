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
        public static readonly DependencyProperty BorderFocusedBrushProperty =
            DependencyProperty.RegisterAttached(nameof(BorderFocusedBrush), typeof(BrushEXT), typeof(RadioButtonEXT),
                new PropertyMetadata(new BrushEXT(Colors.Gray, Color.FromArgb(200, 35, 175, 255), null, 55)));

        #endregion

        #region 扩展
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
        public RadioButtonEXT()
        {
            DefaultStyleKey = typeof(RadioButtonEXT);
        }
    }
}
