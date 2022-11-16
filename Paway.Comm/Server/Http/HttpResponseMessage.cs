using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    [Serializable]
    public class HttpResponseMessage
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }
        public int Total { get; set; }

        public HttpResponseMessage() { }
        public HttpResponseMessage(bool result, string msg, object data = null, int total = 0) : this()
        {
            this.Code = result ? 200 : 400;
            this.Msg = msg;
            this.Data = data;
            this.Total = total;
        }
    }
}
