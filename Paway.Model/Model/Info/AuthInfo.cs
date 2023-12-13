using Newtonsoft.Json;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Model
{
    /// <summary>
    /// 权限定义
    /// </summary>
    [Serializable]
    [Table("Organ_Auths")]
    [Description("权限")]
    public class AuthInfo : ParentBase
    {
        /// <summary>
        /// 菜单权限
        /// </summary>
        [Text("菜单权限")]
        public MenuAuthType MenuType { get; set; }

        /// <summary>
        /// 操作权限
        /// </summary>
        [Text("操作权限")]
        public ButtonAuthType ButtonType { get; set; }

        /// <summary>
        /// 操作权限-插入
        /// </summary>
        [NoShow, NoExcel, NoSelect, JsonIgnore]
        public bool Insert { get { return (ButtonType & ButtonAuthType.Insert) == ButtonAuthType.Insert; } }
        /// <summary>
        /// 操作权限-更新
        /// </summary>
        [NoShow, NoExcel, NoSelect, JsonIgnore]
        public bool Update { get { return (ButtonType & ButtonAuthType.Update) == ButtonAuthType.Update; } }
        /// <summary>
        /// 操作权限-删除
        /// </summary>
        [NoShow, NoExcel, NoSelect, JsonIgnore]
        public bool Delete { get { return (ButtonType & ButtonAuthType.Delete) == ButtonAuthType.Delete; } }

        /// <summary>
        /// 权限定义
        /// </summary>
        public AuthInfo() { }
        /// <summary>
        /// 权限定义
        /// </summary>
        public AuthInfo(int parentId)
        {
            this.ParentId = parentId;
        }
    }

    /// <summary>
    /// 权限定义设置
    /// </summary>
    [Serializable]
    [Description("权限")]
    public class AuthReportInfo : ParentBase
    {
        private bool iMenuType;
        /// <summary>
        /// 菜单权限
        /// </summary>
        [NoShow]
        public bool IMenuType
        {
            get { return iMenuType; }
            set { iMenuType = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 菜单权限
        /// </summary>
        [Text("菜单权限")]
        public string MenuType { get; set; }

        private bool iButtonType;
        /// <summary>
        /// 操作权限
        /// </summary>
        [NoShow]
        public bool IButtonType
        {
            get { return iButtonType; }
            set { iButtonType = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 操作权限
        /// </summary>
        [Text("操作权限")]
        public string ButtonType { get; set; }

        /// <summary>
        /// 权限定义
        /// </summary>
        public AuthReportInfo()
        {
            this.Id = this.GetHashCode();
        }
        /// <summary>
        /// 权限定义
        /// </summary>
        public AuthReportInfo(int parentId)
        {
            this.ParentId = parentId;
        }
    }
}
