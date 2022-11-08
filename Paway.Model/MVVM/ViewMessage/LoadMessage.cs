using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.Model
{
    public class LoadMessage
    {
        public DependencyObject Obj { get; set; }
    }
    public class LoginLoadMessage : LoadMessage { }
}
