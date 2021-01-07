using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public enum UserType
    {
        [Description(TConfig.None)]
        None = 0,
        User = 10,
        Admin = 100,
    }
}
