using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media;
using Paway.Helper;
using Paway.WPF;

namespace Paway.Test
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum SexType
    {
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Man,
        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        Woman,
    }
}
