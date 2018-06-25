using System;
using System.Runtime.InteropServices;

namespace DirectInputLib32BitA
{
    public static class Input
    {
        [DllImport("user32.dll")]
        static extern UInt32 SendInput(UInt32 nInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] pInputs, Int32 cbSize);

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public Int32 dx;
            public Int32 dy;
            public UInt32 mouseData;
            public UInt32 dwFlags;
            public UInt32 time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public UInt16 wVk;
            public UInt16 wScan;
            public UInt32 dwFlags;
            public UInt32 time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public UInt32 uMsg;
            public UInt16 wParamL;
            public UInt16 wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct INPUT
        {
            [FieldOffset(0)]
            public UInt32 type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }

        const UInt32 INPUTTYPE_MOUSE = 0;
        const UInt32 INPUTTYPE_KEYBOARD = 1;
        const UInt32 INPUTTYPE_HARDWARE = 2;

        const UInt32 KEYEVENTF_EXTENDEDKEY = (UInt32)0x0001;
        const UInt32 KEYEVENTF_KEYUP = (UInt32)0x0002;
        const UInt32 KEYEVENTF_UNICODE = (UInt32)0x0004;
        const UInt32 KEYEVENTF_SCANCODE = (UInt32)0x0008;


        public static void SendKey(short Keycode)
        {
            INPUT[] inputs = new INPUT[1];

            UInt16 vkcode = (UInt16)0x57;
            UInt16 scancode = (UInt16)0x11;

            inputs[0].type = INPUTTYPE_KEYBOARD;
            inputs[0].ki.wVk = vkcode;
            inputs[0].ki.wScan = scancode;
            inputs[0].ki.dwFlags = 0;
            inputs[0].ki.time = 0;
            inputs[0].ki.dwExtraInfo = IntPtr.Zero;

            // inputs[1].type = INPUTTYPE_KEYBOARD;
            // inputs[1].ki.wScan = scancode;
            // inputs[1].ki.dwFlags = KEYEVENTF_KEYUP | KEYEVENTF_SCANCODE;
            // inputs[1].ki.time = 0;
            // inputs[1].ki.dwExtraInfo = IntPtr.Zero;

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
    }
}

// This just about managed to press backspace, but is otherwise still broken

// using System;
// using System.Runtime.InteropServices;

// public static class DirectInput
// {
//     [DllImport("user32.dll")]
//     static extern UInt32 SendInput(UInt32 nInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] pInputs, Int32 cbSize);

//     [StructLayout(LayoutKind.Sequential)]
//     struct MOUSEINPUT
//     {
//         public int dx;
//         public int dy;
//         public int mouseData;
//         public int dwFlags;
//         public int time;
//         public IntPtr dwExtraInfo;
//     }

//     [StructLayout(LayoutKind.Sequential)]
//     struct KEYBDINPUT
//     {
//         public short wVk;
//         public short wScan;
//         public int dwFlags;
//         public int time;
//         public IntPtr dwExtraInfo;
//     }

//     [StructLayout(LayoutKind.Sequential)]
//     struct HARDWAREINPUT
//     {
//         public int uMsg;
//         public short wParamL;
//         public short wParamH;
//     }

//     [StructLayout(LayoutKind.Explicit)]
//     struct INPUT
//     {
//         [FieldOffset(0)]
//         public int type;
//         [FieldOffset(4)]
//         public MOUSEINPUT mi;
//         [FieldOffset(4)]
//         public KEYBDINPUT ki;
//         [FieldOffset(4)]
//         public HARDWAREINPUT hi;
//     }

//     const int INPUTTYPE_MOUSE = 0;
//     const int INPUTTYPE_KEYBOARD = 1;
//     const int INPUTTYPE_HARDWARE = 2;

//     const int KEYEVENTF_EXTENDEDKEY = 0x0001;
//     const int KEYEVENTF_KEYUP = 0x0002;
//     const int KEYEVENTF_UNICODE = 0x0004;
//     const int KEYEVENTF_SCANCODE = 0x0008;


//     public static void SendKey(short Keycode, int KeyUporDown)
//     {
//         INPUT[] inputs = new INPUT[2];

//         inputs[0].type = INPUTTYPE_KEYBOARD;
//         inputs[0].ki.wScan = Keycode;
//         inputs[0].ki.dwFlags = KEYEVENTF_SCANCODE;
//         inputs[0].ki.time = 0;
//         inputs[0].ki.dwExtraInfo = IntPtr.Zero;

//         inputs[1].type = INPUTTYPE_KEYBOARD;
//         inputs[1].ki.wScan = Keycode;
//         inputs[1].ki.dwFlags = KEYEVENTF_KEYUP | KEYEVENTF_SCANCODE;
//         inputs[1].ki.time = 0;
//         inputs[1].ki.dwExtraInfo = IntPtr.Zero;

//         SendInput((UInt32)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
//     }

// }