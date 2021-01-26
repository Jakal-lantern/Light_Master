using System;
using System.Runtime.InteropServices;
using LightCheat.Sys.Data;

namespace LightCheat.Sys
{
    public static class User32
    {
        // Sets a new extended window style
        public const int GWL_EXSTYLE = -20;

        // Use bAlpha to determine the opacity of the layered window
        public const int LWA_ALPHA = 0x2;

        // The window is a layered window
        // This style cannot be used if the window has a class style of either CS_OWNDC or cs_CLASSDC
        // Windows 8: The WS_EX_LAYERED style is supported for top-level windows and child windows
        // Previous Windows versions support WS_EX_LAYERED only for top-level windows
        public const int WS_EX_LAYERED = 0x80000;

        // The window should not be painted until siblins beneath the window (that were created by the sme thread) have been painted
        // The window appears transparent because the bits of underlying sibling windows have already been painted
        // To achieve transparency without these restrictions, use the SetWindowRgn function
        public const int WS_EX_TRANSPARENT = 0x20;

        // The ClientToScreen function converts the client-are coordinates of a specified point to screen coordinates
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ClientToScreen(IntPtr hWnd, out Point lpPoint);

        // Retrieves the coordinates of a window's client area
        // The client coordinates specify the upper-left and lower-right corners of the client area
        // Because client coordinates are relative to the upper-left corner of a window's client are, the coordinates of the upper-left corner are (0,0)
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, out Rect lpRect);

        // Retrieves a handle to the foreground window (the window with which the user is currently working)
        // The system assigns a slightly higher priority to thre thread that creates the foreground window than it does to other threads
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        // Retrieves information about the specified window
        // The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    }
}