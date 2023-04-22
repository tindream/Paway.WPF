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
    public class BaseWindowModel : ViewModelBasePlus
    {
        #region 属性
        public bool ILoad { get; set; }
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        #endregion

        #region 命令
        protected virtual bool? OnCancel() { return false; }
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

        protected virtual bool? OnCommit(Window wd) { return true; }
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