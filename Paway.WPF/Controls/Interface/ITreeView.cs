using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// 选择
        /// </summary>
        bool? IsChecked { get; set; }
        /// <summary>
        /// 组
        /// </summary>
        bool IsGrouping { get; set; }
        /// <summary>
        /// 子级
        /// </summary>
        ObservableCollection<ITreeView> Children { get; set; }
        /// <summary>
        /// 组名
        /// </summary>
        string GroupName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        string SurName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        string Subtitle { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        string Desc { get; set; }
    }
}
