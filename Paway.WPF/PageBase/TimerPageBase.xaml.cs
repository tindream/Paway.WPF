using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Paway.WPF
{
    /// <summary>
    /// 定时器页
    /// </summary>
    public abstract partial class TimerPageBase : Page
    {
        private readonly DispatcherTimer dispatcherTimer;
        private int Time = 30;
        private TimeSpan Interval = new TimeSpan(0, 0, 1);
        private Action<int> Timing;
        private Action Timeout;

        #region 定时器
        public TimerPageBase()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            this.Loaded += TimerBase_Loaded;
            this.Unloaded += TimerBase_Unloaded;
        }
        protected virtual void Init(int time = 30, int interval = 1, Action<int> timing = null, Action timeout = null)
        {
            this.Time = time;
            this.Interval = new TimeSpan(0, 0, interval);
            this.Timing = timing;
            this.Timeout = timeout;
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Time--;
            this.Timing?.Invoke(Time);
            if (Time == 0)
            {
                this.Timeout?.Invoke();
            }
        }
        private void TimerBase_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Interval = Interval;
            dispatcherTimer.Start();
        }
        private void TimerBase_Unloaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.IsEnabled = false;
        }

        #endregion
    }
}
