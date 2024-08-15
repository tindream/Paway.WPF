using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Paway.Helper;
using Paway.WPF;
using Paway.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// 模型-添加窗口
    /// </summary>
    public class AddWindowModel<T> : BaseWindowModel where T : class, ICompare<T>, IId
    {
        #region 属性
        private T info;
        private T normal;
        /// <summary>
        /// 模型
        /// <para>复制体，修改不影响原实体</para>
        /// </summary>
        public T Info
        {
            get { return info; }
            set
            {
                normal = value;
                info = value.Clone();
                Title = info.Id == 0 ? $"{PConfig.LanguageBase.Add} - {info.GetType().Description()}" : $"{info.GetType().Description()} - {info}";
                OnPropertyChanged();
                ReLoad();
            }
        }

        #endregion

        #region 命令
        /// <summary>
        /// 模型加载后
        /// </summary>
        protected virtual void ReLoad() { }
        /// <summary>
        /// 检查模型中的值
        /// <para>输入控件限定为TextBoxEXT，控件名称为tb+name</para>
        /// </summary>
        protected bool ValidationError(Window wd, string name, bool allEmpty = false)
        {
            return PMethod.ValidationError(wd, info, name, allEmpty);
        }
        /// <summary>
        /// 确认保存方法
        /// </summary>
        protected virtual bool? OnSave(Window wd, T info)
        {
            var errorList = PMethod.ValidationError(wd);
            if (errorList.Count > 0)
            {
                PMethod.Hit(wd, errorList.Join("\r\n"), ColorType.Error);
                return null;
            }
            if (info is IChecked @checked) @checked.Checked();
            else if (Cache.Any<T>(c => c.Compare(info))) throw new WarningException($"[{typeof(T).Description()}]{info} {PConfig.LanguageBase.Exist}");
            if (info.Id == 0 && info is IIndex index)
            {
                var tList = Cache.FindAll<T>();
                index.Index = tList.Count == 0 ? 0 : tList.Max(c => ((IIndex)c).Index) + 1;
            }
            return true;
        }
        /// <summary>
        /// 点击确认
        /// </summary>
        public override bool? OnCommit(Window wd)
        {
            var result = OnSave(wd, info);
            if (result != true) return result;
            info.Clone(normal);
            return base.OnCommit(wd);
        }

        #endregion

        /// <summary>
        /// 模型添加窗口
        /// </summary>
        public AddWindowModel() { }
    }
}