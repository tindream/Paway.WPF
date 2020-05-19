using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// Border样式基础控件
    /// </summary>
    public abstract partial class BorderControl : UserControl
    {
        private Point lastClickPoint;

        #region 点击验证
        /// <summary>
        /// Border点击触发
        /// </summary>
        protected virtual void ClickHandle(object sender) { }
        /// <summary>
        /// </summary>
        protected void Border_MouseDown(object sender, InputEventArgs e)
        {
            Point pointToWindow = Mouse.GetPosition(this);
            this.lastClickPoint = PointToScreen(pointToWindow);
        }
        /// <summary>
        /// </summary>
        protected void Border_MouseUp(object sender, InputEventArgs e)
        {
            if (this.lastClickPoint == new Point()) return;
            Point pointToWindow = Mouse.GetPosition(this);
            var point = PointToScreen(pointToWindow);
            if (sender is Border border && border.CaptureMouse())
            {
                border.ReleaseMouseCapture();
                ClickHandle(sender);
            }
            else if (Math.Abs(point.X - lastClickPoint.X) < 20 && Math.Abs(point.Y - lastClickPoint.Y) < 20)
            {
                ClickHandle(sender);
            }
            this.lastClickPoint = new Point();
        }

        #endregion
    }
}
