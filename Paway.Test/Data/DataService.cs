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
    public partial class DataService : Paway.Model.DataService
    {
        private static DataService intance;
        public static DataService Default
        {
            get
            {
                if (intance == null) intance = new DataService();
                return intance;
            }
        }
        public DataService() : base()
        {
            base.Create(@"pack://application:,,,/Paway.Test;component/Resources/script.sql");
        }
        protected override void Created()
        {
            var user = new UserInfo { UserType = UserType.Admin, UserName = "admin", Password = EncryptHelper.MD5("admin" + Config.Suffix) };
            this.Insert(user);
            var auth = new AuthInfo(user.Id);
            auth.SetValue(nameof(auth.MenuType), Method.Sum<MenuAuthType>());
            auth.SetValue(nameof(auth.ButtonType), Method.Sum<MenuAuthType>());
            this.Insert(auth);
        }

        #region 自动升级
        public void Load()
        {
            Config.Admin = LoadAdmin<AdminInfo>();
            AutoUpdate();
        }
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
    }
}
