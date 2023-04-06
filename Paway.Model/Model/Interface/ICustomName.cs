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
    /// 自定义名称
    /// </summary>
    public interface ICustomName : IId
    {
        /// <summary>
        /// 自定义名称
        /// </summary>
        string CustomName { get; }
    }
}
