using MQTTnet.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    [Serializable]
    public class MealMessage : IMessage
    {
        public CommType Type { get; set; }
        [JsonIgnore]
        public MqttQualityOfServiceLevel Level { get; set; }

        public MealMessage() { }
        public MealMessage(CommType type)
        {
            this.Type = type;
            this.Level = MqttQualityOfServiceLevel.AtLeastOnce;
        }
        public virtual byte[] Buffer() { return null; }
        public virtual void Parse(byte[] data) { }
    }
}
