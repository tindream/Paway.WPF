using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// 排序接口
    /// </summary>
    public interface IIndex
    {
        /// <summary>
        /// 序号
        /// </summary>
        int Index { get; set; }
    }
}
