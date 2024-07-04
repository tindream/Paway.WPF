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
using System.Windows.Media.Animation;

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
        /// 左圆角
        /// <para>默认值：2, 0, 0, 2</para>
        /// </summary>
        public static readonly DependencyProperty RadiusLeftProperty =
            DependencyProperty.RegisterAttached("RadiusLeft", typeof(CornerRadius), typeof(SliderEXT), new PropertyMetadata(new CornerRadius(2, 0, 0, 2)));
        /// <summary>
        /// 下圆角
        /// <para>默认值：0, 0, 2, 2</para>
        /// </summary>
        public static readonly DependencyProperty RadiusBottomProperty =
            DependencyProperty.RegisterAttached("RadiusBottom", typeof(CornerRadius), typeof(SliderEXT), new PropertyMetadata(new CornerRadius(0, 0, 2, 2)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackHeightProperty =
            DependencyProperty.RegisterAttached(nameof(TrackHeight), typeof(double), typeof(SliderEXT), new PropertyMetadata(3d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackButtonWidthProperty =
            DependencyProperty.RegisterAttached(nameof(TrackButtonWidth), typeof(double), typeof(SliderEXT), new PropertyMetadata(18d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackButtonHeightProperty =
            DependencyProperty.RegisterAttached(nameof(TrackButtonHeight), typeof(double), typeof(SliderEXT), new PropertyMetadata(18d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackColorLinearProperty =
            DependencyProperty.RegisterAttached(nameof(TrackColorLinear), typeof(ColorLinear), typeof(SliderEXT),
                new PropertyMetadata(new ColorLinear()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(TrackBackground), typeof(Brush), typeof(SliderEXT),
                new PropertyMetadata(Colors.LightGray.ToBrush()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.RegisterAttached(nameof(ButtonType), typeof(SliderButtonType), typeof(SliderEXT),
                new PropertyMetadata(SliderButtonType.Round, OnButtonTypeChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ShowTrackTextProperty =
            DependencyProperty.RegisterAttached(nameof(ShowTrackText), typeof(bool), typeof(SliderEXT),
                new PropertyMetadata(false, OnShowTrackTextChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.RegisterAttached(nameof(Title), typeof(string), typeof(SliderEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleMinWidthProperty =
            DependencyProperty.RegisterAttached(nameof(TitleMinWidth), typeof(double), typeof(SliderEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ButtonImageProperty =
            DependencyProperty.RegisterAttached(nameof(ButtonImage), typeof(ImageSource), typeof(SliderEXT),
                new PropertyMetadata(null, OnButtonTypeChanged));

        /// <summary>
        /// 按钮样式事件
        /// </summary>
        internal event Action OnButtonTypeEvent;
        private static void OnButtonTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is SliderEXT slider)
            {
                slider.OnButtonTypeEvent?.Invoke();
            }
        }
        private static void OnShowTrackTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is SliderEXT slider)
            {//刷新 显示刻度值
                var tickPlacement = slider.TickPlacement;
                slider.TickPlacement = TickPlacement.None;
                slider.TickPlacement = tickPlacement;
            }
        }
        private static void OnRadiusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is SliderEXT slider)
            {
                slider.Loaded += delegate
                {
                    if (((CornerRadius)slider.GetValue(RadiusLeftProperty)).TopLeft != slider.Radius.TopLeft)
                        slider.SetValue(RadiusLeftProperty, new CornerRadius(slider.Radius.TopLeft, 0, 0, slider.Radius.BottomLeft));
                    if (((CornerRadius)slider.GetValue(RadiusBottomProperty)).BottomLeft != slider.Radius.BottomLeft)
                        slider.SetValue(RadiusBottomProperty, new CornerRadius(0, 0, slider.Radius.BottomRight, slider.Radius.BottomLeft));
                };
            }
        }

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
        /// 轨道高度
        /// <para>默认值：3</para>
        /// </summary>
        [Category("扩展")]
        [Description("轨道高度")]
        public double TrackHeight
        {
            get { return (double)GetValue(TrackHeightProperty); }
            set { SetValue(TrackHeightProperty, value); }
        }
        /// <summary>
        /// 轨道按钮宽度
        /// <para>默认值：18</para>
        /// </summary>
        [Category("扩展")]
        [Description("轨道按钮宽度")]
        [TypeConverter(typeof(LengthConverter))]
        public double TrackButtonWidth
        {
            get { return (double)GetValue(TrackButtonWidthProperty); }
            set { SetValue(TrackButtonWidthProperty, value); }
        }
        /// <summary>
        /// 轨道按钮高度
        /// <para>默认值：18</para>
        /// </summary>
        [Category("扩展")]
        [Description("轨道按钮高度")]
        [TypeConverter(typeof(LengthConverter))]
        public double TrackButtonHeight
        {
            get { return (double)GetValue(TrackButtonHeightProperty); }
            set { SetValue(TrackButtonHeightProperty, value); }
        }
        /// <summary>
        /// 轨道线性颜色
        /// <para>默认值：默认</para>
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
        /// <para>默认值：LightGray</para>
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
        /// <para>默认值：Round</para>
        /// </summary>
        [Category("扩展")]
        [Description("按钮类型")]
        public SliderButtonType ButtonType
        {
            get { return (SliderButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }
        /// <summary>
        /// 显示刻度值
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("显示刻度值")]
        public bool ShowTrackText
        {
            get { return (bool)GetValue(ShowTrackTextProperty); }
            set { SetValue(ShowTrackTextProperty, value); }
        }
        /// <summary>
        /// 标题
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("标题")]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        /// <summary>
        /// 标题最小长度
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("标题最小长度")]
        public double TitleMinWidth
        {
            get { return (double)GetValue(TitleMinWidthProperty); }
            set { SetValue(TitleMinWidthProperty, value); }
        }
        /// <summary>
        /// 按钮图标
        /// <para>默认值：无</para>
        /// </summary>
        [Category("扩展")]
        [Description("按钮图标")]
        public ImageSource ButtonImage
        {
            get { return (ImageSource)GetValue(ButtonImageProperty); }
            set { SetValue(ButtonImageProperty, value); }
        }

        #endregion

        #region 动画进度
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty AnimationValueProperty =
            DependencyProperty.RegisterAttached(nameof(AnimationValue), typeof(double), typeof(SliderEXT), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAnimationValueChanged));
        /// <summary>
        /// 动画进度值
        /// <para>默认值：0</para>
        /// </summary>
        [Category("扩展")]
        [Description("动画进度值")]
        public double AnimationValue
        {
            get { return (double)GetValue(AnimationValueProperty); }
            set { SetValue(AnimationValueProperty, value); }
        }
        private static void OnAnimationValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is SliderEXT slider)
            {
                var animTime = PMethod.AnimTime((double)e.NewValue - slider.Value) * 1.5;
                var animValue = new DoubleAnimation((double)e.NewValue, new Duration(TimeSpan.FromMilliseconds(animTime)))
                {
                    //AccelerationRatio = 0,
                    //DecelerationRatio = 1,
                    EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut },
                };
                slider.BeginAnimation(SliderEXT.ValueProperty, animValue);
            }
        }

        #endregion

        #region ToolTip值更新路由事件
        /// <summary>
        /// ToolTip值更新路由事件
        /// </summary>
        public event EventHandler<ValueChangeEventArgs> TipValueChanged;
        /// <summary>
        /// ToolTip值更新路由事件
        /// </summary>
        private double OnTipValueChanged(double value, RoutedEvent routedEvent)
        {
            var args = new ValueChangeEventArgs(value, routedEvent, this);
            TipValueChanged?.Invoke(this, args);
            return args.Value;
        }
        /// <summary>
        /// ToolTip值更新显示路由事件
        /// </summary>
        public event EventHandler<ValueChangeEventArgs> TipValuesChanged;
        /// <summary>
        /// ToolTip值更新显示路由事件
        /// </summary>
        private void OnTipValuesChanged(double value, RoutedEvent routedEvent)
        {
            if (TipValuesChanged != null)
            {
                var args = new ValuesChangeEventArgs(value, this.AutoToolTipPrecision, routedEvent, this);
                TipValuesChanged.Invoke(this, args);
                this.ToolTip = args.Values;
            }
            else
            {
                this.ToolTip = value.Rounds(this.AutoToolTipPrecision, this.AutoToolTipPrecision);
            }
            if (toolTip != null) toolTip.Content = this.ToolTip;
        }

        #endregion
        #region 刻度值重写路由事件
        /// <summary>
        /// 刻度值重写路由事件
        /// </summary>
        public event EventHandler<ValueChangeEventArgs> TrackValueChanged;
        /// <summary>
        /// 刻度值重写路由事件
        /// </summary>
        private void OnTrackValueChanged(ValueChangeEventArgs e)
        {
            TrackValueChanged?.Invoke(this, e);
        }

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
                tickBar.TrackValueChanged += TickBar_TrackValue;
            }
        }
        private void TickBar_TrackValue(object sender, ValueChangeEventArgs e)
        {
            OnTrackValueChanged(e);
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
            again:
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
            OnTipValuesChanged(value, e.RoutedEvent);
            if (toolTip.IsOpen)
            {
                toolTip.IsOpen = false;
                //再次=True时位置不更新，需重置
                toolTip = null;
                goto again;
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
            var value = OnTipValueChanged(this.Value, routedEvent);
            OnTipValuesChanged(value, routedEvent);
            return value;
        }

        #endregion
    }
}
