using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Keys = System.Windows.Forms.Keys;

namespace Paway.WPF
{
    /// <summary>
    /// KeyboardAll.xaml 的交互逻辑
    /// </summary>
    public partial class KeyboardAll : UserControl
    {
        /// <summary>
        /// 大写
        /// </summary>
        private bool iCapsLock;
        /// <summary>
        /// 数字键盘与字符
        /// </summary>
        private bool iKeyboardNum;
        /// <summary>
        /// 中英文标记
        /// </summary>
        private bool iChina;
        /// <summary>
        /// 关闭事件
        /// </summary>
        public event Action CloseEvent;
        /// <summary>
        /// 点击空白处拖动窗体后事件
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> DragMovedEvent;
        private static Dictionary<string, KeyboardKeyInfo> KeyList;
        static KeyboardAll()
        {
            if (KeyList == null) KeyList = new Dictionary<string, KeyboardKeyInfo>();
            else return;
            KeyList.Add("q", new KeyboardKeyInfo("q", "Q", (int)Keys.Q, "1", (int)Keys.D1, false, "1", (int)Keys.D1, false));
            KeyList.Add("w", new KeyboardKeyInfo("w", "W", (int)Keys.W, "2", (int)Keys.D2, false, "2", (int)Keys.D2, false));
            KeyList.Add("e", new KeyboardKeyInfo("e", "E", (int)Keys.E, "3", (int)Keys.D3, false, "3", (int)Keys.D3, false));
            KeyList.Add("r", new KeyboardKeyInfo("r", "R", (int)Keys.R, "4", (int)Keys.D4, false, "4", (int)Keys.D4, false));
            KeyList.Add("t", new KeyboardKeyInfo("t", "T", (int)Keys.T, "5", (int)Keys.D5, false, "5", (int)Keys.D5, false));
            KeyList.Add("y", new KeyboardKeyInfo("y", "Y", (int)Keys.Y, "6", (int)Keys.D6, false, "6", (int)Keys.D6, false));
            KeyList.Add("u", new KeyboardKeyInfo("u", "U", (int)Keys.U, "7", (int)Keys.D7, false, "7", (int)Keys.D7, false));
            KeyList.Add("i", new KeyboardKeyInfo("i", "I", (int)Keys.I, "8", (int)Keys.D8, false, "8", (int)Keys.D8, false));
            KeyList.Add("o", new KeyboardKeyInfo("o", "O", (int)Keys.O, "9", (int)Keys.D9, false, "9", (int)Keys.D9, false));
            KeyList.Add("p", new KeyboardKeyInfo("p", "P", (int)Keys.P, "0", (int)Keys.D0, false, "0", (int)Keys.D0, false));

            KeyList.Add("a", new KeyboardKeyInfo("a", "A", (int)Keys.A, "@", (int)Keys.D2, true, "@", (int)Keys.D2, true));
            KeyList.Add("s", new KeyboardKeyInfo("s", "S", (int)Keys.S, "#", (int)Keys.D3, true, "#", (int)Keys.D3, true));
            KeyList.Add("d", new KeyboardKeyInfo("d", "D", (int)Keys.D, "%", (int)Keys.D5, true, "%", (int)Keys.D5, true));
            KeyList.Add("f", new KeyboardKeyInfo("f", "F", (int)Keys.F, "/", (int)Keys.OemQuestion, false, "/", BitConverter.ToUInt16(Encoding.Unicode.GetBytes("/"), 0), false, true));
            KeyList.Add("g", new KeyboardKeyInfo("g", "G", (int)Keys.G, "(", (int)Keys.D9, true, "（", BitConverter.ToUInt16(Encoding.Unicode.GetBytes("（"), 0), false, true));
            KeyList.Add("h", new KeyboardKeyInfo("h", "H", (int)Keys.H, ")", (int)Keys.D0, true, "）", BitConverter.ToUInt16(Encoding.Unicode.GetBytes("）"), 0), false, true));
            KeyList.Add("j", new KeyboardKeyInfo("j", "J", (int)Keys.J, "+", (int)Keys.Add, true, "+", (int)Keys.Add, true));
            KeyList.Add("k", new KeyboardKeyInfo("k", "K", (int)Keys.K, "-", (int)Keys.Subtract, false, "-", (int)Keys.Subtract, false));
            KeyList.Add("l", new KeyboardKeyInfo("l", "L", (int)Keys.L, "=", (int)Keys.Oemplus, false, "=", (int)Keys.Oemplus, false));

            KeyList.Add("caps lock", new KeyboardKeyInfo(null, null, 0, ",", (int)Keys.Oemcomma, false, "，", BitConverter.ToUInt16(Encoding.Unicode.GetBytes("，"), 0), false, true));
            KeyList.Add("z", new KeyboardKeyInfo("z", "Z", (int)Keys.Z, ":", (int)Keys.OemSemicolon, true, "：", BitConverter.ToUInt16(Encoding.Unicode.GetBytes("："), 0), false, true));
            KeyList.Add("x", new KeyboardKeyInfo("x", "X", (int)Keys.X, ";", (int)Keys.OemSemicolon, false, "；", BitConverter.ToUInt16(Encoding.Unicode.GetBytes("；"), 0), false, true));
            KeyList.Add("c", new KeyboardKeyInfo("c", "C", (int)Keys.C, "'", (int)Keys.OemQuotes, false, "’", BitConverter.ToUInt16(Encoding.Unicode.GetBytes("’"), 0), false, true));
            KeyList.Add("v", new KeyboardKeyInfo("v", "V", (int)Keys.V, "\"", (int)Keys.OemQuotes, true, "”", BitConverter.ToUInt16(Encoding.Unicode.GetBytes("”"), 0), false, true));
            KeyList.Add("b", new KeyboardKeyInfo("b", "B", (int)Keys.B, "!", (int)Keys.D1, true, "！", BitConverter.ToUInt16(Encoding.Unicode.GetBytes("！"), 0), false, true));
            KeyList.Add("n", new KeyboardKeyInfo("n", "N", (int)Keys.N, "?", (int)Keys.OemQuestion, true, "？", BitConverter.ToUInt16(Encoding.Unicode.GetBytes("？"), 0), false, true));
            KeyList.Add("m", new KeyboardKeyInfo("m", "M", (int)Keys.M, "\\", (int)Keys.OemPipe, false, "、", BitConverter.ToUInt16(Encoding.Unicode.GetBytes("、"), 0), false, true));

            KeyList.Add(".", new KeyboardKeyInfo(".", ".", (int)Keys.Decimal, ".", (int)Keys.Decimal, false, "。", BitConverter.ToUInt16(Encoding.Unicode.GetBytes("。"), 0), false, true));
        }

