using Newtonsoft.Json;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Paway.Model
{
    /// <summary>
    /// Info基类
    /// <para>无更新</para>
    /// </summary>
    [Serializable]
    public class BaseOperateOnce : BaseOperateInfo
    {
        /// <summary>
        /// 不保存更新时间
        /// </summary>
        [NoShow, NoExcel, NoSelect, JsonIgnore]
        public override DateTime UpdateOn { get; set; }
    }
    /// <summary>
    /// 父子结构
    /// <para>无更新</para>
    /// </summary>
    [Serializable]
    public class ParentBaseOnce : ParentBaseOperateInfo
    {
        /// <summary>
        /// 不保存更新时间
        /// </summary>
        [NoShow, NoExcel, NoSelect, JsonIgnore]
        public override DateTime UpdateOn { get { return CreateOn; } }
    }
}
