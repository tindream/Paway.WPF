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

namespace Invengo.Utils
{
    /// <summary>
    /// Border样式基础控件
    /// </summary>
    public abstract partial class BorderControlBase : UserControl
    {
        private Point lastClickPoint;

        #region 点击验证
        protected virtual void ClickHandle(object sender) { }
        protected void Border_MouseDown(object sender, InputEventArgs e)
        {
            Point pointToWindow = Mouse.GetPosition(this);
            this.lastClickPoint = PointToScreen(pointToWindow);
        }
        protected void Border_MouseUp(object sender, InputEventArgs e)
        {
            if (this.lastClickPoint == new Point()) return;
            Point pointToWindow = Mouse.GetPosition(this);
            var point = PointToScreen(pointToWindow);
            if (Math.Abs(point.X - lastClickPoint.X) < 20 && Math.Abs(point.Y - lastClickPoint.Y) < 20)
            {
                ClickHandle(sender);
            }
            this.lastClickPoint = new Point();
        }

        #endregion

        #region 按钮样式
        protected virtual void YellowHandle(string borderName) { }
        protected virtual void BlueHandle(string borderName) { }
        protected virtual void WhileHandle(string borderName) { }
        protected virtual void TransHandle(string borderName) { }
        protected virtual void Yellow_MouseMove(object sender, InputEventArgs e)
        {
            var border = sender as Border;
            border.Background = new SolidColorBrush(Color.FromArgb(255, 255, 144, 71));
            YellowHandle(border.Name);
        }
        protected virtual void Blue_MouseMove(object sender, InputEventArgs e)
        {
            var border = sender as Border;
            border.Background = new SolidColorBrush(Color.FromArgb(170, 35, 175, 255));
            BlueHandle(border.Name);
        }
        protected virtual void Blue_MouseDown(object sender, InputEventArgs e)
        {
            var border = sender as Border;
            border.Background = new SolidColorBrush(Color.FromArgb(230, 35, 175, 255));
            BlueHandle(border.Name);
        }
        protected virtual void BlueBorder_MouseMove(object sender, InputEventArgs e)
        {
            var border = sender as Border;
            border.BorderBrush = new SolidColorBrush(Color.FromArgb(200, 35, 175, 255));
            BlueHandle(border.Name);
        }
        protected virtual void While_MouseLeave(object sender, InputEventArgs e)
        {
            var border = sender as Border;
            border.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            WhileHandle(border.Name);
        }
        protected virtual void Trans_MouseLeave(object sender, InputEventArgs e)
        {
            var border = sender as Border;
            border.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            TransHandle(border.Name);
        }
        protected virtual void TransBorder_MouseLeave(object sender, InputEventArgs e)
        {
            var border = sender as Border;
            border.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            TransHandle(border.Name);
        }

        #endregion
    }
}
