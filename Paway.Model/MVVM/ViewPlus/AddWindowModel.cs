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
                Title = info.Id == 0 ? $"新加 - {info.GetType().Description()}" : $"{info.GetType().Description()} - {info}";
                RaisePropertyChanged();
                ReLoad();
            }
        }

        #endregion

        #region 命令
        protected virtual void ReLoad() { }
        /// <summary>
        /// 检查模型中的值
        /// <para>输入控件限定为TextBoxEXT，控件名称为tb+name</para>
        /// </summary>
        protected bool ValidationError(Window wd, string name, bool allEmpty = false)
        {
            return Method.ValidationError(wd, info, name, allEmpty);
        }
        protected virtual bool? OnSave(Window wd, T info)
        {
            var errorList = Method.ValidationError(wd);
            if (errorList.Count > 0)
            {
                Method.Hit(wd, errorList.Join("\r\n"), ColorType.Error);
                return null;
            }
            if (info is IChecked @checked)
            {
                @checked.Checked();
            }
            if (info.Id == 0 && info is IIndex index)
            {
                var tList = Cache.List<T>();
                index.Index = tList.Count == 0 ? 0 : tList.Max(c => ((IIndex)c).Index) + 1;
            }
            return true;
        }
        protected override bool? OnCommit(Window wd)
        {
            var result = OnSave(wd, info);
            if (result != true) return result;
            info.Clone(normal);
            return base.OnCommit(wd);
        }

        #endregion

        public AddWindowModel() { }
    }
}