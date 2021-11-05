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
                new PropertyMetadata(new ColorLinear()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(TrackBackground), typeof(Brush), typeof(SliderEXT),
                new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.RegisterAttached(nameof(ButtonType), typeof(ButtonType), typeof(SliderEXT),
                new PropertyMetadata(ButtonType.Round));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ShowTrackTextProperty =
            DependencyProperty.RegisterAttached(nameof(ShowTrackText), typeof(bool), typeof(SliderEXT),
                new PropertyMetadata(false));

        private static void OnRadiusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is SliderEXT slider)
            {
                slider.Loaded += delegate
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
        /// <summary>
        /// 按钮类型
        /// </summary>
        [Category("扩展")]
        [Description("按钮类型")]
        public ButtonType ButtonType
        {
            get { return (ButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }
        /// <summary>
        /// 显示刻度值
        /// </summary>
        [Category("扩展")]
        [Description("显示刻度值")]
        public bool ShowTrackText
        {
            get { return (bool)GetValue(ShowTrackTextProperty); }
            set { SetValue(ShowTrackTextProperty, value); }
        }

        #endregion

        #region 事件
        /// <summary>
        /// ToolTip值更新路由事件
        /// </summary>
        public event EventHandler<ValueChangeEventArgs> ToolTipValueChanged;

        /// <summary>
        /// 刻度值重写路由事件
        /// </summary>
        public event EventHandler<ValueChangeEventArgs> TrackValueEvent;

        #endregion

        /// <summary>
        /// </summary>
        public SliderEXT()
        {
            DefaultStyleKey = typeof(SliderEXT);
            this.ValueChanged += SliderEXT_ValueChanged;
        }

        #region 刻度值
        /// <summary>
        /// 注册刻度值重写路由事件
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (PMethod.Child(this, out TickBarEXT tickBar, "TopTick", false))
            {
                tickBar.TrackValue += TickBar_TrackValue;
            }
        }
        private void TickBar_TrackValue(object sender, ValueChangeEventArgs e)
        {
            TrackValueEvent?.Invoke(sender, e);
        }

        #endregion

        #region ToolTip
        private ToolTip toolTip;
        /// <summary>
        /// 点击移动到指定位置
        /// </summary>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            var value = OnToolTipValueChanged(e.RoutedEvent);
            if (this.AutoToolTipPlacement == AutoToolTipPlacement.None) return;
            if (toolTip == null)
            {
                if (PMethod.Child(this, out Thumb thumb, iParent: false))
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
            toolTip.Content = PMethod.Rounds(value, this.AutoToolTipPrecision, this.AutoToolTipPrecision);
            if (toolTip.IsOpen)
            {
                toolTip.IsOpen = false;
                //会自动更新位置，再次=True位置不更新
                return;
            }
            toolTip.IsOpen = true;
            PMethod.ExecuteMethod(toolTip.Parent, "Reposition");
        }
        private CustomPopupPlacement[] AutoToolTipCustomPlacementCallbackTemp(Size popupSize, Size targetSize, Point offset)
        {
            if (PMethod.ExecuteMethod(this, "AutoToolTipCustomPlacementCallback", out object result, popupSize, targetSize, offset))
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
            OnToolTipValueChanged(e.RoutedEvent);
            if (PMethod.Child(this, out Thumb thumb, iParent: false))
            {
                if (thumb.ToolTip is ToolTip toolTip)
                {
                    toolTip.Content = this.ToolTip;
                }
            }
        }

        private void SliderEXT_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            OnToolTipValueChanged(e.RoutedEvent);
        }
        private double OnToolTipValueChanged(RoutedEvent routedEvent)
        {
            if (this.AutoToolTipPlacement == AutoToolTipPlacement.None) return this.Value;
            var arg = new ValueChangeEventArgs(this.Value, routedEvent, this);
            ToolTipValueChanged?.Invoke(this, arg);
            this.ToolTip = PMethod.Rounds(arg.Value, this.AutoToolTipPrecision, this.AutoToolTipPrecision);
            if (toolTip != null) toolTip.Content = this.ToolTip;
            return arg.Value;
        }

        #endregion
    }
}
