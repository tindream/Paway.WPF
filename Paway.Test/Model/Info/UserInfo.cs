using Newtonsoft.Json;
using Paway.Comm;
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
        public string Name
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
                return Name;
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

        private EnableType _enable = EnableType.Enable;
        /// <summary>
        /// 启用状态
        /// </summary>
        [NoShow, NoExcel]
        public EnableType Enable
        {
            get => _enable;
            set { _enable = value; OnPropertyChanged(); OnPropertyChanged(nameof(Status)); }
        }
        [Text("状态")]
        [NoSelect]
        public string Status
        {
            get => Enable.Description();
            set { Enable = value.Parse<EnableType>(); }
        }

        private UserType _userType;
        [Text("类别")]
        public UserType UserType
        {
            get => _userType;
            set { _userType = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 登录时间
        /// </summary>
        [NoExcel]
        [Text("登录时间")]
        public DateTime LoginOn { get; set; }

        #region 接口
        [NoShow, NoSelect, JsonIgnore]
        public object Tag { get { return UserType; } set { } }
        private string _clientId;
        [NoShow, NoSelect, JsonIgnore]
        public string VerCode
        {
            get
            {
                if (_clientId == null) _clientId = EncryptHelper.MD5($"{Id}_{Config.Suffix}");
                return _clientId;
            }
        }

        #endregion

        public void Checked()
        {
            if (Cache.UserList.Any(c => c.Id != this.Id && c.Name == this.Name)) throw new WarningException($"[{this.GetType().Description()}]{this.Name} 已存在");
        }
        public bool Compare(UserInfo item)
        {
            return this.Name == item.Name;
        }

        public UserInfo() { }
        public UserInfo(string name)
        {
            this.Name = name;
        }
        public override string ToString()
        {
            return CustomName;
        }
    }
}
