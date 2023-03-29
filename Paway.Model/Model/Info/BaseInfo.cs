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
    /// </summary>
    [Serializable]
    public class BaseInfo : ModelBase, IBaseInfo
    {
        [NoShow, NoExcel]
        public override int Id { get => base.Id; set => base.Id = value; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [NoShow, NoExcel]
        public virtual DateTime CreateOn { get; set; }
        /// <summary>
        ///更新时间
        /// </summary>
        [NoShow, NoExcel]
        public virtual DateTime UpdateOn { get; set; }

        public BaseInfo()
        {
            this.CreateOn = DateTime.Now;
        }
    }
    /// <summary>
    /// Info基类
    /// <para>无更新</para>
    /// </summary>
    [Serializable]
    public class BaseOnce : BaseInfo
    {
        [NoShow, NoExcel, NoSelect, JsonIgnore]
        public override DateTime UpdateOn { get; set; }
    }
    /// <summary>
    /// 父子结构
    /// </summary>
    [Serializable]
    public class ParentBase : BaseInfo, IParent
    {
        [NoShow, NoExcel]
        public int ParentId { get; set; }
    }
    /// <summary>
    /// 父子结构
    /// <para>无更新</para>
    /// </summary>
    [Serializable]
    public class ParentBaseOnce : ParentBase
    {
        [NoShow, NoExcel, NoSelect, JsonIgnore]
        public override DateTime UpdateOn { get { return CreateOn; } }
    }
}
