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
        private OperateItem operateItem;
        private DockPanel Panel;
        private bool iExit = false;
        private DateTime exitTime = DateTime.MinValue;

        #endregion

        #region 权限控制
        /// <summary>
        /// 默认按钮权限
        /// </summary>
        public MenuAuthType Auth
        {
            get { return operateItem?.Auth ?? MenuAuthType.None; }
            set { if (operateItem != null) operateItem.Auth = value; }
        }

        #endregion

        #region 菜单
        /// <summary>
        /// 通用动作命令-按钮按键操作
        /// </summary>
        public override bool Action(string item)
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
            return base.Action(item);
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
            ExcelBuilder.Create(file, list).Build();
            Messenger.Default.Send(new StatuMessage(PConfig.LanguageBase.ExportSuccess, obj));
            if (iOpen && PMethod.Ask(obj, PConfig.LanguageBase.ExportSuccessAndOpen))
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
            switch (msg.Key)
            {
                case Key.Escape:
                    if ((Auth & MenuAuthType.Search) != MenuAuthType.Search) break;
                    if (PMethod.Find(Panel, out TextBoxEXT tbSearch, "tbSearch"))
                    {
                        if (tbSearch.Text.IsEmpty()) break;
                        if (iExit && DateTime.Now.Subtract(exitTime).TotalMilliseconds < PConfig.DoubleInterval)
                        {
                            Messenger.Default.Send(new StatuMessage(PConfig.LanguageBase.QueryCancel));
                            tbSearch.Text = null;
                            msg.Handled = true;
                        }
                        else
                        {
                            iExit = true;
                            exitTime = DateTime.Now;
                            Messenger.Default.Send(new StatuMessage(PConfig.LanguageBase.QueryCancelAgain));
                            msg.Handled = true;
                        }
                    }
                    break;
                case Key.F:
                    if ((Auth & MenuAuthType.Search) != MenuAuthType.Search) break;
                    if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control) break;
                    if (PMethod.Find(Panel, out tbSearch, "tbSearch") && !tbSearch.IsKeyboardFocusWithin)
                    {
                        Messenger.Default.Send(new StatuMessage(PConfig.LanguageBase.Query));
                        tbSearch.Focus();
                        msg.Handled = true;
                    }
                    break;
            }
        }

        #endregion

        #region 按钮
        /// <summary>
        /// 默认权限
        /// <para>刷新、保存</para>
        /// </summary>
        protected virtual void AuthNormal()
        {
            if (Auth == MenuAuthType.None) this.Auth = MenuAuthType.Refresh | MenuAuthType.Save;
        }
        /// <summary>
        /// 添加竖线
        /// </summary>
        protected void AddInterval(int index = -1, Action<Border> action = null)
        {
            this.operateItem.AddInterval(index = -1, action);
        }
        /// <summary>
        /// 添加按钮
        /// <para>可指定图标、快捷键，可自定义</para>
        /// </summary>
        protected void AddButton(object content, ICommand command, ImageSourceEXT imageEXT = null, Key key = Key.None, int index = -1, Action<ButtonEXT> action = null)
        {
            this.operateItem.AddButton(imageEXT, Dock.Top, content, command, content, key, index, content, action);
        }
        /// <summary>
        /// 添加按钮
        /// <para>可指定图标位置、命令、提示、快捷键，可自定义</para>
        /// </summary>
        protected void AddButton(ImageSourceEXT imageEXT, Dock imageDock, object content, ICommand command, object cmd = null, Key key = Key.None, int index = -1, object toolTip = null, Action<ButtonEXT> action = null)
        {
            this.operateItem.AddButton(imageEXT, imageDock, content, command, cmd, key, index, toolTip, action);
        }
        /// <summary>
        /// 添加自定义控件
        /// </summary>
        protected void AddUIElement(Func<UIElement> func, int index = -1)
        {
            this.operateItem.AddUIElement(func, index);
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
            Messenger.Default.Register<OperateLoadMessage>(this, msg =>
            {
                //基类通知，如自定义菜单栏，此事件未触发，会被后续控件触发初始化
                if (this.Panel == null && msg.Obj is OperateItem operateItem)
                {
                    this.operateItem = operateItem;
                    if (PMethod.Find(operateItem, out DockPanel panel, "dpOperateItem"))
                    {
                        this.Panel = panel;
                        Messenger.Default.Register<KeyMessage>(this, panel.GetHashCode(), keyMsg => Action(keyMsg));
                    }
                    AuthNormal();
                }
            });
        }
    }
}