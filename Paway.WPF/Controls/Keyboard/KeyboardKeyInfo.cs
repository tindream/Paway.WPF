using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Keys = System.Windows.Forms.Keys;

namespace Paway.WPF
{
    internal class KeyboardKeyInfo
    {
        /// <summary>
        /// 全键盘-小写
        /// </summary>
        public string AllLower { get; set; }
        /// <summary>
        /// 全键盘-大写
        /// </summary>
        public string AllUp { get; set; }
        /// <summary>
        /// 全键盘-值
        /// </summary>
        public int AllValue { get; set; }

        /// <summary>
        /// 数字键盘-英文模式
        /// </summary>
        public string NumEn { get; set; }
        /// <summary>
        /// 数字键盘-英文模式-值
        /// </summary>
        public int NumEnV { get; set; }
        /// <summary>
        /// 数字键盘-英文模式-需要Shift标记
        /// </summary>
        public bool INumEnShift { get; set; }

        /// <summary>
        /// 数字键盘-中文模式
        /// </summary>
        public string NumCn { get; set; }
        /// <summary>
        /// 数字键盘-中文模式-值
        /// </summary>
        public int NumCnV { get; set; }
        /// <summary>
        /// 数字键盘-中文模式-需要Shift标记
        /// </summary>
        public bool INumCnShift { get; set; }
        /// <summary>
        /// 数字键盘-中文模式-启用Unicode模式标记
        /// </summary>
        public bool IUnicode { get; set; }

        public KeyboardKeyInfo() { }
        public KeyboardKeyInfo(string allLower, string allUp, int allValue, string numEn, int numEnV, bool iNumEnShift, string numCn, int numCnV, bool iNumCnShift, bool iUnicode = false)
        {
            this.AllLower = allLower;
            this.AllUp = allUp;
            this.AllValue = allValue;

            this.NumEn = numEn;
            this.NumEnV = numEnV;
            this.INumEnShift = iNumEnShift;

            this.NumCn = numCn;
            this.NumCnV = numCnV;
            this.INumCnShift = iNumCnShift;
            this.IUnicode = iUnicode;
        }
    }
}