        /// <summary>
        /// 虚拟键盘-全键盘
        /// </summary>
        public KeyboardAll()
        {
            InitializeComponent();
            listview1.SelectionChanged += Listview1_SelectionChanged;
            listview1.DragMovedEvent += Listview1_DragMovedEvent;
        }
        private void Listview1_DragMovedEvent(object sender, MouseButtonEventArgs e)
        {
            DragMovedEvent?.Invoke(sender, e);
        }
        /// <summary>
        /// 监听键盘，切换中英文
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            KeyboardHelper.StartHook(new Action<int, bool>(this.KeyPressed));
            this.iChina = InputMethod.Current.ImeState == InputMethodState.On && InputMethod.Current.ImeConversionMode != ImeConversionModeValues.Alphanumeric;
            this.iCapsLock = Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.Toggled;
            if (this.iCapsLock) this.ChangeCapsLock();
            if (this.iChina) this.ChangeChina();
        }
        private void KeyPressed(int virtualKey, bool keyUp)
        {
            switch (virtualKey)
            {
                case (int)Keys.CapsLock: if (!keyUp) this.iCapsLock = KeyboardHelper.GetKeyState((int)Keys.CapsLock) == 0; this.ChangeCapsLock(); break;
                case (int)Keys.LShiftKey:
                case (int)Keys.RShiftKey: if (keyUp) { PMethod.DoEvents(); this.ChangeChina(); } break;
            }
        }

