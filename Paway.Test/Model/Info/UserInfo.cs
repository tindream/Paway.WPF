using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    [Serializable]
    [Table("Sys_Users")]
    [Description("用户")]
    public class UserInfo : BaseInfo, IUser, IChecked, ICompare<UserInfo>
    {
        private string _userName;
        /// <summary>
        /// 用户名
        /// </summary>
        [Text("用户名")]
        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }
        private string _display;
        /// <summary>
        /// 昵称
        /// </summary>
        [NoShow, Text("姓名")]
        public string Display
        {
            get => _display;
            set { _display = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 显示名
        /// </summary>
        [NoShow, NoExcel, NoSelect]
        public string CustomName
        {
            get
            {
                if (!Display.IsEmpty()) return Display;
                return UserName;
            }
        }
        [NoShow]
        [Text("密码")]
        public string Password { get; set; }

        private SexType _sex;
        [Text("性别")]
        public SexType Sex
        {
            get => _sex;
            set { _sex = value; OnPropertyChanged(); }
        }

        private bool _iStatu = true;
        /// <summary>
        /// 启用状态
        /// </summary>
        [NoShow, NoExcel]
        public bool IStatu
        {
            get => _iStatu;
            set { _iStatu = value; OnPropertyChanged(); OnPropertyChanged(nameof(Status)); }
        }
        [Text("状态")]
        [NoSelect]
        public string Status
        {
            get => IStatu ? "启用" : "停用";
            set { IStatu = value == "启用"; }
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
            if (Cache.UserList.Any(c => c.Id != this.Id && c.UserName == this.UserName)) throw new WarningException($"[{this.GetType().Description()}]{this.UserName} 已存在");
        }
        public bool Compare(UserInfo item)
        {
            return this.UserName == item.UserName;
        }

        public UserInfo() { }
        public UserInfo(string name)
        {
            this.UserName = name;
        }
        public override string ToString()
        {
            return CustomName;
        }
    }
}
