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
        }

        // Close Function
        public void Dispose()
        {
        }

        #endregion
    }
}
