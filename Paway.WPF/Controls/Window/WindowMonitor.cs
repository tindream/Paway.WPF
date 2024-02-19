using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Paway.WPF
{
    /// <summary>
    /// for custom window
    /// </summary>
    public partial class WindowMonitor : DependencyObject
    {
        #region 依赖属性
        /// <summary>
        /// 绑定命令
        /// </summary>
        public static readonly DependencyProperty IsBindingToSystemCommandsProperty =
            DependencyProperty.RegisterAttached(nameof(IsBindingToSystemCommands), typeof(bool), typeof(WindowMonitor), new PropertyMetadata(default(bool), OnIsBindingToSystemCommandsChanged));
        /// <summary>
        /// 绑定命令
        /// </summary>
        public bool IsBindingToSystemCommands
        {
            get { return (bool)GetValue(IsBindingToSystemCommandsProperty); }
            set { SetValue(IsBindingToSystemCommandsProperty, value); }
        }
        private static void OnIsBindingToSystemCommandsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue && obj is Window window)
            {
                var service = new WindowCommandHelper(window);
                service.ActiveCommands();
            }
        }

        /// <summary>
        /// 允许移动
        /// </summary>
        public static readonly DependencyProperty IsDragMoveEnabledProperty =
            DependencyProperty.RegisterAttached(nameof(IsDragMoveEnabled), typeof(bool), typeof(WindowMonitor), new PropertyMetadata(default(bool), OnIsDragMoveEnabledChanged));
        /// <summary>
        /// 允许移动
        /// <para>默认值：false</para>
        /// </summary>
        public bool IsDragMoveEnabled
        {
            get { return (bool)GetValue(IsDragMoveEnabledProperty); }
            set { SetValue(IsDragMoveEnabledProperty, value); }
        }
        private static void OnIsDragMoveEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue) return;

            if (obj is Window target)
            {
                if (newValue) target.MouseLeftButtonDown += OnWindowMouseLeftButtonDown;
                else target.MouseLeftButtonDown -= OnWindowMouseLeftButtonDown;
            }
        }
        private static void OnWindowMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && sender is Window target)
            {
                target.DragMove();
            }
        }

        #endregion
    }
}
