using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    [Serializable]
    public class HttpResponseMessage
    {
        public int code { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
        public int total { get; set; }

        public HttpResponseMessage() { }
        public HttpResponseMessage(bool result, string msg, object data = null, int total = 0) : this()
        {
            this.code = result ? 200 : 400;
            this.msg = msg;
            this.data = data;
            this.total = total;
        }
    }
}
