using System;
using System.Diagnostics;

namespace LightCheat.Utils
{
    /*
     * Class for measuring fps
     */
    public class FpsCounter
    {
        #region // Storage

        // One second timespan
        private static readonly TimeSpan TimeSpanFpsUpdate = new TimeSpan(0, 0, 0, 1);

        // Stopwatch measuring time
        private Stopwatch FpsTimer { get; } = Stopwatch.StartNew();

        // Frame count since last timer restart
        private int FpsFrameCount { get; set; }

        // Average fps
        public double Fps { get; private set; }

        #endregion

        #region // Routines

        // Trigger frame update
        public void Update()
        {
            var fpsTimerElasped = FpsTimer.Elapsed;
            if (fpsTimerElasped > TimeSpanFpsUpdate)
            {
                Fps = FpsFrameCount / fpsTimerElasped.TotalSeconds;
                FpsTimer.Restart();
                FpsFrameCount = 0;
            }
            FpsFrameCount++;
        }

        #endregion
    }
}
