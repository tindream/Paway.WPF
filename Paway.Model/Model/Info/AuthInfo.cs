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
    /// 权限
    /// </summary>
    [Serializable]
    [Table("Organ_Auths")]
    [Description("权限")]
    public class AuthInfo : ParentBase
    {
        [Text("菜单权限")]
        public MenuType MenuType { get; set; }

        [Text("操作权限")]
        public ButtonType ButtonType { get; set; }

        [NoSelect]
        public bool Insert { get { return (ButtonType & ButtonType.Insert) == ButtonType.Insert; } }
        [NoSelect]
        public bool Update { get { return (ButtonType & ButtonType.Update) == ButtonType.Update; } }
        [NoSelect]
        public bool Delete { get { return (ButtonType & ButtonType.Delete) == ButtonType.Delete; } }

        public AuthInfo() { }
        public AuthInfo(int parentId)
        {
            this.ParentId = parentId;
        }
    }

    [Serializable]
    [Description("权限")]
    public class AuthReportInfo : ParentBase
    {
        private bool iMenuType;
        [NoShow]
        public bool IMenuType
        {
            get { return iMenuType; }
            set { iMenuType = value; OnPropertyChanged(); }
        }
        [Text("菜单权限")]
        public string MenuType { get; set; }

        private bool iButtonType;
        [NoShow]
        public bool IButtonType
        {
            get { return iButtonType; }
            set { iButtonType = value; OnPropertyChanged(); }
        }
        [Text("操作权限")]
        public string ButtonType { get; set; }

        public AuthReportInfo()
        {
            this.Id = this.GetHashCode();
        }
        public AuthReportInfo(int parentId)
        {
            this.ParentId = parentId;
        }
    }
}
