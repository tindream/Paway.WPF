using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// 系统字体
    /// </summary>
    public class FontInfo : BaseOperateInfo
    {
        /// <summary>
        /// 字体名称
        /// </summary>
        [FillSize]
        public string Name { get; set; }
        /// <summary>
        /// 字体名称
        /// </summary>
        [NoShow]
        public FontFamily FontFamily { get; set; }
    }
}
