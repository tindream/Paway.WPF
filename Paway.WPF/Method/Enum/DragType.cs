
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 拖拽状态
    /// </summary>
    public enum DragType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 拖拽开始
        /// </summary>
        Enter,
        /// <summary>
        /// 拖动过程
        /// </summary>
        Over,
        /// <summary>
        /// 拖动离开
        /// </summary>
        Leave,
        /// <summary>
        /// 拖动完成
        /// </summary>
        Completed,
    }
}
