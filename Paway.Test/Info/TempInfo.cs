using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class TempInfo : ModelBase
    {
        public DateTime DateTime { get; set; }
        private double _value;
        /// <summary>
        /// 值
        /// </summary>
        public double Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
        }

        private string _values;
        /// <summary>
        /// 值
        /// </summary>
        public string Values
        {
            get { return _values; }
            set { _values = value; OnPropertyChanged(); }
        }

        public TempInfo() { }
        public TempInfo(DateTime time, double value)
        {
            this.DateTime = time;
            Value = value;
        }
    }
    public class RateInfo : ModelBase
    {
        public string Text { get; set; }

        private double x;
        public double X
        {
            get { return x; }
            set
            {
                var temp = value;
                if (this.Text == null) { }
                else if (temp > Config.MaxRate) temp = Config.MaxRate;
                else if (temp < Config.MinRate) temp = Config.MinRate;
                x = temp;
                Rates = $"{Math.Pow(x, Config.Zoom).ToInt()}";
            }
        }
        private int rate;
        public int Rate
        {
            get { return rate; }
            set
            {
                rate = value;
                Rates = $"{value}";
                X = Math.Pow(value, 1.0 / Config.Zoom);
                OnPropertyChanged();
            }
        }
        private string rates;
        public string Rates
        {
            get { return rates; }
            set
            {
                rates = value;
                if (Rate != value.ToInt())
                    Rate = value.ToInt();
                OnPropertyChanged();
            }
        }

        private double increase;
        public double Increase
        {
            get { return increase; }
            set
            {
                var temp = value;
                if (temp > Config.MaxIncrease) temp = Config.MaxIncrease;
                else if (temp < Config.MinIncrease) temp = Config.MinIncrease;
                increase = temp;
                Increases = Method.Rounds(value, 1, 1);
                OnPropertyChanged();
            }
        }
        private string increases;
        public string Increases
        {
            get { return increases; }
            set
            {
                increases = value;
                if (Method.Round(Increase, 1) != Method.Round(Increase.ToDouble(), 1))
                    Increase = value.ToDouble();
                OnPropertyChanged();
            }
        }

        public RateInfo() { }
        public RateInfo(string text, int x, double value)
        {
            this.Text = text;
            this.X = Math.Pow(x, 1.0 / Config.Zoom);
            this.Increase = value;
        }
    }
}
