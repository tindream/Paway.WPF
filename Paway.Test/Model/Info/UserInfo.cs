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
    public class UserInfo : UserBaseInfo, IChecked, ICompare<UserInfo>
    {
        private SexType _sex;
        [Text("性别")]
        public SexType Sex
        {
            get => _sex;
            set { _sex = value; OnPropertyChanged(); }
        }

        private UserType _userType;
        [Text("类别")]
        public UserType UserType
        {
            get => _userType;
            set { _userType = value; OnPropertyChanged(); }
        }

        #region 接口
        [NoShow, NoSelect, JsonIgnore]
        public override object Tag { get { return UserType; } set { } }
        private string _token;
        [NoShow, NoSelect, JsonIgnore]
        public override string Token
        {
            get
            {
                if (_token == null) _token = EncryptHelper.MD5($"{Id}_{Config.Suffix}");
                return _token;
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
    }
}
