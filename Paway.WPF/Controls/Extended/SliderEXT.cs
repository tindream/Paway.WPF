using Paway.Helper;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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
        public static readonly DependencyProperty RadiusBottomProperty =
            DependencyProperty.RegisterAttached("RadiusBottom", typeof(CornerRadius), typeof(SliderEXT), new PropertyMetadata(new CornerRadius(0, 0, 2, 2)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackHeightProperty =
            DependencyProperty.RegisterAttached(nameof(TrackHeight), typeof(double), typeof(SliderEXT), new PropertyMetadata(3d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackWidthProperty =
            DependencyProperty.RegisterAttached(nameof(TrackWidth), typeof(double), typeof(SliderEXT), new PropertyMetadata(18d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackColorLinearProperty =
            DependencyProperty.RegisterAttached(nameof(TrackColorLinear), typeof(ColorLinear), typeof(SliderEXT),
                new PropertyMetadata(new ColorLinear(85, 250)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(TrackBackground), typeof(Brush), typeof(SliderEXT),
                new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        private static void OnRadiusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is SliderEXT slider)
            {
                slider.LayoutUpdated += delegate
                {
                    if (((CornerRadius)slider.GetValue(RadiusLeftProperty)).TopLeft != slider.Radius.TopLeft)
                        slider.SetValue(RadiusLeftProperty, new CornerRadius(slider.Radius.TopLeft, 0, 0, slider.Radius.BottomLeft));
                    if (((CornerRadius)slider.GetValue(RadiusBottomProperty)).BottomLeft != slider.Radius.BottomLeft)
                        slider.SetValue(RadiusBottomProperty, new CornerRadius(0, 0, slider.Radius.BottomLeft, slider.Radius.BottomRight));
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
        /// <summary>
        /// 轨道背景颜色
        /// </summary>
        [Category("扩展")]
        [Description("轨道背景颜色")]
        public Brush TrackBackground
        {
            get { return (Brush)GetValue(TrackBackgroundProperty); }
            set { SetValue(TrackBackgroundProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public SliderEXT()
        {
            DefaultStyleKey = typeof(SliderEXT);
            this.ValueChanged += SliderEXT_ValueChanged;
        }

        #region ToolTip
        private ToolTip toolTip;
        /// <summary>
        /// 点击移动到指定位置
        /// </summary>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            this.ToolTip = TMethod.Round(this.Value, this.AutoToolTipPrecision);

            if (this.AutoToolTipPlacement == System.Windows.Controls.Primitives.AutoToolTipPlacement.None) return;
            if (toolTip == null)
            {
                if (TMethod.Child(this, out Thumb thumb))
                {
                    toolTip = new ToolTip
                    {
                        Placement = PlacementMode.Custom,
                        PlacementTarget = thumb,
                        CustomPopupPlacementCallback = AutoToolTipCustomPlacementCallbackTemp
                    };
                    thumb.ToolTip = toolTip;
                }
            }
            toolTip.Content = TMethod.Round(this.Value, this.AutoToolTipPrecision);
            if (toolTip.IsOpen)
            {
                toolTip.IsOpen = false;
                //会自动更新位置，再次=True位置不更新
                return;
            }
            toolTip.IsOpen = true;
            TMethod.ExecuteMethod(toolTip.Parent, "Reposition");
        }
        private CustomPopupPlacement[] AutoToolTipCustomPlacementCallbackTemp(Size popupSize, Size targetSize, Point offset)
        {
            if (TMethod.ExecuteMethod(this, "AutoToolTipCustomPlacementCallback", out object result, popupSize, targetSize, offset))
            {
                return result as CustomPopupPlacement[];
            }
            return null;
        }
        /// <summary>
        /// 鼠标离开
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (this.toolTip != null) this.toolTip.IsOpen = false;
        }

        /// <summary>
        /// 拖动开始
        /// </summary>
        protected override void OnThumbDragStarted(DragStartedEventArgs e)
        {
            base.OnThumbDragStarted(e);
            if (this.toolTip != null) this.toolTip.IsOpen = false;
        }
        /// <summary>
        /// 拖动到指定位置
        /// </summary>
        protected override void OnThumbDragDelta(DragDeltaEventArgs e)
        {
            base.OnThumbDragDelta(e);
            this.ToolTip = TMethod.Round(this.Value, this.AutoToolTipPrecision);
        }

        private void SliderEXT_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.ToolTip = TMethod.Round(this.Value, this.AutoToolTipPrecision);
        }

        #endregion
    }
}
