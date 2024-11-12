using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// System.Windows.Input.Key解析结构
    /// </summary>
    public class KeyDecodeInfo
    {
        /// <summary>
        /// System.Windows.Input.Key
        /// </summary>
        public Key Key;
        /// <summary>
        /// 输出
        /// </summary>
        public bool Printable;
        /// <summary>
        /// 字符
        /// </summary>
        public char Character;
        /// <summary>
        /// </summary>
        public bool Shift;
        /// <summary>
        /// </summary>
        public bool Ctrl;
        /// <summary>
        /// </summary>
        public bool Alt;
        /// <summary>
        /// sideband
        /// </summary>
        public int Type;
        /// <summary>
        /// sideband
        /// </summary>
        public string Sideband;
    };
}
