using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// Window基础模型
    /// </summary>
    public class BaseWindowModel : ViewModelBasePlus, IWindowModel
    {
        #region 属性
        private string title;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        #endregion

        #region 命令
        /// <summary>
        /// 关闭处理
        /// </summary>
        public virtual bool? OnCancel(Window wd) { return false; }
        private readonly ICommand cancel;
        /// <summary>
        /// 点击关闭
        /// </summary>
        public ICommand Cancel => cancel;

        /// <summary>
        /// 提交处理
        /// </summary>
        public virtual bool? OnCommit(Window wd) { return true; }
        private readonly ICommand commit;
        /// <summary>
        /// 点击提交
        /// </summary>
        public ICommand Commit => commit;

        #endregion

        /// <summary>
        /// Window基础模型
        /// </summary>
        public BaseWindowModel()
        {
            //命令对象只创建一次，避免每次绑定取值都新建
            this.cancel = new RelayCommand<Window>(wd =>
            {
                try
                {
                    var result = OnCancel(wd);
                    if (wd != null) wd.DialogResult = result;
                }
                catch (Exception ex)
                {
                    WeakReferenceMessenger.Default.Send(new StatuMessage(ex, wd));
                }
            });
            this.commit = new RelayCommand<Window>(wd =>
            {
                try
                {
                    var result = OnCommit(wd);
                    if (wd != null) wd.DialogResult = result;
                }
                catch (Exception ex)
                {
                    WeakReferenceMessenger.Default.Send(new StatuMessage(ex, wd));
                }
            });
        }
    }
}