        private void Listview1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListViewCustom listView && listView.SelectedItem is ListBoxItemEXT viewItem)
            {
                var key = viewItem.Tag.ToString();
                switch (key)
                {
                    case "caps lock":
                        if (this.iKeyboardNum)
                        {
                            SendKey(key);
                            break;
                        }
                        else
                        {
                            var lastCapsLock = this.iCapsLock;
                            KeyboardHelper.Send(Keys.CapsLock);
                            this.iCapsLock = !lastCapsLock;
                            this.ChangeCapsLock();
                        }
                        break;
                    case "键盘": this.iKeyboardNum = !this.iKeyboardNum; this.ChangeKeyboardNum(); break;
                    case "中英":
                        InputMethod.Current.ImeState = this.iChina ? InputMethodState.Off : InputMethodState.On;
                        InputMethod.Current.ImeConversionMode = this.iChina ? ImeConversionModeValues.Alphanumeric : (ImeConversionModeValues.Native | ImeConversionModeValues.Symbol);
                        ChangeChina();
                        break;
                    case "backspace": KeyboardHelper.Send(Keys.Back); break;
                    case "space": KeyboardHelper.Send(Keys.Space); break;
                    case "enter": KeyboardHelper.Send(Keys.Enter); break;
                    default: SendKey(key); break;
                    case "关闭": this.OnCloseEvent(); break;
                }
                listView.SelectedIndex = -1;
            }
        }
        private void SendKey(string key)
        {
            var value = this.iKeyboardNum ? (this.iChina ? KeyList[key].NumCnV : KeyList[key].NumEnV) : KeyList[key].AllValue;
            var iModifierKey = this.iKeyboardNum ? (this.iChina ? KeyList[key].INumCnShift : KeyList[key].INumEnShift) : false;
            var modifierKeys = new List<int>();
            if (iModifierKey) modifierKeys.Add((int)Keys.ShiftKey);
            KeyboardHelper.Send(modifierKeys, value, (this.iKeyboardNum && this.iChina) ? KeyList[key].IUnicode : false);
        }
        /// <summary>
        /// 切换中英文键盘
        /// </summary>
        private void ChangeChina()
        {
            this.iChina = InputMethod.Current.ImeState == InputMethodState.On && InputMethod.Current.ImeConversionMode != ImeConversionModeValues.Alphanumeric;
            if (this.iKeyboardNum) this.ChangeKeyboardNum();
            else this.ChangeCapsLock();
        }
        /// <summary>
        /// 切换数字键盘
        /// </summary>
        private void ChangeKeyboardNum()
        {
            foreach (ListBoxItemEXT viewItem in listview1.Items)
            {
                var key = viewItem.Tag.ToString();
                switch (key)
                {
                    case "中英": viewItem.Text = this.iChina ? "中" : "英"; break;
                    case "caps lock":
                        if (this.iKeyboardNum)
                        {
                            viewItem.Text = this.iChina ? KeyList[key].NumCn : KeyList[key].NumEn;
                            viewItem.Image = new ImageEXT();
                        }
                        else
                        {
                            viewItem.Text = null;
                            var imageName = this.iCapsLock ? "caps_lock_2" : "caps_lock";
                            viewItem.Image = new ImageEXT($"pack://application:,,,/Paway.WPF;component/Images/keyboard/{imageName}_white.png",
                                $"pack://application:,,,/Paway.WPF;component/Images/keyboard/{imageName}.png",
                                $"pack://application:,,,/Paway.WPF;component/Images/keyboard/{imageName}_white.png");
                        }
                        break;
                    case "backspace": break;
                    case "键盘": viewItem.Text = this.iKeyboardNum ? "abc." : "?123"; break;
                    case "space":
                    case "关闭":
                    case "enter": break;
                    default: viewItem.Text = this.iKeyboardNum ? (this.iChina ? KeyList[key].NumCn : KeyList[key].NumEn) : (this.iCapsLock ? KeyList[key].AllUp : KeyList[key].AllLower); break;
                }
            }
        }
        /// <summary>
        /// 切换大小写
        /// </summary>
        private void ChangeCapsLock()
        {
            foreach (ListBoxItemEXT viewItem in listview1.Items)
            {
                var key = viewItem.Tag.ToString();
                switch (key)
                {
                    case "中英": viewItem.Text = this.iChina ? "中" : "英"; break;
                    case "caps lock":
                        var imageName = this.iCapsLock ? "caps_lock_2" : "caps_lock";
                        viewItem.Image = new ImageEXT($"pack://application:,,,/Paway.WPF;component/Images/keyboard/{imageName}_white.png",
                            $"pack://application:,,,/Paway.WPF;component/Images/keyboard/{imageName}.png",
                            $"pack://application:,,,/Paway.WPF;component/Images/keyboard/{imageName}_white.png");
                        break;
                    case "backspace":
                    case "键盘":
                    case "space":
                    case "关闭":
                    case "enter": break;
                    default: viewItem.Text = this.iCapsLock ? KeyList[key].AllUp : KeyList[key].AllLower; break;
                }
            }
        }
        private void OnCloseEvent()
        {
            if (CloseEvent != null) CloseEvent.Invoke();
            else this.Visibility = Visibility.Collapsed;
        }
    }
    internal class KeyboardKeyInfo
    {
        /// <summary>
        /// 全键盘-小写
        /// </summary>
        public string AllLower { get; set; }
        /// <summary>
        /// 全键盘-大写
        /// </summary>
        public string AllUp { get; set; }
        /// <summary>
        /// 全键盘-值
        /// </summary>
        public int AllValue { get; set; }

        /// <summary>
        /// 数字键盘-英文模式
        /// </summary>
        public string NumEn { get; set; }
        /// <summary>
        /// 数字键盘-英文模式-值
        /// </summary>
        public int NumEnV { get; set; }
        /// <summary>
        /// 数字键盘-英文模式-需要Shift标记
        /// </summary>
        public bool INumEnShift { get; set; }

        /// <summary>
        /// 数字键盘-中文模式
        /// </summary>
        public string NumCn { get; set; }
        /// <summary>
        /// 数字键盘-中文模式-值
        /// </summary>
        public int NumCnV { get; set; }
        /// <summary>
        /// 数字键盘-中文模式-需要Shift标记
        /// </summary>
        public bool INumCnShift { get; set; }
        /// <summary>
        /// 数字键盘-中文模式-启用Unicode模式标记
        /// </summary>
        public bool IUnicode { get; set; }

        public KeyboardKeyInfo() { }
        public KeyboardKeyInfo(string allLower, string allUp, int allValue, string numEn, int numEnV, bool iNumEnShift, string numCn, int numCnV, bool iNumCnShift, bool iUnicode = false)
        {
            this.AllLower = allLower;
            this.AllUp = allUp;
            this.AllValue = allValue;

            this.NumEn = numEn;
            this.NumEnV = numEnV;
            this.INumEnShift = iNumEnShift;

            this.NumCn = numCn;
            this.NumCnV = numCnV;
            this.INumCnShift = iNumCnShift;
            this.IUnicode = iUnicode;
        }
    }
}
