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
    // Token: 0x02000123 RID: 291
    internal class NativeMethodXs
    {
        #region API
        // Token: 0x060019D5 RID: 6613
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

        // Token: 0x060019D6 RID: 6614
        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(uint idThread);

        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which 
        /// the user is currently working). The system assigns a slightly higher 
        /// priority to the thread that creates the foreground window than it 
        /// does to other threads.
        /// </summary>
        /// <returns>A handle to the foreground window.</returns>
        // Token: 0x06003B58 RID: 15192
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// The SendInput function synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        /// <param name="numberOfInputs">Number of structures in the Inputs array.</param>
        /// <param name="inputs">Pointer to an array of INPUT structures. Each structure represents an event to be inserted into the keyboard or mouse input stream.</param>
        /// <param name="sizeOfInputStructure">Specifies the size, in bytes, of an INPUT structure. If cbSize is not the size of an INPUT structure, the function fails.</param>
        /// <returns>The function returns the number of events that it successfully inserted into the keyboard or mouse input stream. If the function returns zero, the input was already blocked by another thread. To get extended error information, call GetLastError.Microsoft Windows Vista. This function fails when it is blocked by User Interface Privilege Isolation (UIPI). Note that neither GetLastError nor the return value will indicate the failure was caused by UIPI blocking.</returns>
        /// <remarks>
        /// Microsoft Windows Vista. This function is subject to UIPI. Applications are permitted to inject input only into applications that are at an equal or lesser integrity level.
        /// The SendInput function inserts the events in the INPUT structures serially into the keyboard or mouse input stream. These events are not interspersed with other keyboard or mouse input events inserted either by the user (with the keyboard or mouse) or by calls to keybd_event, mouse_event, or other calls to SendInput.
        /// This function does not reset the keyboard's current state. Any keys that are already pressed when the function is called might interfere with the events that this function generates. To avoid this problem, check the keyboard's state with the GetAsyncKeyState function and correct as necessary.
        /// </remarks>
        // Token: 0x060019D9 RID: 6617
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint numberOfInputs, NativeMethodXs.INPUT[] inputs, int sizeOfInputStructure);

        // Token: 0x060019E1 RID: 6625
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, NativeMethodXs.KeyboardHookDelegate lpfn, IntPtr hMod, uint dwThreadId);

        // Token: 0x060019E2 RID: 6626
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        // Token: 0x060019E3 RID: 6627
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        // Token: 0x060019E4 RID: 6628
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion
        #region API结构体
        // Token: 0x0200027D RID: 637
        public struct MOUSEINPUT
        {
            // Token: 0x04000CD6 RID: 3286
            public int X;

            // Token: 0x04000CD7 RID: 3287
            public int Y;

            // Token: 0x04000CD8 RID: 3288
            public uint MouseData;

            // Token: 0x04000CD9 RID: 3289
            public uint Flags;

            // Token: 0x04000CDA RID: 3290
            public uint Time;

            // Token: 0x04000CDB RID: 3291
            public IntPtr ExtraInfo;
        }

        // Token: 0x0200027E RID: 638
        public struct INPUT
        {
            // Token: 0x04000CDC RID: 3292
            public uint Type;

            // Token: 0x04000CDD RID: 3293
            public NativeMethodXs.MOUSEKEYBDHARDWAREINPUT Data;
        }

        // Token: 0x0200027F RID: 639
        [StructLayout(LayoutKind.Explicit)]
        public struct MOUSEKEYBDHARDWAREINPUT
        {
            // Token: 0x04000CDE RID: 3294
            [FieldOffset(0)]
            public NativeMethodXs.HARDWAREINPUT Hardware;

            // Token: 0x04000CDF RID: 3295
            [FieldOffset(0)]
            public NativeMethodXs.KEYBDINPUT Keyboard;

            // Token: 0x04000CE0 RID: 3296
            [FieldOffset(0)]
            public NativeMethodXs.MOUSEINPUT Mouse;
        }

        // Token: 0x02000280 RID: 640
        public struct HARDWAREINPUT
        {
            // Token: 0x04000CE1 RID: 3297
            public uint Msg;

            // Token: 0x04000CE2 RID: 3298
            public ushort ParamL;

            // Token: 0x04000CE3 RID: 3299
            public ushort ParamH;
        }

        // Token: 0x02000281 RID: 641
        public struct KEYBDINPUT
        {
            // Token: 0x04000CE4 RID: 3300
            public ushort Vk;

            // Token: 0x04000CE5 RID: 3301
            public ushort Scan;

            // Token: 0x04000CE6 RID: 3302
            public uint Flags;

            // Token: 0x04000CE7 RID: 3303
            public uint Time;

            // Token: 0x04000CE8 RID: 3304
            public IntPtr ExtraInfo;
        }

        #endregion
        #region API常量
        // Token: 0x0400086E RID: 2158
        public const int INPUT_KEYBOARD = 1;

        // Token: 0x0400086F RID: 2159
        public const int WH_KEYBOARD_LL = 13;

        // Token: 0x04000870 RID: 2160
        public const uint KEYEVENTF_EXTENDEDKEY = 1u;

        // Token: 0x04000871 RID: 2161
        public const uint KEYEVENTF_KEYUP = 2u;

        // Token: 0x04000872 RID: 2162
        public const uint KEYEVENTF_UNICODE = 4u;


        #endregion

        #region 键盘钩子
        // Token: 0x04000876 RID: 2166
        private static IntPtr processPointer = IntPtr.Zero;
        // Token: 0x04000877 RID: 2167
        private static List<Action<int, bool>> callbacks;
        // Token: 0x04000875 RID: 2165
        private static readonly HashSet<int> downKeys = new HashSet<int>();

        // Token: 0x02000282 RID: 642
        // (Invoke) Token: 0x060024F9 RID: 9465
        private delegate IntPtr KeyboardHookDelegate(int nCode, IntPtr wParam, IntPtr lParam);
        // Token: 0x04000874 RID: 2164
        private static readonly NativeMethodXs.KeyboardHookDelegate keyboardHookDelegate = new NativeMethodXs.KeyboardHookDelegate(NativeMethodXs.HookCallback);
        // Token: 0x060019DF RID: 6623 RVA: 0x0005D128 File Offset: 0x0005B328
        internal static void StartHook(Action<int, bool> callback)
        {
            if (NativeMethodXs.callbacks == null)
            {
                NativeMethodXs.callbacks = new List<Action<int, bool>>();
            }
            if (NativeMethodXs.callbacks.Contains(callback))
            {
                return;
            }
            NativeMethodXs.callbacks.Add(callback);
            NativeMethodXs.processPointer = NativeMethodXs.SetHook(NativeMethodXs.keyboardHookDelegate);
        }
        // Token: 0x060019E5 RID: 6629 RVA: 0x0005D170 File Offset: 0x0005B370
        private static IntPtr SetHook(NativeMethodXs.KeyboardHookDelegate hookDelegate)
        {
            IntPtr result;
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule mainModule = currentProcess.MainModule)
                {
                    result = NativeMethodXs.SetWindowsHookEx(13, hookDelegate, NativeMethodXs.GetModuleHandle(mainModule.ModuleName), 0u);
                }
            }
            return result;
        }
        // Token: 0x060019E6 RID: 6630 RVA: 0x0005D1D4 File Offset: 0x0005B3D4
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
                        if (NativeMethodXs.downKeys.Contains(num))
                        {
                            flag = true;
                        }
                        else
                        {
                            NativeMethodXs.downKeys.Add(num);
                        }
                        break;
                    case 257:
                    case 261:
                        arg = true;
                        NativeMethodXs.downKeys.Remove(num);
                        break;
                }
                if (!flag)
                {
                    foreach (Action<int, bool> action in NativeMethodXs.callbacks)
                    {
                        action(num, arg);
                    }
                }
            }
            return NativeMethodXs.CallNextHookEx(NativeMethodXs.processPointer, nCode, wParam, lParam);
        }

        // Token: 0x060019E0 RID: 6624 RVA: 0x0005D163 File Offset: 0x0005B363
        internal static void StopHook()
        {
            NativeMethodXs.UnhookWindowsHookEx(NativeMethodXs.processPointer);
        }

        #endregion

        #region 发送按键
        public static void Send(Keys virtualKey, bool iUnicode = false)
        {
            Send(null, (int)virtualKey, iUnicode);
        }
        public static void Send(int virtualKey, bool iUnicode = false)
        {
            Send(null, virtualKey, iUnicode);
        }
        // Token: 0x060019DA RID: 6618 RVA: 0x0005CDB4 File Offset: 0x0005AFB4
        public static void Send(ICollection<int> modifierKeys, int virtualKey, bool iUnicode = false)
        {
            if (virtualKey <= 0)
            {
                return;
            }
            if (modifierKeys == null || modifierKeys.Count == 0)
            {
                NativeMethodXs.INPUT input = NativeMethodXs.CreateKeyDownInput(virtualKey, iUnicode);
                NativeMethodXs.INPUT input2 = NativeMethodXs.CreateKeyUpInput(virtualKey, iUnicode);
                NativeMethodXs.INPUT[] inputs = new NativeMethodXs.INPUT[]
                {
                    input,
                    input2
                };
                if (NativeMethodXs.SendInput(2u, inputs, Marshal.SizeOf(typeof(NativeMethodXs.INPUT))) == 0u)
                {
                    throw new InvalidOperationException("Could not send key. VirtualKeyboardNativeMethodXs.SendInput function returned 0.");
                }
            }
            else
            {
                NativeMethodXs.INPUT[] array = new NativeMethodXs.INPUT[modifierKeys.Count * 2 + 2];
                int num = 0;
                foreach (int keyCode in modifierKeys)
                {
                    array[num++] = NativeMethodXs.CreateKeyDownInput(keyCode, iUnicode);
                }
                array[num++] = NativeMethodXs.CreateKeyDownInput(virtualKey, iUnicode);
                array[num++] = NativeMethodXs.CreateKeyUpInput(virtualKey, iUnicode);
                foreach (int keyCode2 in modifierKeys)
                {
                    array[num++] = NativeMethodXs.CreateKeyUpInput(keyCode2, iUnicode);
                }
                if (NativeMethodXs.SendInput((uint)array.Length, array, Marshal.SizeOf(typeof(NativeMethodXs.INPUT))) == 0u)
                {
                    throw new InvalidOperationException("Could not send key. VirtualKeyboardNativeMethodXs.SendInput function returned 0.");
                }
            }
        }

        /// <summary>
        /// Creates a key down <see cref="T:Telerik.Windows.Controls.VirtualKeyboard.NativeMethodXs.INPUT" /> object.
        /// </summary>
        /// <param name="keyCode">The virtual key code of the key.</param>
        /// <returns>The <see cref="T:Telerik.Windows.Controls.VirtualKeyboard.NativeMethodXs.INPUT" /> instance.</returns>
        // Token: 0x060019E9 RID: 6633 RVA: 0x0005D458 File Offset: 0x0005B658
        private static NativeMethodXs.INPUT CreateKeyDownInput(int keyCode, bool iUnicode = false)
        {
            NativeMethodXs.INPUT result = default(NativeMethodXs.INPUT);
            result.Type = 1u;
            result.Data.Keyboard = new NativeMethodXs.KEYBDINPUT
            {
                Vk = iUnicode ? (ushort)0 : (ushort)keyCode,
                Scan = iUnicode ? (ushort)keyCode : (ushort)0,
                Flags = (iUnicode ? KEYEVENTF_UNICODE : 0) + (NativeMethodXs.IsExtendedKey(keyCode) ? KEYEVENTF_EXTENDEDKEY : 0),
                Time = 0u,
                ExtraInfo = IntPtr.Zero
            };
            return result;
        }
        /// <summary>
        /// Creates a key up <see cref="T:Telerik.Windows.Controls.VirtualKeyboard.NativeMethodXs.INPUT" /> object.
        /// </summary>
        /// <param name="keyCode">The virtual key code of the key.</param>
        /// <returns>The <see cref="T:Telerik.Windows.Controls.VirtualKeyboard.NativeMethodXs.INPUT" /> instance.</returns>
        // Token: 0x060019EA RID: 6634 RVA: 0x0005D4C4 File Offset: 0x0005B6C4
        private static NativeMethodXs.INPUT CreateKeyUpInput(int keyCode, bool iUnicode = false)
        {
            NativeMethodXs.INPUT result = default(NativeMethodXs.INPUT);
            result.Type = 1u;
            result.Data.Keyboard = new NativeMethodXs.KEYBDINPUT
            {
                Vk = iUnicode ? (ushort)0 : (ushort)keyCode,
                Scan = iUnicode ? (ushort)keyCode : (ushort)0,
                Flags = (iUnicode ? KEYEVENTF_UNICODE : 0) + KEYEVENTF_KEYUP + (NativeMethodXs.IsExtendedKey(keyCode) ? KEYEVENTF_EXTENDEDKEY : 0),
                Time = 0u,
                ExtraInfo = IntPtr.Zero
            };
            return result;
        }
        /// <summary>
        /// The extended-key flag indicates whether the keystroke message originated from one of the 
        /// additional keys on the enhanced keyboard.
        /// The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; 
        /// the INS, DEL, HOME, END, PAGE UP, PAGE DOWN, and arrow keys in the clusters to the left 
        /// of the numeric keypad; the NUM LOCK key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/).
        /// </summary>
        /// <param name="virtualKeyCode"></param>
        /// <returns></returns>
        // Token: 0x060019EB RID: 6635 RVA: 0x0005D530 File Offset: 0x0005B730
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
