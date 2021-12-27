using System;
using System.ComponentModel;
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
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 鼠标拖动设置大小
        /// </summary>
        [Category("扩展")]
        [Description("鼠标拖动设置大小")]
        public bool MoveResizer
        {
            get { return (bool)GetValue(MoveResizerProperty); }
            set { SetValue(MoveResizerProperty, value); }
        }

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
                VisualStateManager.GoToElementState(this.PART_Root, this.IsExpanded ? $"Storyboard_Expanded_{suffix}" : $"Storyboard_Collapsed_{suffix}", true);
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
                        break;
                    case ExpandDirection.Left:
                    case ExpandDirection.Right:
                        suffix = ExpandDirection.ToString();
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
                        break;
                    case ExpandDirection.Left:
                    case ExpandDirection.Right:
                        suffix = ExpandDirection.ToString();
                        break;
                }
                VisualStateManager.GoToElementState(this.PART_Root, $"Storyboard_Expanded_{suffix}", true);
            }
        }

        #endregion

        #region 大小拖动控制
        private bool _pressed;
        private Point _prevPoint;
        private bool? LeftDirection;
        private bool? TopDirection;
        /// <summary>
        /// 鼠标拖动设置大小完成事件
        /// </summary>
        public event EventHandler ResizeEvent;
        /// <summary>
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
            double vertiChange, horiChange;
            vertiChange = horiChange = 0;
            var pointScr = this.PointToScreen(point);
            if (LeftDirection.HasValue)
            {//左右拉伸
                horiChange = pointScr.X - _prevPoint.X;
                if (LeftDirection.Value) horiChange *= -1;
                var value = this.ActualWidth + horiChange;
                var contentValue = value - this.BorderThickness.Left - this.BorderThickness.Right;
                if (this.Content is FrameworkElement content)
                {
                    var minWidth = content.MinWidth;
                    if (minWidth == 0) minWidth = 100;
                    var maxWidth = content.MaxWidth;
                    if (double.IsInfinity(maxWidth)) maxWidth = (this.Parent as FrameworkElement).ActualWidth - 100;
                    if (contentValue > minWidth && value < maxWidth)
                    {
                        content.Width = contentValue;
                    }
                    else return;
                }
            }
            if (TopDirection.HasValue)
            {//上下拉伸
                vertiChange = pointScr.Y - _prevPoint.Y;
                if (TopDirection.Value) vertiChange *= -1;
                var value = this.ActualHeight + vertiChange;
                var contentValue = value - this.BorderThickness.Top - this.BorderThickness.Bottom - 30;
                if (this.Content is FrameworkElement content)
                {
                    var minHeight = content.MinHeight;
                    if (minHeight == 0) minHeight = 100;
                    var maxHeight = content.MaxHeight;
                    if (double.IsInfinity(maxHeight)) maxHeight = (this.Parent as FrameworkElement).ActualHeight - 100;
                    if (contentValue > minHeight && value < maxHeight)
                    {
                        content.Height = contentValue;
                    }
                    else return;
                }
            }
            _prevPoint = pointScr;
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
        /// </summary>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && MoveResizer && (LeftDirection.HasValue || TopDirection.HasValue))
            {
                var point = e.GetPosition(this);
                _pressed = true;
                _prevPoint = this.PointToScreen(point);
                this.CaptureMouse();
                e.Handled = true;
            }
            base.OnPreviewMouseDown(e);
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
        /// </summary>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (MoveResizer)
            {
                _pressed = false;
                this.ReleaseMouseCapture();
            }
            base.OnMouseUp(e);
        }
        #endregion
    }
}