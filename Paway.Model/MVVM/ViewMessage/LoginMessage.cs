using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Paway.Model
{
    public class LoginMessage
    {
        public string UserName { get; set; }
        public LoginMessage() { }
        public LoginMessage(string userName)
        {
            this.UserName = userName;
        }
    }
}
