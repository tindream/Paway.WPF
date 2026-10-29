using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class TempInfo : ParentBaseOperateInfo
    {
        public DateTime DateTime { get; set; }
        private double _value;
        /// <summary>
        /// 值
        /// </summary>
        public double Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
        }

        private string _values;
        /// <summary>
        /// 值
        /// </summary>
        public string Values
        {
            get { return _values; }
            set { _values = value; OnPropertyChanged(); }
        }

        public TempInfo() { }
        public TempInfo(DateTime time, double value)
        {
            this.DateTime = time;
            Value = value;
        }
    }
}
