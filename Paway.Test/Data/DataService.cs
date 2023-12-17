using Paway.Comm;
using Paway.Helper;
using Paway.Model;
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
    public partial class DataService
    {
        private static DataService _default;
        public static DataService Default
        {
            get
            {
                if (_default == null) _default = new DataService();
                return _default;
            }
        }
        private ISQLiteServer _server;
        public ISQLiteServer Server
        {
            get
            {
                if (_server == null)
                {
                    _server = DataServiceBuilder.CreateSQLite();
                    var sql = new Uri(@"pack://application:,,,/Paway.Test;component/Resources/script.sql").ToText();
                    _server.Create(sql, () =>
                    {
                        UserInfo info = new UserInfo()
                        {
                            Name = "admin0"
                        };
                        _server.Insert(info);
                        info.Id = 19;
                        info.Name = "admin001";
                        _server.Insert(info);
                    });
                }
                return _server;
            }
        }
        public DataService() : base() { }

        #region 自动升级
        public void Load()
        {
            Config.Admin = Method.Conversion<AdminInfo, AdminBaseInfo>(Server.Find<AdminBaseInfo>());
            AutoUpdate();
        }
        private void AutoUpdate()
        {
            if (Config.Admin.Version >= 0) return;
            Server.ExecuteTransaction(cmd =>
            {
                if (Config.Admin.Version == 0)
                {
                    //base.Execute($"alter table [MeetScreens] add column [{nameof(MeetScreenInfo.BackImage)}] nvarchar(256);", cmd);
                    Config.Admin.Version = 1;
                    Server.Update(nameof(Config.Admin.Version), cmd);
                }
            });
        }

        #endregion
    }
}
