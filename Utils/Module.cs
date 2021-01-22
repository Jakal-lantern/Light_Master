using System;
using System.Diagnostics;

namespace LightCheat.Utils
{
    // Wrapper for <see cref="ProcessModule"/>
    public class Module :
        IDisposable
    {
        #region // Storage
        
        // <inheritdoc cref="Process"/>
        public Process Process { get; private set; }

        // <inheritdoc cref="ProcessModule"/>
        public ProcessModule ProcessModule { get; private set; }

        #endregion

        #region // Constructor

        public Module(Process process, ProcessModule processModule)
        {
            Process = process;
            ProcessModule = processModule;
        }

        public void Dispose()
        {
            Process.Dispose();
            Process = default;

            ProcessModule.Dispose();
            ProcessModule = default;
        }

        #endregion
    }
}