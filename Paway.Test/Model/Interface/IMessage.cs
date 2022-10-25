using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public interface IMessage
    {
        /// <summary>
        /// 命令
        /// </summary>
        MealType Type { get; set; }
        //MqttQualityOfServiceLevel Level { get; set; }

        byte[] Buffer();
        void Parse(byte[] data);
    }
}
