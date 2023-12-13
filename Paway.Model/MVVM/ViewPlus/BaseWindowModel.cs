using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
    public class BaseWindowModel : ViewModelBasePlus
    {
        #region 属性
        /// <summary>
        /// 加载状态
        /// </summary>
        public bool ILoad { get; set; }
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
        protected virtual bool? OnCancel() { return false; }
        /// <summary>
        /// 点击关闭
        /// </summary>
        public ICommand Cancel => new RelayCommand<Window>(wd =>
        {
            try
            {
                wd.DialogResult = OnCancel();
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new StatuMessage(ex, wd));
            }
        });

        /// <summary>
        /// 提交处理
        /// </summary>
        protected virtual bool? OnCommit(Window wd) { return true; }
        /// <summary>
        /// 点击提交
        /// </summary>
        public ICommand Commit => new RelayCommand<Window>(wd =>
        {
            try
            {
                wd.DialogResult = OnCommit(wd);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new StatuMessage(ex, wd));
            }
        });
        internal void EnterCommit(Window wd)
        {
            wd.DialogResult = OnCommit(wd);
        }

        #endregion
    }
}