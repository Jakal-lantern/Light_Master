using System;
using Microsoft.DirectX;
using LightCheat.Data.Raw;
using LightCheat.Utils;

namespace LightCheat.Data.Internal
{
    /*
     * Base class for entities
     */
    public abstract class EntityBase
    {
        #region // Storage

        // Address base of entity
        public IntPtr AddressBase { get; protected set; }

        // Life state (true = dead, false = alive)
        public bool LifeState { get; protected set; }

        // Health points
        public int Health { get; protected set; }

        public Team Team { get; protected set; }

        // Model origin (in world)
        public Vector3 Origin { get; private set; }

        #endregion

        #region // Routines

        // Is entity alive
        public virtual bool IsAlive()
        {
            return AddressBase != IntPtr.Zero &&
                !LifeState &&
                Health > 0 &&
                (Team == Team.Terrorists || Team == Team.CounterTerrorists);
        }

        // Read Address base
        protected abstract IntPtr ReadAddressBase(GameProcess gameProcess);

        // Update data from process
        public virtual bool Update(GameProcess gameProcess)
        {
            AddressBase = ReadAddressBase(gameProcess);
            if (AddressBase == IntPtr.Zero)
            {
                return false;
            }

            LifeState = gameProcess.Process.Read<bool>(AddressBase + Offsets.m_lifeState);
            Health = gameProcess.Process.Read<int>(AddressBase + Offsets.m_iHealth);
            Team = gameProcess.Process.Read<int>(AddressBase + Offsets.m_iTeamNum).ToTeam();
            Origin = gameProcess.Process.Read<Vector3>(AddressBase + Offsets.m_vecOrigin);

            return true;
        }

        #endregion
    }
}
