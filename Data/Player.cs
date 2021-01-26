using System;
using LightCheat.Math;
using LightCheat.Utils;

namespace LightCheat.Data
{
    // Player class
    public class Player
    {
        // Update data from process
        public void Update(GameProcess gameProcess)
        {
            // Access player data pointer
            var addressBase = gameProcess.ModuleClient.Read<IntPtr>(Offsets.dwLocalPlayer);
            
            // Return if value is corrupted
            if (addressBase == IntPtr.Zero)
            {
                return;
            }

            var origin = gameProcess.Process.Read<Vector3>(addressBase + Offsets.m_vecOrigin);
            var viewOffset = gameProcess.Process.Read<Vector3>(addressBase + Offsets.m_vecViewOffset);
            var eyePosition = origin + viewOffset;

            // Console debug
            //Console.WriteLine($"{eyePosition.X:0.00} {eyePosition.Y:0.00} {eyePosition.Z:0.00}");
        }
    }
}