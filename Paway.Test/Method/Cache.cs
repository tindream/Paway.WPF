using Paway.Helper;
using Paway.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paway.Test
{
    public partial class Cache : Comm.Cache
    {
        #region 系统
        /// <summary>
        /// 人员列表
        /// </summary>
        public static List<UserInfo> UserList { get; private set; } = new List<UserInfo>();

        #endregion
    }
}
