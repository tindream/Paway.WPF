using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.Comm
{
    public class MQCountMessage
    {
        public int Count { get; set; }

        public MQCountMessage() { }
        public MQCountMessage(int count)
        {
            this.Count = count;
        }
    }
}
