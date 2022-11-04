using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    [Serializable]
    [Table("Users")]
    [Description("用户")]
    public class UserInfo : BaseInfo, IChecked, ICompare<UserInfo>
    {
        private string _name;
        /// <summary>
        /// 用户名
        /// </summary>
        [Text("用户名")]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }
        private string _display;
        /// <summary>
        /// 昵称
        /// </summary>
        [Text("姓名")]
        public string Display
        {
            get => _display;
            set { _display = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 显示名
        /// </summary>
        [NoShow, NoExcel, NoSelect]
        public string Desc
        {
            get
            {
                if (!Display.IsEmpty()) return Display;
                return Name;
            }
        }
        [NoShow]
        [Text("密码")]
        public string Pad { get; set; }

        private SexType _sex;
        [Text("性别")]
        public SexType Sex
        {
            get => _sex;
            set { _sex = value; OnPropertyChanged(); }
        }

        private bool _statu = true;
        /// <summary>
        /// 启用状态
        /// </summary>
        [NoShow, NoExcel]
        public bool Statu
        {
            get => _statu;
            set { _statu = value; OnPropertyChanged(); OnPropertyChanged(nameof(Status)); }
        }
        [Text("状态")]
        [NoSelect]
        public string Status
        {
            get => Statu ? "启用" : "停用";
            set { Statu = value == "启用"; }
        }

        private UserType _userType;
        [Text("类别")]
        public UserType UserType
        {
            get => _userType;
            set { _userType = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 登陆时间
        /// </summary>
        [NoExcel]
        [Text("登陆时间")]
        public DateTime LoginOn { get; set; }

        public void Checked()
        {
            if (Cache.UserList.Any(c => c.Id != this.Id && c.Name == this.Name)) throw new WarningException($"[{this.GetType().Description()}]{this.Name} 已存在");
            if (!this.Display.IsEmpty() && Cache.UserList.Any(c => c.Id != this.Id && c.Display == this.Display)) throw new WarningException($"[{this.GetType().Description()}]{this.Display} 已存在");
        }
        public bool Compare(UserInfo item)
        {
            if (this.Name == item.Name) return true;
            if (!this.Display.IsEmpty() && !item.Display.IsEmpty()) return this.Display == item.Display;
            return false;
        }

        public UserInfo() { }
        public UserInfo(string name)
        {
            this.Name = name;
        }
        public override string ToString()
        {
            return Desc;
        }
    }
}
