using Paway.Helper;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Paway.WPF
{
    /// <summary>
    /// 进度控件拖动改值的公共实现
    /// <para>仪表盘与圆环的拖动逻辑完全一致，只有“是否允许拖动”的来源不同（圆环有 IDrag 开关）。
    /// 原先两份实现各自维护，改一处容易漏另一处，这里合并为唯一实现。</para>
    /// </summary>
    internal static class ProgressDragHelper
    {
        /// <summary>
        /// 按下开始拖动
        /// </summary>
        internal static void Down(RangeBase owner, bool canDrag, MouseButtonEventArgs e, ref Point? startPoint)
        {
            if (canDrag && e.ButtonState == MouseButtonState.Pressed && e.ClickCount == 1)
            {
                if (owner.Parent(out Window window)) window.Cursor = Cursors.Hand;
                startPoint = e.GetPosition(owner);
                //尝试将鼠标强制捕获到控件
                owner.CaptureMouse();
                e.Handled = true;
            }
        }
        /// <summary>
        /// 移动位置
        /// </summary>
        internal static void Move(RangeBase owner, MouseEventArgs e, ref Point? startPoint)
        {
            if (e.LeftButton == MouseButtonState.Pressed && startPoint != null)
            {
                var movePoint = e.GetPosition(owner);
                Calc(owner, startPoint.Value, movePoint);
                startPoint = movePoint;
            }
        }
        /// <summary>
        /// 抬起停止拖动
        /// </summary>
        internal static void Up(RangeBase owner, MouseButtonEventArgs e, ref Point? startPoint)
        {
            if (startPoint != null)
            {
                Calc(owner, startPoint.Value, e.GetPosition(owner));
                startPoint = null;
                //当控件具有鼠标捕获的话，则释放该捕获。
                owner.ReleaseMouseCapture();
                if (owner.Parent(out Window window)) window.Cursor = null;
            }
        }
        /// <summary>
        /// 计算移动距离
        /// </summary>
        private static void Calc(RangeBase owner, Point start, Point endPoint)
        {
            var x = Math.Pow(endPoint.X - start.X, 2.0);
            if (endPoint.X < start.X) x *= -1;
            var y = Math.Pow(endPoint.Y - start.Y, 2.0);
            if (endPoint.Y > start.Y) y *= -1;
            var interval = Math.Sqrt(Math.Abs(x + y));
            if (x + y < 0) interval *= -1;
            var percent = interval * 0.35 / owner.ActualWidth;
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) percent /= 3;
            else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) percent *= 3;
            var temp = owner.Value + (owner.Maximum - owner.Minimum) * percent;
            var count = (temp / owner.SmallChange).ToInt();
            temp = (count * owner.SmallChange).Clear();
            if (temp > owner.Maximum) temp = owner.Maximum;
            else if (temp < owner.Minimum) temp = owner.Minimum;
            if (owner.Value != temp) owner.Value = temp;
        }
    }
}
