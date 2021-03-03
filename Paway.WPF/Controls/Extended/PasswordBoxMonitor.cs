using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// PasswordBox扩展监听
    /// </summary>
    internal class PasswordBoxMonitor : DependencyObject
    {
        #region 启用监听，设置水印大小，获取密码长度
        /// <summary>
        /// 自动获取当前密码框文本长度
        /// </summary>
        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached(nameof(PasswordLength), typeof(int), typeof(PasswordBoxMonitor));
        /// <summary>
        /// 启用监听，设置水印大小，获取密码长度
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is PasswordBox pad)
            {
                pad.SetValue(PasswordLengthProperty, pad.Password.Length);
                if ((bool)e.NewValue)
                {
                    pad.PasswordChanged += PasswordChanged;
                    pad.MouseEnter += Pad_MouseEnter;
                    pad.MouseLeave += Pad_MouseLeave;
                }
                else
                {
                    pad.PasswordChanged -= PasswordChanged;
                    pad.MouseEnter -= Pad_MouseEnter;
                    pad.MouseLeave -= Pad_MouseLeave;
                }
            }
        }
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pad)
            {
                pad.SetValue(PasswordLengthProperty, pad.Password.Length);
                pad.SetValue(PasswordBoxEXT.IsUpdatingProperty, true);
                pad.SetValue(PasswordBoxEXT.PasswordProperty, pad.Password);
                pad.SetValue(PasswordBoxEXT.IsUpdatingProperty, false);
            }
        }
        /// <summary>
        /// 启用监听
        /// </summary>
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }
        /// <summary>
        /// 自动获取当前密码框文本长度
        /// </summary>
        public int PasswordLength
        {
            get { return (int)GetValue(PasswordLengthProperty); }
            set { SetValue(PasswordLengthProperty, value); }
        }

        #endregion

        #region 动画
        /// <summary>
        /// 鼠标进入时启动
        /// </summary>
        private static void Pad_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is PasswordBox pad)
            {
                var iAnimation = (double)pad.GetValue(PasswordBoxEXT.IAnimationProperty);
                if (iAnimation > 0) Animation(pad, true);
            }
        }
        /// <summary>
        /// 鼠标离开时关闭
        /// </summary>
        private static void Pad_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is PasswordBox pad)
            {
                var iAnimation = (double)pad.GetValue(PasswordBoxEXT.IAnimationProperty);
                if (iAnimation > 0) Animation(pad, false);
            }
        }
        private static void Animation(PasswordBox pad, bool value)
        {
            TMethod.Child(pad, out Line line1, "line1", false);
            TMethod.Child(pad, out Line line2, "line2", false);
            var storyboard = new Storyboard();
            if (value)
            {
                var animX1 = new DoubleAnimation(line1.X1, pad.BorderThickness.Left, new Duration(TimeSpan.FromMilliseconds(100)));
                var animX2 = new DoubleAnimation(line2.X2, pad.ActualWidth / 2 - pad.BorderThickness.Right, new Duration(TimeSpan.FromMilliseconds(100)));
                if (line1 != null)
                {
                    Storyboard.SetTargetName(animX1, line1.Name);
                    Storyboard.SetTargetProperty(animX1, new PropertyPath(Line.X1Property));
                    storyboard.Children.Add(animX1);
                }
                if (line2 != null)
                {
                    Storyboard.SetTargetName(animX2, line2.Name);
                    Storyboard.SetTargetProperty(animX2, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX2);
                }
            }
            else
            {
                var animX1 = new DoubleAnimation(line1.X1, pad.ActualWidth / 2, new Duration(TimeSpan.FromMilliseconds(100)));
                var animX2 = new DoubleAnimation(line2.X2, 0, new Duration(TimeSpan.FromMilliseconds(100)));
                if (line1 != null)
                {
                    Storyboard.SetTargetName(animX1, line1.Name);
                    Storyboard.SetTargetProperty(animX1, new PropertyPath(Line.X1Property));
                    storyboard.Children.Add(animX1);
                }
                if (line2 != null)
                {
                    Storyboard.SetTargetName(animX2, line2.Name);
                    Storyboard.SetTargetProperty(animX2, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX2);
                }
            }
            if (line1 != null) storyboard.Begin(line1);
        }

        #endregion
    }
}
