using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.Test
{
    public class LoadMessage
    {
        public DependencyObject Obj { get; set; }
    }
    public class TipLoadMessage : LoadMessage { }
    public class LoginLoadMessage : LoadMessage { }
}
