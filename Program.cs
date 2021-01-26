using System;

public class LightCheat
{
	public class Program :
		System.Windows.Application,
		IDisposable
    {
        #region // Entry Point

        // Start program
        public static void Main() => new Program().Run();

        #endregion

        #region // Storage
        private GameProcess GameProcess { get; set; }
        private GameData GameData { get; set; }

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

            GameProcess.Start();
            GameData.Start();
        }

        // Close Function
        public void Dispose()
        {
            GameData.Dispose();
            GameData = default;

            GameProcess.Dispose();
            GameProcess = default;
        }

        #endregion
    }
}
