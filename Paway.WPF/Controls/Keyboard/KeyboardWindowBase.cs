using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 虚拟键盘窗口基类
    /// <para>全键盘与数字键盘窗口只有内容控件不同，外壳行为（标题栏、定位、不抢焦点、关闭转发）统一在这里，
    /// 避免两份几乎相同的窗口过程钩子实现各自维护。</para>
    /// </summary>
    public abstract class KeyboardWindowBase : WindowEXT
    {
        private bool _firstRender;
        private readonly NoActivateWindowHook hook;
        /// <summary>
        /// 键盘操作的输入元素
        /// <para>为空表示窗口独立使用，首次渲染时定位到屏幕底部居中</para>
        /// </summary>
        protected readonly FrameworkElement element;
        /// <summary>
        /// 关闭事件
        /// </summary>
        public event EventHandler<RoutedEventArgs> CloseEvent;

        /// <summary>
        /// </summary>
        /// <param name="element">键盘操作的输入元素，可为空</param>
        protected KeyboardWindowBase(FrameworkElement element)
        {
            this.element = element;
            this.hook = new NoActivateWindowHook(this);
        }
        /// <summary>
        /// 标题栏元素，由具体窗口提供
        /// </summary>
        protected abstract FrameworkElement TitleBar { get; }

        /// <summary>
        /// 初始化标题栏与窗口高度
        /// </summary>
        protected void InitTitle(bool iTitle)
        {
            this.TitleBar.Visibility = iTitle ? Visibility.Visible : Visibility.Collapsed;
            this.Height = iTitle ? 280 : 246;
        }
        /// <summary>
        /// 位置
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (element == null && !_firstRender)
            {
                _firstRender = true;
                // 获取主屏幕的工作区大小（不包括任务栏）
                this.Left = (SystemParameters.WorkArea.Width - this.ActualWidth) / 2;
                this.Top = SystemParameters.WorkArea.Height - this.ActualHeight;
            }
        }
        /// <summary>
        /// 键盘拖动结束后把焦点还给输入元素
        /// </summary>
        protected void OnDragMoved(object sender, MouseButtonEventArgs e)
        {
            this.element?.Focus();
        }
        /// <summary>
        /// 关闭：外部订阅了事件就交给外部关闭，否则直接关闭
        /// </summary>
        protected void OnCloseClick(object sender, RoutedEventArgs e)
        {
            if (CloseEvent != null) CloseEvent.Invoke(sender, e);
            else this.Close();
        }
        /// <summary>
        /// 恢复窗口过程
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            this.hook.Dispose();
            base.OnClosed(e);
        }
    }
}
