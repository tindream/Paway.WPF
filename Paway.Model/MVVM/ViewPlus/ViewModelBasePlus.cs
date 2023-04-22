using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
    public class ViewModelBasePlus : ViewModelBase
    {
        #region 属性
        /// <summary>
        /// 触发更新
        /// </summary>
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            RaisePropertyChanged(name);
        }

        #endregion

        #region 命令
        protected virtual void Action(ButtonEXT btn) { }
        public ICommand ButtonClickCommand => new RelayCommand<ButtonEXT>(btn =>
        {
            try
            {
                Action(btn);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new StatuMessage(ex, btn));
            }
        });

        protected virtual void Action(ListViewCustom listView1) { }
        public ICommand SelectionCommand => new RelayCommand<ListViewCustom>(listView1 =>
        {
            try
            {
                Action(listView1);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new StatuMessage(ex, listView1));
            }
        });

        protected virtual void Action(string item) { }
        public ICommand ItemClickCommand => new RelayCommand<string>(item =>
        {
            try
            {
                Action(item);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new StatuMessage(ex));
            }
        });

        #endregion
    }
}