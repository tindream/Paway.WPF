using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Paway.Test
{
    /// <summary>
    /// 图表线
    /// </summary>
    public enum MonitorType
    {
        None,
        /// <summary>
        /// 左束角
        /// </summary>
        [Tag(new byte[] { 250, 232, 2 })]///FAE802
        [Description("左束角")]
        LeftToeAngle = 1 << 0,
        /// <summary>
        /// 右束角
        /// </summary>
        [Tag(new byte[] { 255, 70, 51 })]//FF4633
        [Description("右束角")]
        RightToeAngle = 1 << 1,
    }
}
