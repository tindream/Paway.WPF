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
    /// 数据更新时间
    /// </summary>
    public interface IBaseInfo
    {
        /// <summary>
        ///创建时间
        /// </summary>
        DateTime CreateOn { get; set; }
        /// <summary>
        ///更新时间
        /// </summary>
        DateTime UpdateOn { get; set; }
    }
}
