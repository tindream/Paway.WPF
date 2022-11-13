using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    [Serializable]
    public class ErrorMessage : MealMessage
    {
        public CommType FromType { get; set; }
        public string Message { get; set; }

        public ErrorMessage() : base(CommType.Error) { }
        public ErrorMessage(CommType type, string msg) : this()
        {
            this.FromType = type;
            this.Message = msg;
        }
    }
}
