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
    public class BaseWindowModel : ViewModelBase
    {
        #region 属性
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 命令
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
        protected virtual bool? OnCancel() { return false; }
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
        protected virtual bool? OnCommit(Window wd) { return true; }

        #endregion
    }
}