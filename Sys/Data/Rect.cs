using System.Runtime.InteropServices;

namespace LightCheat.Sys.Data
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left, Top, Right, Bottom;
    }
}