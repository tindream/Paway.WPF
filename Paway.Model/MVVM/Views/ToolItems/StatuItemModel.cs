using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Paway.Comm;
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

namespace Paway.Model
{
    /// <summary>
    /// 需优先初始化Init()，以接收消息
    /// </summary>
    public partial class StatuItemModel : ViewModelBasePlus
    {
        #region 属性
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }
        private string _desc;
        public string Desc
        {
            get { return _desc; }
            set { _desc = value; OnPropertyChanged(); }
        }
        private string _timeNow;
        public string TimeNow
        {
            get { return _timeNow; }
            set { if (_timeNow != value) { _timeNow = value; OnPropertyChanged(); } }
        }

        private Brush descBrush = ColorType.High.Color().ToBrush();
        public Brush DescBrush
        {
            get { return descBrush; }
            set { if (descBrush != value) { descBrush = value; OnPropertyChanged(); } }
        }
        private Brush _connectBrush = ColorType.Warn.Color().ToBrush();
        public Brush ConnectBrush
        {
            get { return _connectBrush; }
            set { if (_connectBrush != value) { _connectBrush = value; OnPropertyChanged(); } }
        }
        private Brush _connect2Brush = ColorType.Warn.Color().ToBrush();
        /// <summary>
        /// 连接2状态
        /// </summary>
        public Brush Connect2Brush
        {
            get { return _connect2Brush; }
            set { if (_connect2Brush != value) { _connect2Brush = value; OnPropertyChanged(); } }
        }

        #endregion

        public StatuItemModel()
        {
            Config.OperateLogEvent += msg => AddDesc(msg.Text, iHit: false);

            Messenger.Default.Register<StatuMessage>(this, msg => AddDesc(msg.Msg, msg.Level, msg.IHit, msg.Ower));
            CConfig.StatuLogEvent += (msg, level) => AddDesc(msg, level);
            Messenger.Default.Register<ConnectMessage>(this, msg =>
            {
                Messenger.Default.Send(new StatuMessage(msg.Connectd ? $"连接成功" : $"连接断开", !msg.Connectd), msg.Connectd ? LeveType.Debug : LeveType.Error);
                Method.BeginInvoke(() =>
                {
                    ConnectBrush = msg.Connectd ? ColorType.Success.Color().ToBrush() : ColorType.Error.Color().ToBrush();
                });
            });
            Messenger.Default.Register<Connect2Message>(this, msg =>
            {
                Method.BeginInvoke(() =>
                {
                    Connect2Brush = msg.Connectd ? ColorType.Success.Color().ToBrush() : ColorType.Error.Color().ToBrush();
                });
            });
            Messenger.Default.Register<LoginMessage>(this, msg =>
            {
                var hour = DateTime.Now.Hour;
                var hello = hour < 8 ? "早上好" : hour <= 11 ? "上午好" :
                            hour <= 13 ? "中午好" : hour < 18 ? "下午好" : "晚上好";
                this.UserName = $"{hello}, {msg.UserName}";
            });
            Task.Run(() =>
            {
                this.TimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss dddd");
                Thread.Sleep(1000 - DateTime.Now.Millisecond);
                while (true)
                {
                    this.TimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss dddd");
                    Thread.Sleep(1000);
                }
            });
        }
        public void Init() { }

        #region 系统消息
        /// <summary>
        /// 系统消息
        /// </summary>
        private void AddDesc(string msg, LeveType level = LeveType.Debug, bool iHit = true, DependencyObject ower = null)
        {
            this.Desc = msg;
            Method.Invoke(() =>
            {
                switch (level)
                {
                    case LeveType.Warn:
                        DescBrush = Config.Warn.ToBrush();
                        if (iHit) Method.Hit(ower ?? Config.Window, msg, ColorType.Warn);
                        break;
                    case LeveType.Error:
                        DescBrush = Config.Error.ToBrush();
                        if (iHit) Method.Hit(ower ?? Config.Window, msg, ColorType.Error);
                        break;
                    default:
                        DescBrush = ColorType.High.Color().ToBrush();
                        if (iHit) Method.Toast(ower ?? Config.Window, msg);
                        break;
                }
            });
        }

        #endregion
    }
}