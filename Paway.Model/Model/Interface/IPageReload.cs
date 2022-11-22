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
    /// 页重加载接口
    /// </summary>
    public interface IPageReload
    {
        /// <summary>
        /// 加载状态
        /// </summary>
        bool ILoad { get; set; }
        /// <summary>
        /// 在Loaded第一次触发或重加载时调用
        /// </summary>
        void PageReload();
    }
}
