using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// 模型基础处理
    /// </summary>
    public class ViewModelBasePlus : ObservableObject
    {
        #region 属性
        /// <summary>
        /// 触发更新
        /// </summary>
        public void OnPropertyChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            OnPropertyChanged(propertyName);
        }

        #endregion

        #region 命令
        /// <summary>
        /// 按钮通用动作命令
        /// </summary>
        protected virtual void Action(ButtonEXT btn) { }
        /// <summary>
        /// 点击按钮
        /// </summary>
        public ICommand ButtonClickCommand => new RelayCommand<ButtonEXT>(btn =>
        {
            try
            {
                Action(btn);
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new StatuMessage(ex, btn));
            }
        });

        /// <summary>
        /// 列表通用动作命令
        /// </summary>
        protected virtual void Action(ListViewCustom listView1) { }
        /// <summary>
        /// 选中列表项
        /// </summary>
        public ICommand SelectionCommand => new RelayCommand<ListViewCustom>(listView1 =>
        {
            try
            {
                Action(listView1);
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new StatuMessage(ex, listView1));
            }
        });

        /// <summary>
        /// 通用动作命令
        /// <para>默认返回值 true</para>
        /// </summary>
        public virtual bool Action(string item) { return true; }
        /// <summary>
        /// 点击列表项
        /// </summary>
        public ICommand ItemClickCommand => new RelayCommand<string>(item =>
        {
            try
            {
                Action(item);
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new StatuMessage(ex));
            }
        });

        #endregion
    }
}