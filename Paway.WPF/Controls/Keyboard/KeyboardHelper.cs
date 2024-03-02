﻿using System;
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
        /// 欲测试的虚拟键键码。对字母、数字字符（A-Z、a-z、0-9），用它们实际的ASCII值
        /// </summary>
        [DllImport("user32.dll", EntryPoint = "GetKeyState")]
        internal static extern int GetKeyState(int nVirtKey);

        /// <summary>
        /// 于合成键盘事件和鼠标事件，用来模拟鼠标或者键盘操作。
        /// </summary>
        /// <param name="cInputs">函数第二个参数pInputs的数组个数</param>
        /// <param name="pInputs">INPUT结构体指针，实例化对象时每个对象代表一个模拟操作动作</param>
        /// <param name="cbSize">输入结构的大小（以字节为单位），可用sizeof( )方式获取</param>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint cInputs, Input[] pInputs, int cbSize);

        /// <summary>
        /// 钩子(Hook)，是Windows消息处理机制的一个平台，应用程序可以在上面设置子程以监视指定窗口的某种消息，而且所监视的窗口可以是其他进程所创建的。
        /// 当消息到达后，在目标窗口处理函数之前处理它。钩子机制允许应用程序截获处理window消息或特定事件。
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookDelegate lpfn, IntPtr hMod, uint dwThreadId);

        /// <summary>
        /// 卸载钩子
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

        /// <summary>
        /// <para>调用下一个钩子</para>
        /// <para>会返回下一个钩子执行后的返回值; 0 表示失败</para>
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 获取一个应用程序或动态链接库的模块句柄
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);

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
        /// 此挂钩只能在Windows NT中被安装,用来对底层的键盘输入事件进行监视
        /// </summary>
        public const int WH_KEYBOARD_LL = 13;
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
        private static IntPtr processPointer = IntPtr.Zero;
        private static List<Action<int, bool>> callbacks;
        private static readonly HashSet<int> downKeys = new HashSet<int>();

        internal delegate IntPtr KeyboardHookDelegate(int nCode, IntPtr wParam, IntPtr lParam);
        private static readonly KeyboardHookDelegate keyboardHookDelegate = new KeyboardHookDelegate(HookCallback);
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
            processPointer = SetHook(keyboardHookDelegate);
        }
        private static IntPtr SetHook(KeyboardHookDelegate hookDelegate)
        {
            IntPtr result;
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule mainModule = currentProcess.MainModule)
                {
                    result = SetWindowsHookEx(WH_KEYBOARD_LL, hookDelegate, GetModuleHandle(mainModule.ModuleName), 0u);
                }
            }
            return result;
        }
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
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
            return CallNextHookEx(processPointer, nCode, wParam, lParam);
        }
        internal static void StopHook()
        {
            if (callbacks != null) callbacks.Clear();
            UnhookWindowsHookEx(processPointer);
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
            Input result = default(Input);
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
            Input result = default(Input);
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
