using System;
using System.Linq;
using LightCheat.Sys;

namespace LightCheat.Utils
{
    // Helper class
    public static class U
    {
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
    }
}