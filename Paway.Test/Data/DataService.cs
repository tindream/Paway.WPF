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
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace Paway.Test
{
    public partial class DataService : SQLiteHelper
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

        #region Login
        public UserInfo Login(string name, string pad)
        {
            var list = Find<UserInfo>(c => c.Name == name && c.Pad == pad);
            if (list.Count == 0)
            {
                var error = "用户名或密码错误";
                UserLog(new UserInfo(name), LogType.LoginError, error);
                throw new WarningException(error);
            }
            UserChecked(list[0]);
            list[0].LoginOn = DateTime.Now;
            base.Update(list[0], nameof(UserInfo.LoginOn));
            UserLog(list[0], LogType.Login);
            return list[0];
        }
        private UserInfo UserChecked(UserInfo user)
        {
            if (!user.Statu)
            {
                var error = "用户已停用";
                UserLog(user, LogType.LoginError, error);
                throw new WarningException(error);
            }
            return user;
        }
        public LogInfo UserLog(UserInfo user, LogType type, string msg = null)
        {
            var log = new LogInfo(type, user.Name, msg);
            base.Insert(log);
            return log;
        }
        public UserInfo Login(int userId)
        {
            var info = Cache.UserList.Find(c => c.Id == userId);
            if (info == null) throw new WarningException("用户不存在");
            return UserChecked(info);
        }

        #endregion

        #region Admin.Update
        public void Update(string name, DbCommand arg = null)
        {
            var value = Config.Admin.GetValue(name);
            base.ExecuteCommand(cmd =>
            {
                var list = Find<AdminBaseInfo>(c => c.Name == name, cmd);
                if (list.Count == 0)
                {
                    AdminBaseInfo info = new AdminBaseInfo() { Name = name, Value = value.ToStrings() };
                    Insert(info, cmd);
                }
                else
                {
                    list[0].Value = value.ToStrings();
                    list[0].UpdateOn = DateTime.Now;
                    Update(list[0], cmd);
                }
            }, arg);
        }

        #endregion

        #region 自动升级
        private void AutoUpdate()
        {
            if (Config.Admin.Version >= 0) return;
            base.ExecuteCommand(cmd =>
            {
                if (Config.Admin.Version == 0)
                {
                    //base.Execute($"alter table [MeetScreens] add column [{nameof(MeetScreenInfo.BackImage)}] nvarchar(256);", cmd);
                    Config.Admin.Version = 1;
                    Update(nameof(Config.Admin.Version), cmd);
                }
            });
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
            Config.Admin = Method.Conversion<AdminInfo, AdminBaseInfo>(Find<AdminBaseInfo>());
            AutoUpdate();
        }
        public List<T> FindSort<T>(DbCommand cmd = null) where T : class, new()
        {
            return FindSort<T>(null, cmd);
        }
        public List<T> FindSort<T>(Expression<Func<T, bool>> action, DbCommand cmd = null) where T : class, new()
        {
            var list = Find(action, cmd);
            Method.Sorted(list);
            return list;
        }

        #endregion
    }
}
