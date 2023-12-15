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
    /// <summary>
    /// 通用工具栏模型
    /// </summary>
    public partial class OperateItemModel : ViewModelBasePlus, IPageReload
    {
        #region 属性
        private Panel Panel;
        private bool iExit = false;
        private DateTime exitTime = DateTime.MinValue;

        #endregion

        #region 权限控制
        /// <summary>
        /// 所属菜单
        /// </summary>
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
        /// 通用动作命令-按钮按键操作
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
            if (iOpen && MMethod.Ask(obj, "导出成功,是否打开文件?"))
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
        /// <summary>
        /// 当前搜索框绑定
        /// </summary>
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
            if (MConfig.Menu != this.Menu) return;
            switch (msg.Key)
            {
                case Key.F5: if ((Auth & MenuAuthType.Refresh) == MenuAuthType.Refresh) ActionInternalMsg("刷新"); break;
                case Key.Delete: if ((Auth & MenuAuthType.Delete) == MenuAuthType.Delete) ActionInternalMsg("删除"); break;
                case Key.Escape:
                    if ((Auth & MenuAuthType.Search) != MenuAuthType.Search) break;
                    if (MMethod.Find(Panel, out TextBoxEXT tbSearch, "tbSearch"))
                    {
                        if (tbSearch.Text.IsEmpty()) break;
                        if (iExit && DateTime.Now.Subtract(exitTime).TotalMilliseconds < MConfig.DoubleInterval)
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
                            if ((Auth & MenuAuthType.Search) != MenuAuthType.Search) break;
                            if (MMethod.Find(Panel, out TextBoxEXT tbSearch, "tbSearch") && !tbSearch.IsKeyboardFocusWithin)
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
                Style = Panel.FindResource("Interval") as Style
            };
            action?.Invoke(border);
            Panel.Children.Insert(index != -1 ? index : Panel.Children.Count - 1, border);
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
                Style = Panel.FindResource("MenuButton") as Style,
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
            Panel.Children.Insert(index != -1 ? index : Panel.Children.Count - 1, btn);
        }
        /// <summary>
        /// 添加自定义控件
        /// </summary>
        protected void AddUIElement(Func<UIElement> func, int index = -1)
        {
            var element = func();
            Panel.Children.Insert(index != -1 ? index : Panel.Children.Count - 1, element);
        }
        #endregion

        #region 页重加载
        /// <summary>
        /// 加载状态
        /// </summary>
        public bool ILoad { get; set; }
        /// <summary>
        /// 在Loaded第一次触发或重加载时调用
        /// </summary>
        public virtual void PageReload() { }

        #endregion

        /// <summary>
        /// 通用工具栏模型
        /// </summary>
        public OperateItemModel()
        {
            this.Menu = this.GetType().Description();
            Messenger.Default.Register<KeyMessage>(this, msg => Action(msg));
            Messenger.Default.Register<OperateLoadMessage>(this, msg =>
            {
                //基类通知，如自定义菜单栏，此事件未触发，会被后续控件触发初始化
                if (this.Panel == null && msg.Obj is Panel panel)
                {
                    this.Panel = panel;
                    if (Auth == MenuAuthType.None) AuthNormal();
                }
            });
        }
    }
}