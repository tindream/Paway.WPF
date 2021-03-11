using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paway.Test
{
    public class UserMessage
    {
        public OperType Type { get; set; }
        public UserInfo Info { get; set; }
        public UserMessage(UserInfo info, OperType type)
        {
            this.Info = info;
            this.Type = type;
        }
    }
}
