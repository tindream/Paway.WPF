using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Model
{
    /// <summary>
    /// 图片转换进度事件参数
    /// </summary>
    public class ProgressEventArgs
    {
        /// <summary>
        /// 当前序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 图片转换进度事件参数
        /// </summary>
        public ProgressEventArgs() { }
        /// <summary>
        /// 图片转换进度事件参数
        /// </summary>
        public ProgressEventArgs(int index, int total)
        {
            this.Index = index;
            this.Total = total;
        }
    }
}
