using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// Expander扩展
    /// </summary>
    public class ExpanderEXT : Expander
    {
        private double lastSize = double.NaN;
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ExpanderEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(ExpanderEXT),
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty MoveResizerProperty =
            DependencyProperty.RegisterAttached(nameof(MoveResizer), typeof(bool), typeof(ExpanderEXT), new PropertyMetadata(true));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// <para>默认值：3</para>
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
        /// <para>默认值：默认</para>
        /// </summary>
        [Category("扩展")]
        [Description("边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 鼠标拖动设置大小
        /// <para>默认值：true</para>
        /// </summary>
        [Category("扩展")]
        [Description("鼠标拖动设置大小")]
        public bool MoveResizer
        {
            get { return (bool)GetValue(MoveResizerProperty); }
            set { SetValue(MoveResizerProperty, value); }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 鼠标拖动设置大小完成事件
        /// </summary>
        public event EventHandler ResizeEvent;

        #endregion

        /// <summary>
        /// </summary>
        public ExpanderEXT()
        {
            DefaultStyleKey = typeof(ExpanderEXT);
        }

        #region Private属性
        private FrameworkElement PART_Root;

        #endregion

        #region Override方法
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.PART_Root = this.GetTemplateChild("PART_Root") as FrameworkElement;
            if (this.PART_Root != null)
            {
                if (this.IsExpanded)
                {
                    var suffix = "Y";
                    switch (ExpandDirection)
                    {
                        case ExpandDirection.Down:
                        case ExpandDirection.Up:
                            break;
                        case ExpandDirection.Left:
                        case ExpandDirection.Right:
                            suffix = ExpandDirection.ToString();
                            break;
                    }
                    VisualStateManager.GoToElementState(this.PART_Root, $"Storyboard_Expanded_{suffix}", true);
                }
                else
                {
                    this.OnCollapsed();
                }
            }
        }
        /// <summary>
        /// 折叠
        /// </summary>
        protected override void OnCollapsed()
        {
            base.OnCollapsed();
            if (this.PART_Root != null)
            {
                var suffix = "Y";
                switch (ExpandDirection)
                {
                    case ExpandDirection.Down:
                    case ExpandDirection.Up:
                        this.lastSize = this.Height;
                        this.Height = double.NaN;
                        break;
                    case ExpandDirection.Left:
                    case ExpandDirection.Right:
                        suffix = ExpandDirection.ToString();
                        this.lastSize = this.Width;
                        this.Width = double.NaN;
                        break;
                }
                VisualStateManager.GoToElementState(this.PART_Root, $"Storyboard_Collapsed_{suffix}", true);
            }
        }
        /// <summary>
        /// 展开
        /// </summary>
        protected override void OnExpanded()
        {
            base.OnExpanded();
            if (this.PART_Root != null)
            {
                var suffix = "Y";
                switch (ExpandDirection)
                {
                    case ExpandDirection.Down:
                    case ExpandDirection.Up:
                        this.Height = this.lastSize;
                        break;
                    case ExpandDirection.Left:
                    case ExpandDirection.Right:
                        suffix = ExpandDirection.ToString();
                        this.Width = this.lastSize;
                        break;
                }
                VisualStateManager.GoToElementState(this.PART_Root, $"Storyboard_Expanded_{suffix}", true);
            }
        }

        #endregion

        #region 大小拖动控制
        private bool _pressed;
        private Point _startPoint;
        /// <summary>
        /// 按下时的内容尺寸
        /// </summary>
        private double _startSize;
        private bool? LeftDirection;
        private bool? TopDirection;
        /// <summary>
        /// 鼠标抬起拉伸设置大小
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!MoveResizer) return;
            var point = e.GetPosition(this);
            if (!_pressed)
            {
                SetCursor(point);
                return;
            }
            var pointScr = this.PointToScreen(point);
            if (LeftDirection.HasValue)
            {//左右拉伸
                var horiChange = pointScr.X - _startPoint.X;
                if (LeftDirection.Value) horiChange *= -1;
                var contentValue = _startSize + horiChange;
                {
                    var minWidth = this.MinWidth;
                    if (minWidth == 0) minWidth = 100;
                    var maxWidth = this.MaxWidth;
                    if (contentValue > minWidth && (double.IsInfinity(maxWidth) || contentValue < maxWidth))
                    {
                        this.Width = contentValue;
                        _startSize = contentValue;
                    }
                    else return;
                }
            }
            if (TopDirection.HasValue)
            {//上下拉伸
                var vertiChange = pointScr.Y - _startPoint.Y;
                if (TopDirection.Value) vertiChange *= -1;
                var contentValue = _startSize + vertiChange;
                {
                    var minHeight = this.MinHeight;
                    if (minHeight == 0) minHeight = 100;
                    var maxHeight = this.MaxHeight;
                    if (contentValue > minHeight && (double.IsInfinity(maxHeight) || contentValue < maxHeight))
                    {
                        this.Height = contentValue;
                        _startSize = contentValue;
                    }
                    else return;
                }
            }
            _startPoint = pointScr;
            if (ResizeEvent != null)
            {
                var args = new RoutedEventArgs(e.RoutedEvent, this);
                ResizeEvent.Invoke(this, args);
            }
        }
        private void SetCursor(Point point)
        {
            if (!this.IsExpanded) return;
            var left = point.X;
            var top = point.Y;
            var right = this.ActualWidth - left;
            var bottom = this.ActualHeight - top;

            LeftDirection = TopDirection = null;
            switch (this.ExpandDirection)
            {
                case ExpandDirection.Left:
                    if (right < 7) LeftDirection = false;
                    break;
                case ExpandDirection.Right:
                    if (left < 7) LeftDirection = true;
                    break;
                case ExpandDirection.Up:
                    if (bottom < 7) TopDirection = false;
                    break;
                case ExpandDirection.Down:
                    if (top < 7) TopDirection = true;
                    break;
            }
            if (LeftDirection.HasValue)
            {
                this.Cursor = Cursors.SizeWE;
            }
            else if (TopDirection.HasValue)
            {
                this.Cursor = Cursors.SizeNS;
            }
            else
            {
                this.Cursor = null;
            }
        }
        /// <summary>
        /// 鼠标按下准备移动
        /// </summary>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && MoveResizer && (LeftDirection.HasValue || TopDirection.HasValue))
            {
                var point = e.GetPosition(this);
                _pressed = true;
                _startPoint = this.PointToScreen(point);
                if (LeftDirection.HasValue) _startSize = this.ActualWidth;
                else _startSize = this.ActualHeight;
                this.CaptureMouse();
                e.Handled = true;
            }
            base.OnPreviewMouseLeftButtonDown(e);
        }
        /// <summary>
        /// </summary>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (MoveResizer)
            {
                var point = e.GetPosition(this);
                if (!_pressed)
                {
                    SetCursor(point);
                }
            }
            base.OnMouseEnter(e);
        }
        /// <summary>
        /// 鼠标抬起停止移动
        /// </summary>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (MoveResizer && _pressed)
            {
                _pressed = false;
                this.ReleaseMouseCapture();
            }
            base.OnMouseLeftButtonUp(e);
        }
        #endregion
    }
}