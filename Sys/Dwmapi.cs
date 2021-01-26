using System;
using System.Runtime.InteropServices;
using LightCheat.Sys.Data;

namespace LightCheat.Sys
{
    public static class Dwmpai
    {
        // Extends the window frame into the client area
        [DllImport("dwmapi.dll"), SetLastError = true]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMargins);
    }
}