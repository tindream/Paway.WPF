using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    public abstract partial class OperateItemModel : ViewModelBase, IPageReload
    {
        #region 属性
        protected DependencyObject Root;

        #endregion

        #region 权限控制
        public abstract string Menu { get; }
        private MenuAuthType _auth;
        /// <summary>
        /// 默认按钮权限
        /// </summary>
        public MenuAuthType Auth
        {
            get { return _auth; }
            set { _auth = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 菜单
        internal virtual void ActionInternal(string item)
        {
            try
            {
                Action(item);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new StatuMessage(ex));
            }
        }
        /// <summary>
        /// 按钮按键操作
        /// </summary>
        protected virtual void Action(string item)
        {
            switch (item)
            {
                case "刷新":
                    Refresh();
                    break;
                case "保存":
                    Save();
                    break;
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        protected virtual void Refresh() { }
        /// <summary>
        /// 保存
        /// </summary>
        protected virtual void Save() { }
        public ICommand ItemClickCommand => new RelayCommand<string>(item => ActionInternal(item));

        #endregion

        #region 搜索
        /// <summary>
        /// 搜索
        /// </summary>
        protected virtual void Search() { }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; RaisePropertyChanged(); Search(); }
        }
        /// <summary>
        /// 清空搜索框，不引发搜索事件
        /// </summary>
        protected void ClearSearch() { this._searchText = null; RaisePropertyChanged(nameof(SearchText)); }

        #endregion

        #region 按键
        /// <summary>
        /// 按键
        /// </summary>
        protected virtual void Action(KeyMessage msg)
        {
            if (Config.Menu != this.Menu) return;
            switch (msg.Key)
            {
                case Key.F5: ActionInternal("刷新"); break;
                case Key.Delete: ActionInternal("删除"); break;
            }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (msg.Key)
                {
                    case Key.A: ActionInternal("添加"); break;
                    case Key.E: ActionInternal("编辑"); break;
                    case Key.D: ActionInternal("删除"); break;
                    case Key.I: ActionInternal("导入"); break;
                    case Key.O: ActionInternal("导出"); break;
                    case Key.S: ActionInternal("保存"); break;
                    default: if (KeyCmdDic.ContainsKey(msg.Key)) ActionInternal(KeyCmdDic[msg.Key]); break;
                }
            }
        }

        #endregion

        #region 按钮
        /// <summary>
        /// 自定义按键快捷按键命令对应表
        /// </summary>
        private readonly Dictionary<Key, string> KeyCmdDic = new Dictionary<Key, string>();
        /// <summary>
        /// 默认权限
        /// <para>刷新、保存</para>
        /// </summary>
        protected virtual void AuthNormal()
        {
            this.Auth = MenuAuthType.Refresh | MenuAuthType.Save;
        }
        /// <summary>
        /// 添加竖线
        /// </summary>
        protected void AddInterval(FrameworkElement parent, Action<Border> action = null)
        {
            if (Method.Find(parent, out DockPanel dpOperateItem, "dpOperateItem"))
            {
                var border = new Border
                {
                    Style = parent.FindResource("Interval") as Style
                };
                action?.Invoke(border);
                dpOperateItem.Children.Insert(dpOperateItem.Children.Count - 1, border);
            }
        }
        /// <summary>
        /// 添加按钮
        /// <para>可指定图标、快捷键，可自定义</para>
        /// </summary>
        protected void AddButton(FrameworkElement parent, object cmd, ImageEXT imageEXT = null, Key key = Key.None, Action<ButtonEXT> action = null)
        {
            AddButton(parent, imageEXT, Dock.Top, cmd, cmd, key, cmd, action);
        }
        /// <summary>
        /// 添加按钮
        /// <para>可指定图标位置、命令、提示、快捷键，可自定义</para>
        /// </summary>
        protected void AddButton(FrameworkElement parent, ImageEXT imageEXT, Dock imageDock, object content, object cmd = null, Key key = Key.None, object toolTip = null, Action<ButtonEXT> action = null)
        {
            if (Method.Find(parent, out DockPanel dpOperateItem, "dpOperateItem"))
            {
                var btn = new ButtonEXT
                {
                    Style = parent.FindResource("MenuButton") as Style,
                    ItemBorder = new ThicknessEXT(0),
                    Content = content,
                    ToolTip = toolTip ?? content,
                    Image = imageEXT,
                    ImageDock = imageDock,
                    Command = ItemClickCommand,
                    CommandParameter = cmd ?? content
                };
                if (key != Key.None)
                {
                    if (KeyCmdDic.ContainsKey(key)) throw new WarningException($"按键{key}已存在");
                    KeyCmdDic.Add(key, (cmd ?? content).ToStrings());
                }
                action?.Invoke(btn);
                dpOperateItem.Children.Insert(dpOperateItem.Children.Count - 1, btn);
            }
        }
        /// <summary>
        /// 添加自定义控件
        /// </summary>
        protected void AddUIElement(FrameworkElement parent, Func<UIElement> func, int index = -1)
        {
            if (Method.Find(parent, out DockPanel dpOperateItem, "dpOperateItem"))
            {
                var element = func();
                dpOperateItem.Children.Insert(index != -1 ? index : dpOperateItem.Children.Count - 1, element);
            }
        }
        #endregion

        #region 页重加载
        public virtual void PageReload() { }

        #endregion

        public OperateItemModel()
        {
            AuthNormal();
            Messenger.Default.Register<KeyMessage>(this, msg => Action(msg));
        }
    }
}