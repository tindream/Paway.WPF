using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Paway.WPF
{
    /// <summary>
    /// 通知消息数据
    /// </summary>
    public class AdornerNoticeInfo
    {
        /// <summary>
        /// 记录创建时的顺序计数，作为唯一标识
        /// <para>批量弹出时，时间戳可能会重复</para>
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 控件Code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 记录实际高度
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 当前消息控件
        /// </summary>
        public Border Border { get; set; }
        /// <summary>
        /// 标记
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// </summary>
        public AdornerNoticeInfo(int adornerIndex, int code, Border border, object tag)
        {
            this.Index = adornerIndex;
            this.Code = code;
            this.Border = border;
            this.Height = border.ActualHeight + 2;
            this.Tag = tag;
        }
    }
}
