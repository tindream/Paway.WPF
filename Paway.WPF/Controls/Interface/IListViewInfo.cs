using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// ListView数接口据定义
    /// </summary>
    public interface IListViewInfo
    {
        /// <summary>
        /// 内容
        /// </summary>
        string Content { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        string Desc { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        ImageEXT Image { get; set; }
        /// <summary>
        /// 选择项
        /// </summary>
        bool IsSelected { get; set; }
    }
}
