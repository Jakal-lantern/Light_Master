using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;
using LightCheat.Data;
using LightCheat.Sys;
using LightCheat.Sys.Data;
using LightCheat.Utils;
using Point = System.Drawing.Point;

namespace LightCheat.Gfx
{
    // Overlay window for graphics
    public class WindowOverlay :
        ThreadedComponent
    {
        #region // Storage

        protected override string ThreadName => nameof(WindowOverlay);

        protected override TimeSpan ThreadFrameSleep { get; set; } = new TimeSpan(0, 0, 0, 0, 500);

        private GameProcess GameProcess { get; set; }

        // Physical overlay window
        public Form Window { get; private set; }

        #endregion

        #region // Constructor

        public WindowOverlay(GameProcess gameProcess)
        {
            GameProcess = gameProcess;

            // Create window
            Window = new Form
            {
                Name = "Overlay Window",
                Text = "Overlay Window",
                MinimizeBox = false,
                MaximizeBox = false,
                FormBorderStyle = FormBorderStyle.None,
                TopMost = true,
                Width = 16,
                Height = 16,
                Left = -32000,
                Top = -32000,
                StartPosition = FormStartPosition.Manual,
            };

            Window.Load += (sender, args) =>
            {
                var exStyle = User32.GetWindowLong(Window.Handle, User32.GWL_EXSTYLE);
                exStyle |= User32.WS_EX_LAYERED;
                exStyle |= User32.WS_EX_TRANSPARENT;

                // Make the window border completely transparent
                User32.SetWindowLong(Window.Handle, User32.GWL_EXSTYLE, (IntPtr)exStyle);

                // Set the alpha on the whole window to 255 (solid)
                User32.SetLayeredWindowAttributes(Window.Handle, 0, 255, User32.LWA_ALPHA);
            };
            Window.SizeChanged += (sender, args) => ExtendFrameIntoClientArea();
            Window.LocationChanged += (sender, args) => ExtendFrameIntoClientArea();
            Window.Closed += (sender, args) => System.Windows.Application.Current.Shutdown();

            // Show window
            Window.Show();
        }

        public override void Dispose()
        {
            base.Dispose();

            Window.Close();
            Window.Dispose();
            Window = default;

            GameProcess = default;
        }

        #endregion

        #region // Routines

        // Extend the window frame into the client area
        private void ExtendFrameIntoClientArea()
        {
            var margins = new Margins
            {
                Left = -1,
                Right = -1,
                Top = -1,
                Bottom = -1
            };
            Dwmpai.DwmExtendFrameIntoClientArea(Window.Handle, ref margins);
        }

        protected override void FrameAction()
        {
            Update(GameProcess.WindowRectangleClient);
        }

        // Update position and size
        private void Update(Rectangle windowRectangleClient)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Window.BackColor = Color.Blue;  // TODO: Temporary

                if (Window.Location != windowRectangleClient.Location || Window.Size != windowRectangleClient.Size)
                {
                    if (windowRectangleClient.Width > 0 && windowRectangleClient.Height > 0)
                    {
                        // Valid
                        Window.Location = windowRectangleClient.Location;
                        Window.Size = windowRectangleClient.Size;
                    }
                    else
                    {
                        // Invalid
                        Window.Location = new Point(-32000, -32000);
                        Window.Size = new Size(16, 16);
                    }
                }
            }, DispatcherPriority.Normal);
        }

        #endregion
    }
}