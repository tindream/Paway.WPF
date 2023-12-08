using Paway.Helper;
using Paway.Utils;
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

namespace Paway.Comm
{
    public partial class SQLiteBaseService : SQLiteHelper, IDataGridServer
    {
        public SQLiteBaseService(string dbName = "test.db", string createSql = null)
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbName);
            base.InitConnect(file);
            if (!createSql.IsEmpty() && base.InitCreate(createSql))
            {
                Created();
            }
        }
        /// <summary>
        /// 从资源文件创建数据库文件
        /// </summary>
        protected void Create(string sql)
        {
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
            var info = Cache.Find<T>(userId);
            if (info == null) throw new WarningException("用户不存在");
            return UserChecked(info);
        }

        #endregion
    }
}
