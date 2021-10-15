using Paway.Helper;
using Paway.Test.Properties;
using Paway.Utils;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Paway.Test
{
    public class DataService : SQLiteHelper
    {
        private const string dbName = "test.db";
        private static DataService intance;
        public static DataService Default
        {
            get
            {
                if (intance == null) intance = new DataService();
                return intance;
            }
        }
        public DataService()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string file = Path.Combine(path, dbName);
            base.InitConnect(file);
            if (base.InitCreate(Resources.script))
            {
                var user = new UserInfo { UserType = UserType.Admin, Name = "admin", Pad = EncryptHelper.MD5("admin" + Config.Suffix) };
                this.Insert(user);
                var auth = new AuthInfo(user.Id);
                auth.SetValue(nameof(Test.MenuType), Method.Sum<MenuType>());
                auth.SetValue(nameof(Test.ButtonType), Method.Sum<ButtonType>());
                this.Insert(auth);
            }
        }

        public UserInfo Login(string name, string pad)
        {
            var find = "Name = @name  and Pad = @pad";
            List<UserInfo> list = Find<UserInfo>(find, new { name, pad });
            if (list.Count == 0)
            {
                throw new WarningException("用户名或密码错误");
            }
            list[0].LoginOn = DateTime.Now;
            Update(list[0], nameof(UserInfo.LoginOn));
            return list[0];
        }

        #region Admin.Update
        public void Update(string name, DbCommand arg = null)
        {
            var value = Config.Admin.GetValue(name);
            base.ExecuteCommand(cmd =>
            {
                string find = string.Format("Name = '{0}'", name);
                List<AdminBaseInfo> list = Find<AdminBaseInfo>(find, cmd);
                if (list.Count == 0)
                {
                    AdminBaseInfo info = new AdminBaseInfo() { Name = name, Value = value.ToStrings(), DateTime = DateTime.Now };
                    Insert(info, cmd);
                }
                else
                {
                    list[0].Value = value.ToString();
                    list[0].DateTime = DateTime.Now;
                    Update(list[0], cmd);
                }
            }, arg);
        }

        #endregion

        #region 自动升级
        private void AutoUpdate()
        {
            #region 1.0
            if (Config.Admin.Version == 0)
            {
                base.ExecuteCommand(cmd =>
                {
                    //base.Execute("alter table [AlarmRecords] add column [DeviceId] int;", cmd);
                    //Config.Admin.Version = 32;
                    //Update(nameof(Config.Admin.Version), cmd);
                });
            }

            #endregion
        }

        #endregion

        #region 初始化
        public void Init()
        {
            new Action(() =>
            {
                ExecuteCommand(cmd => { });
            }).BeginInvoke(null, null);
        }
        public void Load()
        {
            base.ExecuteCommand(cmd =>
            {
                Config.Admin = FindAdmin(cmd);
                AutoUpdate();
            });
        }
        private AdminInfo FindAdmin(DbCommand cmd = null)
        {
            List<AdminBaseInfo> temp = Find<AdminBaseInfo>(cmd);
            List<IInfo> list = new List<IInfo>();
            list.AddRange(temp);
            return Method.Conversion<AdminInfo, IInfo>(list);
        }

        #endregion
    }
}
