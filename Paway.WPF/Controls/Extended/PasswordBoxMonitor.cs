using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        /// 启用监听，设置水印大小，获取密码长度
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(PasswordBoxMonitor), new PropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is PasswordBox pad)
            {
                if (PasswordBoxEXT.GetWater(pad) == null)
                {
                    PasswordBoxEXT.SetWater(pad, PConfig.LanguageBase.PleaseInputPasswordWater);
                    var waterBinding = new Binding
                    {
                        Source = PConfig.LanguageBase,//设置要绑定源-语言类
                        Path = new PropertyPath(nameof(PConfig.LanguageBase.PleaseInputPasswordWater)),//绑定绑定源下的属性。
                        Mode = BindingMode.OneWay//绑定模式单向
                    };
                    pad.SetBinding(PasswordBoxEXT.WaterProperty, waterBinding);//设置绑定到要绑定的控件
                }
                pad.SetValue(PasswordLengthProperty, pad.Password.Length);
                pad.PasswordChanged -= PasswordChanged;
                pad.MouseEnter -= Pad_MouseEnter;
                pad.MouseLeave -= Pad_MouseLeave;
                if ((bool)e.NewValue)
                {
                    pad.PasswordChanged += PasswordChanged;
                    pad.MouseEnter += Pad_MouseEnter;
                    pad.MouseLeave += Pad_MouseLeave;
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

        #endregion

        #region 动画
        /// <summary>
        /// 鼠标进入时启动
        /// </summary>
        private static void Pad_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is PasswordBox pad)
            {
                var animation = (double)pad.GetValue(PasswordBoxEXT.AnimationProperty);
                if (animation > 0) PMethod.Animation(pad, true);
            }
        }
        /// <summary>
        /// 鼠标离开时关闭
        /// </summary>
        private static void Pad_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is PasswordBox pad)
            {
                var animation = (double)pad.GetValue(PasswordBoxEXT.AnimationProperty);
                if (animation > 0) PMethod.Animation(pad, false);
            }
        }

        #endregion

        #region 依赖属性
        /// <summary>
        /// 自动获取当前密码框文本长度
        /// </summary>
        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached(nameof(PasswordLength), typeof(int), typeof(PasswordBoxMonitor));
        /// <summary>
        /// 自动获取当前密码框文本长度
        /// </summary>
        public int PasswordLength
        {
            get { return (int)GetValue(PasswordLengthProperty); }
            set { SetValue(PasswordLengthProperty, value); }
        }

        #endregion
    }
}
