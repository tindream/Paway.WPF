using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Model
{
    /// <summary>
    /// 管理数据结构
    /// </summary>
    [Serializable]
    [Table("Sys_Admins")]
    public class AdminBaseInfo : BaseInfo, IInfo
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public AdminBaseInfo()
        {
            this.CreateOn = DateTime.Now;
        }
    }
}
