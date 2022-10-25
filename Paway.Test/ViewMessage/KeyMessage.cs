using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Paway.Test
{
    public class KeyMessage
    {
        public Key Key { get; }
        public bool Cancel { get; }

        public KeyMessage() { }
        public KeyMessage(Key key)
        {
            this.Key = key;
        }
    }
}
