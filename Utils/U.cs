using System;
using System.Linq;
using System.Runtime.InteropServices;
using LightCheat.Sys;

namespace LightCheat.Utils
{
    /*
     * Helper class
     */
    public static class U
    {
        #region // Degrees <-> Radians

        private const double _PI_Over_180 = Math.PI / 180.0;
        private const double _180_Over_PI = 180.0 / Math.PI;

        public static double DegreeToRadian(this double degree)
        {
            return degree * _PI_Over_180;
        }

        public static double RadianToDegree(this double radian)
        {
            return radian * _180_Over_PI;
        }

        public static float DegreeToRadian(this float degree)
        {
            return (float)(degree * _PI_Over_180);
        }

        public static float RadianToDegree(this float radian)
        {
            return (float)(radian * _180_Over_PI);
        }

        #endregion


        // Get window client rectangle
        public static System.Drawing.Rectangle GetClientRectangle(IntPtr handle)
        {
            return User32.ClientToScreen(handle, out var point) && User32.GetClientRect(handle, out var rect)
                ? new System.Drawing.Rectangle(point.X, point.Y, rect.Right - rect.Left, rect.Bottom - rect.Top)
                : default;
        }

        // Get module by name
        public static Module GetModule(this System.Diagnostics.Process process, string moduleName)
        {
            var processModule = process.GetProcessModule(moduleName);
            return processModule is null || processModule.BaseAddress == IntPtr.Zero
                ? default
                : new Module(process, processModule);
        }

        // Get process module by name
        public static System.Diagnostics.ProcessModule GetProcessModule(this System.Diagnostics.Process process, string moduleName)
        {
            return process?.Modules.OfType<System.Diagnostics.ProcessModule>()
                .FirstOrDefault(a => string.Equals(a.ModuleName.ToLower(), moduleName.ToLower()));
        }

        // Check if value is infinity or NaN
        public static bool IsInfinityOrNaN(this float value)
        {
            return float.IsNaN(value) || float.IsInfinity(value);
        }

        // Get if process is running
        public static bool IsRunning(this System.Diagnostics.Process process)
        {
            try
            {
                System.Diagnostics.Process.GetProcessById(process.Id);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        // Check if vector is valid to draw in screen space
        public static bool IsValidScreen(this Microsoft.DirectX.Vector3 value)
        {
            return !value.X.IsInfinityOrNaN() && !value.Y.IsInfinityOrNaN() && value.Z >= 0 && value.Z < 1;
        }

        // Read process memory
        public static T Read<T>(this System.Diagnostics.Process process, IntPtr lpBaseAddress)
            where T : unmanaged
        {
            return Read<T>(process.Handle, lpBaseAddress);
        }

        // Read process memory from module
        public static T Read<T>(this Module module, int offset)
            where T : unmanaged
        {
            return Read<T>(module.Process.Handle, module.ProcessModule.BaseAddress + offset);
        }

        // Read process memory
        public static T Read<T>(IntPtr hProcess, IntPtr lpBaseAddress)
            where T : unmanaged
        {
            var size = Marshal.SizeOf<T>();
            var buffer = (object)default(T);
            Kernel32.ReadProcessMemory(hProcess, lpBaseAddress, buffer, size, out var lpNumberOfBytesRead);
            return lpNumberOfBytesRead == size ? (T)buffer : default;
        }

        // Convert to matrix 4x4
        public static Microsoft.DirectX.Matrix ToMatrix(this in Data.Raw.matrix3x4_t matrix)
        {
            return new Microsoft.DirectX.Matrix
            {
                M11 = matrix.m00,
                M12 = matrix.m01,
                M13 = matrix.m02,

                M21 = matrix.m10,
                M22 = matrix.m11,
                M23 = matrix.m12,

                M31 = matrix.m20,
                M32 = matrix.m21,
                M33 = matrix.m22,

                M41 = matrix.m30,
                M42 = matrix.m31,
                M43 = matrix.m32,
                M44 = 1,
            };
        }

        // Convert value to team
        public static Data.Raw.Team ToTeam(this int teamNum)
        {
            switch (teamNum)
            {
                case 1:
                    return Data.Raw.Team.Spectator;
                case 2:
                    return Data.Raw.Team.Terrorists;
                case 3:
                    return Data.Raw.Team.CounterTerrorists;
                default:
                    return Data.Raw.Team.Unknown;
            }
        }
    }
}