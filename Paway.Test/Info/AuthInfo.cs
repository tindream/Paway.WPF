using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    /// <summary>
    /// 权限
    /// </summary>
    [Serializable]
    [Table("Auths")]
    [Description("权限")]
    public class AuthInfo : ModelBase
    {
        [NoShow]
        public int UserId { get; set; }

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
        public AuthInfo(int userId)
        {
            this.UserId = userId;
        }
    }
    [Serializable]
    [Description("权限")]
    public class AuthReportInfo : ModelBase
    {
        [NoShow]
        public int UserId { get; set; }

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
        public AuthReportInfo(int userId)
        {
            this.UserId = userId;
        }
    }
}
