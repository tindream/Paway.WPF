using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// 启用状态
    /// </summary>
    public interface IEnable
    {
        /// <summary>
        /// 启用状态
        /// </summary>
        EnableType Enable { get; set; }
    }
}
