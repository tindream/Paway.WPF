using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.WPF
{
    /// <summary>
    /// TreeView数接口据定义
    /// </summary>
    public interface ITreeView
    {
        /// <summary>
        /// 标识符
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// 组标记
        /// </summary>
        bool IsGrouping { get; set; }
        /// <summary>
        /// 组组名
        /// </summary>
        string GroupName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        string ShortName { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        string Subtitle { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        string Desc { get; set; }

        /// <summary>
        /// 选中标记
        /// </summary>
        bool? IsChecked { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        ITreeView Parent { get; set; }
        /// <summary>
        /// 子级列表
        /// </summary>
        ObservableCollection<ITreeView> Children { get; set; }
    }
    /// <summary>
    /// ITreeView数据模型
    /// </summary>
    public class TreeViewModel : ModelBase, ITreeView
    {
        private int id;
        /// <summary>
        /// 标识符
        /// </summary>
        [NoShow]
        public virtual int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        private bool isGrouping;
        /// <summary>
        /// 组标记
        /// </summary>
        public virtual bool IsGrouping
        {
            get { return isGrouping; }
            set { isGrouping = value; OnPropertyChanged(); }
        }
        private string groupName;
        /// <summary>
        /// 组组名
        /// </summary>
        public virtual string GroupName
        {
            get { return groupName; }
            set { groupName = value; OnPropertyChanged(); }
        }
        private string shortName;
        /// <summary>
        /// 头像
        /// </summary>
        public virtual string ShortName
        {
            get { return shortName; }
            set { shortName = value; OnPropertyChanged(); }
        }
        private string text;
        /// <summary>
        /// 文本
        /// </summary>
        public virtual string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged(); }
        }
        private string subtitle;
        /// <summary>
        /// 副标题
        /// </summary>
        public virtual string Subtitle
        {
            get { return subtitle; }
            set { subtitle = value; OnPropertyChanged(); }
        }
        private string desc;
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Desc
        {
            get { return desc; }
            set { desc = value; OnPropertyChanged(); }
        }

        private bool? isChecked = false;
        /// <summary>
        /// 选中标记
        /// </summary>
        public virtual bool? IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    Checked(value);
                }
            }
        }

        /// <summary>
        /// 父级
        /// </summary>
        public ITreeView Parent { get; set; }
        /// <summary>
        /// 子级列表
        /// </summary>
        public virtual ObservableCollection<ITreeView> Children { get; set; } = new ObservableCollection<ITreeView>();

        /// <summary>
        /// </summary>
        public TreeViewModel()
        {
            this.Id = this.GetHashCode();
        }
        /// <summary>
        /// </summary>
        public TreeViewModel(string groupName, bool isGrouping = false) : this()
        {
            this.GroupName = groupName;
            this.IsGrouping = isGrouping;
        }
        /// <summary>
        /// 添加子级
        /// </summary>
        public void Add(TreeViewModel model)
        {
            model.Parent = this;
            this.Children.Add(model);
        }
        /// <summary>
        /// 添加子级
        /// </summary>
        public void Add(string text, string subtitle = null, string desc = null)
        {
            var child = new TreeViewModel
            {
                Text = text,
                ShortName = text.ToSpell(),
                Subtitle = subtitle,
                Desc = desc,
                Parent = this
            };
            child.ShortName = text.ToSpell();
            if (child.ShortName.Length > 2) child.ShortName = child.ShortName.Substring(0, 2);
            this.Children.Add(child);
        }

        #region 关联选中项
        private void Checked(bool? value)
        {
            UpdateChild(value);
            UpdateParent();
        }
        /// <summary>
        /// 更新值
        /// </summary>
        /// <param name="value"></param>
        private void UpdateValue(bool? value)
        {
            this.isChecked = value;
            OnPropertyChanged(nameof(IsChecked));
        }
        /// <summary>
        /// 更新子级
        /// </summary>
        /// <param name="value"></param>
        private void UpdateChild(bool? value)
        {
            UpdateValue(value);
            foreach (var item in Children)
            {
                if (item is TreeViewModel model) model.UpdateChild(value);
            }
        }
        /// <summary>
        /// 更新父级
        /// </summary>
        private void UpdateParent()
        {
            if (this.Parent != null)
            {
                bool? value = null;
                if (!this.Parent.Children.Any(c => c.IsChecked != true)) value = true;
                if (!this.Parent.Children.Any(c => c.IsChecked != false)) value = false;
                if (this.Parent is TreeViewModel model)
                {
                    model.UpdateValue(value);
                    model.UpdateParent();
                }
            }
        }

        #endregion
    }
}
