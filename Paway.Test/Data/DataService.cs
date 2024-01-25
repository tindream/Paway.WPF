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
        private ISQLiteService _service;
        public ISQLiteService Service
        {
            get
            {
                if (_service == null)
                {
                    _service = DataServiceBuilder.CreateSQLite();
                    var sql = new Uri(@"pack://application:,,,/Paway.Test;component/Resources/script.sql").ToText();
                    _service.Create(sql, () =>
                    {
                        UserInfo info = new UserInfo()
                        {
                            Name = "admin0"
                        };
                        _service.Insert(info);
                        info.Id = 19;
                        info.Name = "admin001";
                        _service.Insert(info);
                    });
                }
                return _service;
            }
        }
        public DataService() : base() { }

        #region 自动升级
        public void Load()
        {
            Config.Admin = Method.Conversion<AdminInfo, AdminBaseInfo>(Service.Find<AdminBaseInfo>());
            AutoUpdate();
        }
        private void AutoUpdate()
        {
            if (Config.Admin.Version >= 0) return;
            Service.ExecuteTransaction(cmd =>
            {
                if (Config.Admin.Version == 0)
                {
                    //base.Execute($"alter table [MeetScreens] add column [{nameof(MeetScreenInfo.BackImage)}] nvarchar(256);", cmd);
                    Config.Admin.Version = 1;
                    Service.Update(nameof(Config.Admin.Version), cmd);
                }
            });
        }

        #endregion
    }
}
