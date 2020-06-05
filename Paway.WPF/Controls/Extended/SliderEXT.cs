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
    /// Slider扩展
    /// </summary>
    public partial class SliderEXT : Slider
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(SliderEXT), new PropertyMetadata(new CornerRadius(2), OnRadiusChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusLeftProperty =
            DependencyProperty.RegisterAttached("RadiusLeft", typeof(CornerRadius), typeof(SliderEXT), new PropertyMetadata(new CornerRadius(2, 0, 0, 2)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackHeightProperty =
            DependencyProperty.RegisterAttached(nameof(TrackHeight), typeof(double), typeof(SliderEXT), new PropertyMetadata(3d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackWidthProperty =
            DependencyProperty.RegisterAttached(nameof(TrackWidth), typeof(double), typeof(SliderEXT), new PropertyMetadata(14d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackColorLinearProperty =
            DependencyProperty.RegisterAttached(nameof(TrackColorLinear), typeof(ColorLinear), typeof(SliderEXT),
                new PropertyMetadata(new ColorLinear(Color.FromArgb(85, 35, 175, 255))));
        private static void OnRadiusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is SliderEXT slider)
            {
                slider.LayoutUpdated += delegate
                {
                    if (((CornerRadius)slider.GetValue(RadiusLeftProperty)).TopLeft != slider.Radius.TopLeft)
                        slider.SetValue(RadiusLeftProperty, new CornerRadius(slider.Radius.TopLeft, 0, 0, slider.Radius.BottomLeft));
                };
            }
        }

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
        /// 轨道高度
        /// </summary>
        [Category("扩展")]
        [Description("轨道高度")]
        public double TrackHeight
        {
            get { return (double)GetValue(TrackHeightProperty); }
            set { SetValue(TrackHeightProperty, value); }
        }
        /// <summary>
        /// 轨道按钮宽度度
        /// </summary>
        [Category("扩展")]
        [Description("轨道按钮宽度度")]
        public double TrackWidth
        {
            get { return (double)GetValue(TrackWidthProperty); }
            set { SetValue(TrackWidthProperty, value); }
        }
        /// <summary>
        /// 轨道线性颜色
        /// </summary>
        [Category("扩展")]
        [Description("轨道线性颜色")]
        public ColorLinear TrackColorLinear
        {
            get { return (ColorLinear)GetValue(TrackColorLinearProperty); }
            set { SetValue(TrackColorLinearProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public SliderEXT()
        {
            DefaultStyleKey = typeof(SliderEXT);
        }
    }
}
