using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Keys = System.Windows.Forms.Keys;

namespace Paway.WPF
{
    /// <summary>
    /// </summary>
    internal class KeyboardHelper
    {
        #region API
        /// <summary>
        /// 于合成键盘事件和鼠标事件，用来模拟鼠标或者键盘操作。
        /// </summary>
        /// <param name="cInputs">函数第二个参数pInputs的数组个数</param>
        /// <param name="pInputs">INPUT结构体指针，实例化对象时每个对象代表一个模拟操作动作</param>
        /// <param name="cbSize">输入结构的大小（以字节为单位），可用sizeof( )方式获取</param>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint cInputs, Input[] pInputs, int cbSize);

        #endregion
        #region API结构体
        /// <summary>
        /// 鼠标输入
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            /// <summary>
            /// </summary>
            public int X;

            /// <summary>
            /// </summary>
            public int Y;

            /// <summary>
            /// </summary>
            public uint MouseData;

            /// <summary>
            /// </summary>
            public uint Flags;

            /// <summary>
            /// </summary>
            public uint Time;

            /// <summary>
            /// </summary>
            public IntPtr ExtraInfo;
        }
        /// <summary>
        /// 输入结构
        /// </summary>
        public struct Input
        {
            /// <summary>
            /// </summary>
            public uint Type;

            /// <summary>
            /// </summary>
            public MouseKeybdHardwareInput Data;
        }
        /// <summary>
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct MouseKeybdHardwareInput
        {
            /// <summary>
            /// </summary>
            [FieldOffset(0)]
            public HardwareInput Hardware;

            /// <summary>
            /// </summary>
            [FieldOffset(0)]
            public KeybdInput Keyboard;

            /// <summary>
            /// </summary>
            [FieldOffset(0)]
            public MouseInput Mouse;
        }
        /// <summary>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct HardwareInput
        {
            /// <summary>
            /// </summary>
            public uint Msg;

            /// <summary>
            /// </summary>
            public ushort ParamL;

            /// <summary>
            /// </summary>
            public ushort ParamH;
        }
        /// <summary>
        /// 键盘输入
        /// </summary>
        public struct KeybdInput
        {
            /// <summary>
            /// </summary>
            public ushort Vk;

            /// <summary>
            /// </summary>
            public ushort Scan;

            /// <summary>
            /// </summary>
            public uint Flags;

            /// <summary>
            /// </summary>
            public uint Time;

            /// <summary>
            /// </summary>
            public IntPtr ExtraInfo;
        }

        #endregion
        #region API常量
        /// <summary>
        /// 如果指定， 则 wScan 扫描代码由两个字节组成的序列组成，其中第一个字节的值为 0xE0。
        /// </summary>
        public const uint KEYEVENTF_EXTENDEDKEY = 1u;
        /// <summary>
        /// 如果指定，则正在释放按键。如果未指定，则正在按下该键。
        /// </summary>
        public const uint KEYEVENTF_KEYUP = 2u;
        /// <summary>
        /// 如果指定，系统会合成 VK_PACKET 击键。 wVk 参数必须为零。 此标志只能与 KEYEVENTF_KEYUP 标志组合使用。 有关详细信息，请参见“备注”部分。
        /// </summary>
        public const uint KEYEVENTF_UNICODE = 4u;


        #endregion

        #region 键盘钩子
        private static int processPointer;
        private static List<Action<int, bool>> callbacks;
        private static readonly HashSet<int> downKeys = new HashSet<int>();

