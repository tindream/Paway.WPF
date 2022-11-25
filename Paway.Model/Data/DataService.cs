using Paway.Helper;
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
using System.Windows;

namespace Paway.Model
{
    public partial class DataService : SQLiteHelper, IDataGridServer
    {
        public DataService(string dbName = "test.db", string createSql = null)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string file = Path.Combine(path, dbName);
            base.InitConnect(file);
            if (!createSql.IsEmpty() && base.InitCreate(createSql))
            {
                Created();
            }
        }
        /// <summary>
        /// 从资源文件创建数据库文件
        /// </summary>
        protected void Create(Uri uri)
        {
            var sql = Method.ResourceText(uri);
            if (base.InitCreate(sql))
            {
                Created();
            }
        }
        protected virtual void Created() { }

        #region Login
        public T Login<T>(string userName, string password) where T : class, IUser, new()
        {
            var list = Find<T>(c => c.UserName == userName && c.Password == password);
            if (list.Count == 0) throw new WarningException("用户名或密码错误");
            UserChecked(list[0]);
            list[0].LoginOn = DateTime.Now;
            base.Update(list[0], nameof(IUser.LoginOn));
            return list[0];
        }
        private T UserChecked<T>(T user) where T : IUser
        {
            if (!user.IStatu) throw new WarningException("用户已停用");
            return user;
        }
        public T Login<T>(int userId) where T : IUser
        {
            var info = Cache.List<T>().Find(c => c.Id == userId);
            if (info == null) throw new WarningException("用户不存在");
            return UserChecked(info);
        }

        #endregion

        #region Admin.Update
        public void UpdateAdmin<T>(T admin, string name, DbCommand arg = null) where T : class
        {
            var value = admin.GetValue(name);
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

        #region 初始化
        public void Init()
        {
            new Action(() =>
            {
                ExecuteCommand(cmd => { });
            }).BeginInvoke(null, null);
        }
        public T LoadAdmin<T>() where T : class
        {
            return Method.Conversion<T, AdminBaseInfo>(Find<AdminBaseInfo>());
        }

        #endregion
    }
}
