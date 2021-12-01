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
    public interface ITreeViewItem : IId
    {
        /// <summary>
        /// 数据
        /// </summary>
        object Tag { get; set; }
        /// <summary>
        /// 组标记
        /// </summary>
        bool IsGroup { get; set; }
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
        ITreeViewItem Parent { get; set; }
        /// <summary>
        /// 子级列表
        /// </summary>
        ObservableCollection<ITreeViewItem> Children { get; set; }

        /// <summary>
        /// 添加子级
        /// </summary>
        void Add(ITreeViewItem model);
        /// <summary>
        /// 添加子级
        /// </summary>
        void Add(string text, string subtitle = null, string desc = null);
        /// <summary>
        /// 更新节点选中
        /// </summary>
        void Checked(bool? value);
        /// <summary>
        /// 更新子级节点选中
        /// </summary>
        void CheckedChild(bool? value);
        /// <summary>
        /// 更新父级节点选中
        /// </summary>
        void CheckedParent();
    }
    /// <summary>
    /// ITreeView数据模型
    /// </summary>
    public class TreeViewItemModel : ModelBase, ITreeViewItem
    {
        /// <summary>
        /// 数据
        /// </summary>
        [NoShow]
        public object Tag { get; set; }

        private bool isGrouping;
        /// <summary>
        /// 组标记
        /// </summary>
        [NoShow]
        public virtual bool IsGroup
        {
            get { return isGrouping; }
            set { isGrouping = value; OnPropertyChanged(); }
        }
        private string shortName;
        /// <summary>
        /// 头像
        /// </summary>
        [NoShow]
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
        [NoShow]
        public virtual bool? IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    CheckedChild(value);
                    CheckedParent();
                }
            }
        }

        /// <summary>
        /// 父级
        /// </summary>
        public ITreeViewItem Parent { get; set; }
        /// <summary>
        /// 子级列表
        /// </summary>
        public virtual ObservableCollection<ITreeViewItem> Children { get; set; } = new ObservableCollection<ITreeViewItem>();

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return Text;
        }
        /// <summary>
        /// </summary>
        public TreeViewItemModel()
        {
            this.Id = this.GetHashCode();
        }
        /// <summary>
        /// </summary>
        public TreeViewItemModel(string text, bool isGroup = false) : this()
        {
            this.Text = text;
            this.IsGroup = isGroup;
        }
        /// <summary>
        /// 添加子级
        /// </summary>
        public void Add(ITreeViewItem model)
        {
            model.Parent = this;
            this.Children.Add(model);
        }
        /// <summary>
        /// 添加子级
        /// </summary>
        public void Add(string text, string subtitle = null, string desc = null)
        {
            var child = new TreeViewItemModel
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
        /// <summary>
        /// 更新节点选中
        /// </summary>
        public virtual void Checked(bool? value)
        {
            this.isChecked = value;
            OnPropertyChanged(nameof(IsChecked));
        }
        /// <summary>
        /// 更新子级节点选中
        /// </summary>
        public virtual void CheckedChild(bool? value)
        {
            Checked(value);
            foreach (var item in Children)
            {
                item.CheckedChild(value);
            }
        }
        /// <summary>
        /// 更新父级节点选中
        /// </summary>
        public virtual void CheckedParent()
        {
            if (this.Parent != null)
            {
                bool? value = null;
                if (!this.Parent.Children.Any(c => c.IsChecked != true)) value = true;
                if (!this.Parent.Children.Any(c => c.IsChecked != false)) value = false;
                this.Parent.Checked(value);
                this.Parent.CheckedParent();
            }
        }

        #endregion
    }
}
