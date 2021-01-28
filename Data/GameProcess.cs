using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using LightCheat.Sys;
using LightCheat.Utils;

namespace LightCheat.Data
{
    // Game process component
    public class GameProcess :
        ThreadedComponent
    {
        #region // Static

        private const string NAME_PROCESS = "csgo";

        private const string NAME_MODULE_CLIENT = "client_panorama.dll";

        private const string NAME_MODULE_ENGINE = "engine.dll";

        private const string NAME_WINDOW = "Counter-Strike: Global Offensive";

        #endregion

        #region // Storage

        // <inheritdoc />
        protected override string ThreadName => nameof(GameProcess);

        // <inhereitdoc />
        protected override TimeSpan ThreadFrameSleep { get; set; } = new TimeSpan(0, 0, 0, 0, 500);

        // Game process
        public Process Process { get; private set; }

        // Client module
        public Module ModuleClient { get; private set; }

        // Engine module
        public Module ModuleEngine { get; private set; }

        // Game window handle
        private IntPtr WindowHwnd { get; set; }
        
        // Game window client rectangle
        public Rectangle WindowRectangleClient { get; private set; }

        // Whether game window is active
        private bool WindowActive { get; set; }

        // Is game proces valid
        public bool IsValid => WindowActive && !(Process is null) && !(ModuleClient is null) && !(ModuleEngine is null);

        #endregion

        #region // Constructor
        // <inheritdoc />
        public override void Dispose()
        {
            InvalidateWindow();
            InvalidateModules();

            base.Dispose();
        }

        #endregion

        #region // Routines

        // <inheritdoc />
        protected override void FrameAction()
        {
            if (!EnsureProcessAndModules())
            {
                InvalidateModules();
            }

            if (!EnsureWindow())
            {
                InvalidateWindow();
            }

            // Console debug
            //Console.WriteLine(IsValid
            //    ? $"0x{(int)Process.Handle:X8} {WindowRectangleClient.X} {WindowRectangleClient.Y} {WindowRectangleClient.Width} {WindowRectangleClient.Height}"
            //    : "Game process invalid");
        }

        // Invalidate all game modules
        private void InvalidateModules()
        {
            ModuleEngine?.Dispose();
            ModuleEngine = default;

            ModuleClient?.Dispose();
            ModuleClient = default;

            Process?.Dispose();
            Process = default;
        }

        // Invalidate game window
        private void InvalidateWindow()
        {
            WindowHwnd = IntPtr.Zero;
            WindowRectangleClient = Rectangle.Empty;
            WindowActive = false;
        }

        // Ensure game process and modules
        private bool EnsureProcessAndModules()
        {
            if (Process is null)
            {
                Process = Process.GetProcessesByName(NAME_PROCESS).FirstOrDefault();
            }
            if (Process is null || !Process.IsRunning())
            {
                return false;
            }

            if (ModuleClient is null)
            {
                ModuleClient = Process.GetModule(NAME_MODULE_CLIENT);
            }
            if (ModuleClient is null)
            {
                return false;
            }

            if (ModuleEngine is null)
            {
                ModuleEngine = Process.GetModule(NAME_MODULE_ENGINE);
            }
            if (ModuleEngine is null)
            {
                return false;
            }

            return true;
        }

        // Ensure game window
        private bool EnsureWindow()
        {
            WindowHwnd = User32.FindWindow(null, NAME_WINDOW);
            if (WindowHwnd == IntPtr.Zero)
            {
                return false;
            }

            WindowRectangleClient = U.GetClientRectangle(WindowHwnd);
            if (WindowRectangleClient.Width <= 0 || WindowRectangleClient.Height <= 0)
            {
                return false;
            }

            WindowActive = WindowHwnd == User32.GetForegroundWindow();

            return WindowActive;
        }

        #endregion
    }
}