        private readonly static NativeMethods.HookProc hookDelegate = HookCallback;
        internal static void StartHook(Action<int, bool> callback)
        {
            if (callbacks == null)
            {
                callbacks = new List<Action<int, bool>>();
            }
            if (callbacks.Contains(callback))
            {
                return;
            }
            callbacks.Add(callback);
            processPointer = SetHook(hookDelegate);
        }
        private static int SetHook(NativeMethods.HookProc hookDelegate)
        {
            int idHook;
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule mainModule = currentProcess.MainModule)
                {
                    idHook = NativeMethods.SetWindowsHookEx(HookType.WH_KEYBORARD_LL, hookDelegate, NativeMethods.GetModuleHandle(mainModule.ModuleName), 0);
                }
            }
            return idHook;
        }
        private static int HookCallback(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int num = Marshal.ReadInt32(lParam);
                bool flag = false;
                bool arg = false;
                switch ((int)wParam)
                {
                    case 256:
                    case 260:
                        if (downKeys.Contains(num))
                        {
                            flag = true;
                        }
                        else
                        {
                            downKeys.Add(num);
                        }
                        break;
                    case 257:
                    case 261:
                        arg = true;
                        downKeys.Remove(num);
                        break;
                }
                if (!flag)
                {
                    foreach (Action<int, bool> action in callbacks)
                    {
                        action(num, arg);
                    }
                }
            }
            return NativeMethods.CallNextHookEx(processPointer, nCode, wParam, lParam);
        }
        internal static void StopHook()
        {
            callbacks?.Clear();
            NativeMethods.UnhookWindowsHookEx(processPointer);
        }

        #endregion

        #region 发送按键
        public static void Send(Keys virtualKey, bool iUnicode = false)
        {
            Send(null, (int)virtualKey, iUnicode);
        }
        public static void Send(ICollection<int> modifierKeys, int virtualKey, bool iUnicode = false)
        {
            if (virtualKey <= 0)
            {
                return;
            }
            if (modifierKeys == null || modifierKeys.Count == 0)
            {
                Input input = CreateKeyDownInput(virtualKey, iUnicode);
                Input input2 = CreateKeyUpInput(virtualKey, iUnicode);
                Input[] inputs = new Input[]
                {
                    input,
                    input2
                };
                if (SendInput(2u, inputs, Marshal.SizeOf(typeof(Input))) == 0u)
                {
                    throw new InvalidOperationException("Could not send key. VirtualKeyboardNativeMethodXs.SendInput function returned 0.");
                }
            }
            else
            {
                Input[] array = new Input[modifierKeys.Count * 2 + 2];
                int num = 0;
                foreach (int keyCode in modifierKeys)
                {
                    array[num++] = CreateKeyDownInput(keyCode, iUnicode);
                }
                array[num++] = CreateKeyDownInput(virtualKey, iUnicode);
                array[num++] = CreateKeyUpInput(virtualKey, iUnicode);
                foreach (int keyCode2 in modifierKeys)
                {
                    array[num++] = CreateKeyUpInput(keyCode2, iUnicode);
                }
                if (SendInput((uint)array.Length, array, Marshal.SizeOf(typeof(Input))) == 0u)
                {
                    throw new InvalidOperationException("Could not send key. VirtualKeyboardNativeMethodXs.SendInput function returned 0.");
                }
            }
        }

        private static Input CreateKeyDownInput(int keyCode, bool iUnicode = false)
        {
            Input result = default;
            result.Type = 1u;
            result.Data.Keyboard = new KeybdInput
            {
                Vk = iUnicode ? (ushort)0 : (ushort)keyCode,
                Scan = iUnicode ? (ushort)keyCode : (ushort)0,
                Flags = (iUnicode ? KEYEVENTF_UNICODE : 0) + (IsExtendedKey(keyCode) ? KEYEVENTF_EXTENDEDKEY : 0),
                Time = 0u,
                ExtraInfo = IntPtr.Zero
            };
            return result;
        }
        private static Input CreateKeyUpInput(int keyCode, bool iUnicode = false)
        {
            Input result = default;
            result.Type = 1u;
            result.Data.Keyboard = new KeybdInput
            {
                Vk = iUnicode ? (ushort)0 : (ushort)keyCode,
                Scan = iUnicode ? (ushort)keyCode : (ushort)0,
                Flags = (iUnicode ? KEYEVENTF_UNICODE : 0) + KEYEVENTF_KEYUP + (IsExtendedKey(keyCode) ? KEYEVENTF_EXTENDEDKEY : 0),
                Time = 0u,
                ExtraInfo = IntPtr.Zero
            };
            return result;
        }
        private static bool IsExtendedKey(int virtualKeyCode)
        {
            return virtualKeyCode == 3 ||
                  virtualKeyCode == 18 ||
                  virtualKeyCode == 44 || virtualKeyCode == 45 || virtualKeyCode == 46 ||
                  virtualKeyCode == 33 || virtualKeyCode == 34 || virtualKeyCode == 35 || virtualKeyCode == 36 || virtualKeyCode == 37 || virtualKeyCode == 38 || virtualKeyCode == 39 || virtualKeyCode == 40 ||
                  virtualKeyCode == 111 ||
                  virtualKeyCode == 144 ||
                  virtualKeyCode == 162 || virtualKeyCode == 163 || virtualKeyCode == 164 || virtualKeyCode == 165;
        }

        #endregion
    }
}
