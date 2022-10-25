using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.Test
{
    public class LevelMessage
    {
        public string Msg { get; set; }
        public LeveType Level { get; set; }

        public LevelMessage() { }
        public LevelMessage(string msg)
        {
            this.Msg = msg;
            this.Level = LeveType.Debug;
        }
        public LevelMessage(string msg, LeveType level)
        {
            this.Msg = msg;
            this.Level = level;
        }
    }
    public class StatuMessage : LevelMessage
    {
        public bool IHit { get; set; } = true;
        public DependencyObject Ower { get; set; }
        public StatuMessage(string msg, bool iHit = true) : base(msg)
        {
            this.IHit = iHit;
        }
        public StatuMessage(string msg, DependencyObject ower) : base(msg)
        {
            this.Ower = ower;
        }
        public StatuMessage(string msg, LeveType level, DependencyObject ower = null) : base(msg, level)
        {
            this.Ower = ower;
        }
        public StatuMessage(Exception ex, DependencyObject ower = null) : this(null, ex)
        {
            this.Ower = ower;
        }
        public StatuMessage(string title, Exception ex, DependencyObject ower = null)
        {
            ex.Log();
            var msg = $"{ex.Message().Replace("\r\n", "。")}";
            if (!title.IsEmpty()) msg = $"{title}: {msg}";
            this.Msg = msg;
            this.Level = ex is WarningException ? LeveType.Warn : LeveType.Error;
            this.Ower = ower;
        }
    }
}
