using System;
using LightCheat.Data;
using LightCheat.Gfx;

namespace LightCheat
{
    public class Program :
        System.Windows.Application,
        IDisposable
    {
        #region // Entry Point

        // Start program
        [STAThread]
        public static void Main() => new Program().Run();

        #endregion

        #region // Storage

        private GameProcess GameProcess { get; set; }
        private GameData GameData { get; set; }
        private WindowOverlay WindowOverlay { get; set; }
        private Graphics Graphics { get; set; }

        #endregion

        #region // Contructor

        public Program()
        {
            // Startup and close events
            Startup += (sender, args) => Ctor();
            Exit += (sender, args) => Dispose();
        }

        // Startup Function
        public void Ctor()
        {
            GameProcess = new GameProcess();
            GameData = new GameData(GameProcess);
            WindowOverlay = new WindowOverlay(GameProcess);
            Graphics = new Graphics(WindowOverlay, GameProcess, GameData);

            GameProcess.Start();
            GameData.Start();
            WindowOverlay.Start();
            Graphics.Start();
        }

        // Close Function
        public void Dispose()
        {
            Graphics.Dispose();
            Graphics = default;

            WindowOverlay.Dispose();
            WindowOverlay = default;

            GameData.Dispose();
            GameData = default;

            GameProcess.Dispose();
            GameProcess = default;
        }

        #endregion
    }
}
