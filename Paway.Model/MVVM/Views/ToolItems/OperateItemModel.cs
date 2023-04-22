using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.Utils;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    public partial class OperateItemModel : ViewModelBasePlus, IPageReload
    {
        #region 属性
        private DockPanel DockPanel;
        private bool iExit = false;
        private DateTime exitTime = DateTime.MinValue;

        #endregion

        #region 权限控制
        public virtual string Menu { get; }
        private MenuAuthType _auth;
        /// <summary>
        /// 默认按钮权限
        /// </summary>
        public MenuAuthType Auth
        {
            get { return _auth; }
            set { _auth = value; OnPropertyChanged(); }
        }

        #endregion

        #region 菜单
        /// <summary>
        /// 发送状态并执行
        /// </summary>
        internal void ActionInternalMsg(string item)
        {
            Messenger.Default.Send(new StatuMessage(item, false));
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
        protected override void Action(string item)
        {
            base.Action(item);
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
        protected virtual void Refresh(Action action = null) { }
        /// <summary>
        /// 保存
        /// </summary>
        protected virtual void Save() { }
        /// <summary>
        /// 导出列表
        /// </summary>
        protected virtual void Export<T>(DependencyObject obj, List<T> list, string file, bool iOpen = true) where T : class
        {
            ExcelHelper.ToExcel(list, null, file);
            Messenger.Default.Send(new StatuMessage("导出成功", obj));
            if (iOpen && Method.Ask(obj, "导出成功,是否打开文件?"))
            {
                Process.Start(file);
            }
        }

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
            set { _searchText = value; OnPropertyChanged(); Search(); }
        }
        /// <summary>
        /// 清空搜索框，不引发搜索事件
        /// </summary>
        protected void ClearSearch() { this._searchText = null; OnPropertyChanged(nameof(SearchText)); }

        #endregion

        #region 按键
        /// <summary>
        /// 按键
        /// </summary>
        protected virtual void Action(KeyMessage msg)
        {
            if (Config.Menu != this.Menu) return;
            if (DockPanel?.Visibility != Visibility.Visible) return;
            switch (msg.Key)
            {
                case Key.F5: if ((Auth & MenuAuthType.Refresh) == MenuAuthType.Refresh) ActionInternalMsg("刷新"); break;
                case Key.Delete: if ((Auth & MenuAuthType.Delete) == MenuAuthType.Delete) ActionInternalMsg("删除"); break;
                case Key.Escape:
                    if (Method.Find(DockPanel, out TextBoxEXT tbSearch, "tbSearch"))
                    {
                        if (tbSearch.Text.IsEmpty()) break;
                        if (iExit && DateTime.Now.Subtract(exitTime).TotalMilliseconds < Config.DoubleInterval)
                        {
                            Messenger.Default.Send(new StatuMessage("取消查询", false));
                            tbSearch.Text = null;
                        }
                        else
                        {
                            iExit = true;
                            exitTime = DateTime.Now;
                            Messenger.Default.Send(new StatuMessage("再按一次取消查询", false));
                        }
                    }
                    break;
            }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (KeyCmdDic.ContainsKey(msg.Key)) ActionInternalMsg(KeyCmdDic[msg.Key]);
                else
                {
                    switch (msg.Key)
                    {
                        case Key.A: if ((Auth & MenuAuthType.Add) == MenuAuthType.Add) ActionInternalMsg("添加"); break;
                        case Key.E: if ((Auth & MenuAuthType.Edit) == MenuAuthType.Edit) ActionInternalMsg("编辑"); break;
                        case Key.D: if ((Auth & MenuAuthType.Delete) == MenuAuthType.Delete) ActionInternalMsg("删除"); break;
                        case Key.I: if ((Auth & MenuAuthType.Import) == MenuAuthType.Import) ActionInternalMsg("导入"); break;
                        case Key.O: if ((Auth & MenuAuthType.Export) == MenuAuthType.Export) ActionInternalMsg("导出"); break;
                        case Key.S: if ((Auth & MenuAuthType.Save) == MenuAuthType.Save) ActionInternalMsg("保存"); break;
                        case Key.F:
                            if (Method.Find(DockPanel, out TextBoxEXT tbSearch, "tbSearch") && !tbSearch.IsKeyboardFocusWithin)
                            {
                                Messenger.Default.Send(new StatuMessage("查询", false));
                                tbSearch.Focus();
                            }
                            break;
                    }
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
        protected void AddInterval(int index = -1, Action<Border> action = null)
        {
            var border = new Border
            {
                Style = DockPanel.FindResource("Interval") as Style
            };
            action?.Invoke(border);
            DockPanel.Children.Insert(index != -1 ? index : DockPanel.Children.Count - 1, border);
        }
        /// <summary>
        /// 添加按钮
        /// <para>可指定图标、快捷键，可自定义</para>
        /// </summary>
        protected void AddButton(object cmd, ImageEXT imageEXT = null, Key key = Key.None, int index = -1, Action<ButtonEXT> action = null)
        {
            var content = cmd;
            if (key != Key.None) content = $"{cmd}({key})";
            AddButton(imageEXT, Dock.Top, content, cmd, key, index, cmd, action);
        }
        /// <summary>
        /// 添加按钮
        /// <para>可指定图标位置、命令、提示、快捷键，可自定义</para>
        /// </summary>
        protected void AddButton(ImageEXT imageEXT, Dock imageDock, object content, object cmd = null, Key key = Key.None, int index = -1, object toolTip = null, Action<ButtonEXT> action = null)
        {
            var btn = new ButtonEXT
            {
                Style = DockPanel.FindResource("MenuButton") as Style,
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
            DockPanel.Children.Insert(index != -1 ? index : DockPanel.Children.Count - 1, btn);
        }
        /// <summary>
        /// 添加自定义控件
        /// </summary>
        protected void AddUIElement(Func<UIElement> func, int index = -1)
        {
            var element = func();
            DockPanel.Children.Insert(index != -1 ? index : DockPanel.Children.Count - 1, element);
        }
        #endregion

        #region 页重加载
        public bool ILoad { get; set; }
        public virtual void PageReload() { }

        #endregion

        public OperateItemModel()
        {
            this.Menu = this.GetType().Description();
            Messenger.Default.Register<KeyMessage>(this, msg => Action(msg));
            Messenger.Default.Register<OperateLoadMessage>(this, msg =>
            {
                if (this.DockPanel == null && msg.Obj is DockPanel dockPanel)
                {
                    this.DockPanel = dockPanel;
                    AuthNormal();
                }
            });
        }
    }
}