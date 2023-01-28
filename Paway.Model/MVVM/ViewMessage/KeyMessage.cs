using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Paway.Model
{
    public class KeyMessage
    {
        public Key Key { get; }
        public bool Handled { get; set; }

        public KeyMessage() { }
        public KeyMessage(Key key)
        {
            this.Key = key;
        }
    }
}
