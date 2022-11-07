using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Paway.Model
{
    public class ConnectMessage
    {
        public bool Connectd { get; set; }

        public ConnectMessage() { }
        public ConnectMessage(bool connectd)
        {
            this.Connectd = connectd;
        }
    }
}
