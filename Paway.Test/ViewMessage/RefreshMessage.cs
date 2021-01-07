using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.Test
{
    public class RefreshMessage
    {
        public DependencyObject Obj { get; set; }
    }
    public class UserRefreshMessage : RefreshMessage { }


    public class AuthApplyMessage { }
}
