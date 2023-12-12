using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// 外部数据标记
    /// </summary>
    public interface ITag
    {
        /// <summary>
        /// 外部数据
        /// </summary>
        object Tag { get; set; }
    }
}
