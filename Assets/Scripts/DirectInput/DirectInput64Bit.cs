using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

/*

From these sources:

https://stackoverflow.com/questions/8962850/sendinput-fails-on-64bit
https://stackoverflow.com/questions/6830651/sendinput-and-64bits
http://www.pinvoke.net/default.aspx/user32.sendinput
https://www.developerfusion.com/article/84519/mastering-structs-in-c/

*/

namespace DirectInput {
    // https://gist.github.com/tracend/912308
    public enum ScanCode : UInt16
    {
        One = 0x02,
        Two = 0x03,
        Three = 0x04,
        Four = 0x05,
        A = 0x1e,
        D = 0x20,
        E = 0x12,
        H = 0x23,
        J = 0x24,
        Q = 0x10,
        R = 0x13,
        S = 0x1f,
        W = 0x11,
        Space = 0x39,
        LControl = 0x1d,
    }

    public static class Input {
        public static void SendKey(ScanCode scancode, bool up)
        {
            INPUT[] inputs = new INPUT[1];

            //UInt16 vkcode = (UInt16)0x57; // Do we still need this?

            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].u.ki.wVk = 0;
            inputs[0].u.ki.wScan = (UInt16)scancode;
            inputs[0].u.ki.dwFlags = KEYEVENTF_SCANCODE | (up ? KEYEVENTF_KEYUP : 0);
            inputs[0].u.ki.time = 0;
            inputs[0].u.ki.dwExtraInfo = IntPtr.Zero;

            // inputs[1].type = INPUTTYPE_KEYBOARD;
            // inputs[1].ki.wScan = scancode;
            // inputs[1].ki.dwFlags = KEYEVENTF_KEYUP | KEYEVENTF_SCANCODE;
            // inputs[1].ki.time = 0;
            // inputs[1].ki.dwExtraInfo = IntPtr.Zero;

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        const int INPUT_MOUSE = 0;
        const int INPUT_KEYBOARD = 1;
        const int INPUT_HARDWARE = 2;
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_UNICODE = 0x0004;
        const uint KEYEVENTF_SCANCODE = 0x0008;

        struct INPUT
        {
            public int type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            /*Virtual Key code.  Must be from 1-254.  If the dwFlags member specifies KEYEVENTF_UNICODE, wVk must be 0.*/
            public ushort wVk;
            /*A hardware scan code for the key. If dwFlags specifies KEYEVENTF_UNICODE, wScan specifies a Unicode character which is to be sent to the foreground application.*/
            public ushort wScan;
            /*Specifies various aspects of a keystroke.  See the KEYEVENTF_ constants for more information.*/
            public uint dwFlags;
            /*The time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time stamp.*/
            public uint time;
            /*An additional value associated with the keystroke. Use the GetMessageExtraInfo function to obtain this information.*/
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
    }
}
