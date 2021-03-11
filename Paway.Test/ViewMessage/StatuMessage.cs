using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paway.Test
{
    public class StatuMessage
    {
        public string Msg { get; set; }
        public LeveType Level { get; set; }

        public StatuMessage() { }
        public StatuMessage(string msg)
        {
            this.Msg = msg;
            this.Level = LeveType.Debug;
        }
        public StatuMessage(string msg, LeveType level) : this(msg)
        {
            this.Msg = msg;
            this.Level = level;
        }
    }
    public class ErrorStatuMessage : StatuMessage
    {
        public ErrorStatuMessage(string title, Exception ex) : base($"{title}: {ex.Message()}", LeveType.Error) { }
    }
}
