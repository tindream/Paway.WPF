using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    public class AddWindowModel<T> : BaseWindowModel where T : class, IId
    {
        #region 属性
        private T info;
        private T normal;
        public T Info
        {
            get { return info; }
            set
            {
                normal = value;
                info = value.Clone();
                Title = info.Id == 0 ? $"新加{info.GetType().Description()}" : $"编辑{info.GetType().Description()} - {info}";
                RaisePropertyChanged();
                ReLoad();
            }
        }

        #endregion

        #region 命令
        protected virtual void ReLoad() { }
        protected virtual bool? OnSave(Window wd, T info) { return true; }
        protected override bool? OnCommit(Window wd)
        {
            if (info is IChecked @checked)
            {
                @checked.Checked();
            }
            OnSave(wd, info);
            info.Clone(normal);
            return base.OnCommit(wd);
        }

        #endregion

        public AddWindowModel() { }
    }
}