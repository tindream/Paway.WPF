using CommunityToolkit.Mvvm.Messaging;
using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Paway.Model
{
    /// <summary>
    /// 状态栏模型-需优先初始化Init()，以接收消息
    /// </summary>
    public partial class StatuItemModel : ViewModelBasePlus
    {
        #region 属性
        private string _timeNow;
        /// <summary>
        /// 当前时间
        /// </summary>
        public string TimeNow
        {
            get { return _timeNow; }
            set { if (_timeNow != value) { _timeNow = value; OnPropertyChanged(); } }
        }
        private string _userName;
        /// <summary>
        /// 用户
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }
        private string _desc;
        /// <summary>
        /// 当前状态描述
        /// </summary>
        public string Desc
        {
            get { return _desc; }
            set { _desc = value; OnPropertyChanged(); }
        }
        private string _host;
        /// <summary>
        /// 主机
        /// </summary>
        public string Host
        {
            get { return _host; }
            set { _host = value; OnPropertyChanged(); }
        }

        private Brush _descBrush = ColorType.High.Color().ToBrush();
        /// <summary>
        /// 描述颜色
        /// </summary>
        public Brush DescBrush
        {
            get { return _descBrush; }
            set { if (_descBrush != value) { _descBrush = value; OnPropertyChanged(); } }
        }
        private Brush _connectBrush = ColorType.Warn.Color().ToBrush();
        /// <summary>
        /// 连接状态
        /// </summary>
        public Brush ConnectBrush
        {
            get { return _connectBrush; }
            set { if (_connectBrush != value) { _connectBrush = value; OnPropertyChanged(); } }
        }
        /// <summary>
        /// 时间更新外部处理事件
        /// <para>默认为：yyyy-MM-dd HH:mm:ss dddd</para>
        /// </summary>
        public event Func<string> TimeChangeEvent;

        #endregion

        /// <summary>
        /// 提前初始化-以接收消息
        /// </summary>
        internal void Init() { }
        /// <summary>
        /// 状态栏模型-需优先初始化Init()，以接收消息
        /// </summary>
        public StatuItemModel()
        {
            //用具名方法而非 lambda，才能在 Unregister 时真正解绑静态事件
            PConfig.OperateLogEvent += OperateLogAdd;
            PConfig.StatuLogEvent += StatuLogAdd;
            WeakReferenceMessenger.Default.Register<StatuMessage>(this, (obj, msg) => AddDesc(msg.Msg, msg.Level, msg.IHit, msg.Ower));
            WeakReferenceMessenger.Default.Register<ConnectMessage>(this, (obj, msg) =>
            {
                if (!msg.Message.IsEmpty()) WeakReferenceMessenger.Default.Send(new StatuMessage(msg.Message, msg.Connectd ? LevelType.Info : LevelType.Error));
                this.BeginInvoke(() =>
                {
                    ConnectBrush = msg.Connectd ? ColorType.Success.Color().ToBrush() : ColorType.Error.Color().ToBrush();
                });
            });
            WeakReferenceMessenger.Default.Register<LoginMessage>(this, (obj, msg) =>
            {
                var hour = DateTime.Now.Hour;
                var hello = hour < 8 ? "早上好" : hour <= 11 ? "上午好" :
                            hour <= 13 ? "中午好" : hour < 18 ? "下午好" : "晚上好";
                this.UserName = $"{hello}, {msg.UserName}";
            });
            //原实现是 Task.Run 里的无限 while 循环：
            //1) 循环体抛异常会静默终止时钟且无任何日志；2) 宿主不设置 PConfig.Window 时循环永不退出；
            //3) TimeChangeEvent 在后台线程回调，宿主一旦改成 TwoWay 绑定或加转换器即跨线程异常。
            //DispatcherTimer 在UI线程触发，天然安全且可停止。
            if (timeTimer == null)
            {
                timeTimer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1) };
                timeTimer.Tick += TimeTimer_Tick;
            }
            timeTimer.Start();
        }
        private DispatcherTimer timeTimer;
        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            //主窗体关闭后停止计时并解绑静态事件（原先由循环退出时顺带调用）
            if (PConfig.IClose)
            {
                Unregister();
                return;
            }
            try
            {
                this.TimeNow = TimeChangeEvent != null ? TimeChangeEvent.Invoke() : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss dddd");
            }
            catch (Exception ex)
            {
                //单个宿主回调异常不应让时钟静默停摆
                ex.Log("刷新状态栏时间失败");
            }
        }
        private void OperateLogAdd(OperateInfo msg) { AddDesc(msg.Text); }
        private void StatuLogAdd(string msg, LevelType level, object source) { AddDesc(msg, level); }
        /// <summary>
        /// 解绑静态事件与定时器
        /// </summary>
        internal void Unregister()
        {
            if (timeTimer != null)
            {
                timeTimer.Stop();
                timeTimer.Tick -= TimeTimer_Tick;
                timeTimer = null;
            }
            PConfig.OperateLogEvent -= OperateLogAdd;
            PConfig.StatuLogEvent -= StatuLogAdd;
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        #region 系统消息
        /// <summary>
        /// 系统消息
        /// </summary>
        private void AddDesc(string msg, LevelType level = LevelType.Info, bool iHit = false, DependencyObject ower = null)
        {
            this.Desc = msg;
            msg.Log(level);
            this.BeginInvoke(() =>
            {
                switch (level)
                {
                    case LevelType.Warn:
                        DescBrush = PConfig.Warn.ToBrush();
                        if (iHit) PMethod.Hit(ower ?? PConfig.Window, msg, ColorType.Warn);
                        break;
                    case LevelType.Error:
                    case LevelType.Fatal:
                        DescBrush = PConfig.Error.ToBrush();
                        if (iHit) PMethod.Hit(ower ?? PConfig.Window, msg, ColorType.Error);
                        break;
                    default:
                        DescBrush = ColorType.High.Color().ToBrush();
                        if (iHit) PMethod.Toast(ower ?? PConfig.Window, msg);
                        break;
                }
            });
        }

        #endregion
    }
}