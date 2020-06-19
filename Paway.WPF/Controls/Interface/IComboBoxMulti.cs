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
    /// ComboBoxMulti数接口据定义
    /// </summary>
    public interface IComboBoxMulti
    {
        /// <summary>
        /// 关联主键
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// 显示文本
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        bool IsChecked { get; set; }
    }
}
