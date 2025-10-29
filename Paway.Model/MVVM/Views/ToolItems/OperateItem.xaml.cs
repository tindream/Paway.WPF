using GalaSoft.MvvmLight.Messaging;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace Paway.Model
{
    /// <summary>
    /// OperateItem.xaml 的交互逻辑
    /// </summary>
    public partial class OperateItem
    {
        #region 扩展参数
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty AuthProperty =
            DependencyProperty.Register(nameof(Auth), typeof(MenuAuthType), typeof(OperateItem), new PropertyMetadata(MenuAuthType.Refresh | MenuAuthType.Save));
        /// <summary>
        /// 默认按钮权限
        /// <para>默认值：刷新、保存</para>
        /// </summary>
        [Category("扩展")]
        [Description("默认按钮权限")]
        public MenuAuthType Auth
        {
            get { return (MenuAuthType)GetValue(AuthProperty); }
            set { SetValue(AuthProperty, value); }
        }

        #endregion
        #region 操作
        /// <summary>
        /// 添加竖线
        /// </summary>
        public void AddInterval(int index = -1, Action<Border> action = null)
        {
            var border = new Border
            {
                Style = this.dpOperateItem.FindResource("Interval") as Style
            };
            action?.Invoke(border);
            this.dpOperateItem.Children.Insert(index != -1 ? index : this.dpOperateItem.Children.Count - 1, border);
        }
        /// <summary>
        /// 添加按钮
        /// <para>可指定图标、快捷键，可自定义</para>
        /// </summary>
        public void AddButton(object content, ICommand command, ImageSourceEXT imageEXT = null, Key key = Key.None, int index = -1, Action<ButtonEXT> action = null)
        {
            AddButton(imageEXT, Dock.Top, content, command, content, key, index, content, action);
        }
        /// <summary>
        /// 添加按钮
        /// <para>可指定图标位置、命令、提示、快捷键，可自定义</para>
        /// </summary>
        public void AddButton(ImageSourceEXT imageEXT, Dock imageDock, object content, ICommand command, object cmd = null, Key key = Key.None, int index = -1, object toolTip = null, Action<ButtonEXT> action = null)
        {
            var btn = new ButtonEXT
            {
                Style = this.dpOperateItem.FindResource("MenuButton") as Style,
                ItemBorder = new ThicknessEXT(0),
                Content = content,
                ToolTip = toolTip ?? content,
                Image = imageEXT,
                ImageDock = imageDock,
                Command = command,
                CommandParameter = cmd ?? content
            };
            if (key != Key.None)
            {
                btn.ShortcutKey = key;
                btn.ShortcutControl = ModifierKeys.Control;
            }
            action?.Invoke(btn);
            this.dpOperateItem.Children.Insert(index != -1 ? index : this.dpOperateItem.Children.Count - 1, btn);
        }
        /// <summary>
        /// 添加自定义控件
        /// </summary>
        public void AddUIElement(Func<UIElement> func, int index = -1)
        {
            var element = func();
            this.dpOperateItem.Children.Insert(index != -1 ? index : this.dpOperateItem.Children.Count - 1, element);
        }

        #endregion

        /// <summary>
        /// 通用工具栏
        /// </summary>
        public OperateItem()
        {
            InitializeComponent();
            Messenger.Default.Send(new OperateLoadMessage() { Obj = this });
            this.Loaded += OperateItem_Loaded;
        }
        /// <summary>
        /// 监听顶层窗体按键
        /// </summary>
        private void OperateItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (PMethod.Parent(this, out Window window))
            {
                window.PreviewKeyDown += Window_PreviewKeyDown;
            }
            this.Loaded -= OperateItem_Loaded;
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!this.IsLoaded) return;
            var keyMsg = new KeyMessage(e.Key);
            Messenger.Default.Send(keyMsg, this.dpOperateItem.GetHashCode());
            e.Handled = keyMsg.Handled;
        }
    }
}
