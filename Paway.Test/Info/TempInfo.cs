using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class TempInfo
    {
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public double Value { get; private set; }

        public TempInfo() { }
        public TempInfo(DateTime time, double value)
        {
            this.DateTime = time;
            Value = value;
        }
    }
}
