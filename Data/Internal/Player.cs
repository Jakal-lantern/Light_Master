using System;
using Microsoft.DirectX;
using LightCheat.Gfx;
using LightCheat.Utils;

namespace LightCheat.Data.Internal
{
    /*
     * Player data
     */
    public class Player :
        EntityBase
    {
        #region // Storage

        // Matrix from world space to clipping space
        public Matrix MatrixViewProjection { get; private set; }

        // Matrix from clipping space to screen space
        public Matrix MatrixViewport { get; private set; }

        // Matrix from world space to screen space
        public Matrix MatrixViewProjectionViewport { get; private set; }

        // Local offset from model origin to player eyes
        public Vector3 ViewOffset { get; private set; }

        // Eye position (in world)
        public Vector3 EyePosition { get; private set; }

        // View angles (in degrees)
        public Vector3 ViewAngles { get; private set; }

        // Aim punch angles (in degrees)
        public Vector3 AimPunchAngle { get; private set; }

        // Aim direction (in world)
        public Vector3 AimDirection { get; private set; }

        // Player vertical field of view (in degrees)
        public int Fov { get; private set; }

        #endregion

        #region // Routines

        protected override IntPtr ReadAddressBase(GameProcess gameProcess)
        {
            return gameProcess.ModuleClient.Read<IntPtr>(Offsets.dwLocalPlayer);
        }

        public override bool Update(GameProcess gameProcess)
        {
            if (!base.Update(gameProcess))
            {
                return false;
            }

            // Get matrices
            MatrixViewProjection = Matrix.TransposeMatrix(gameProcess.ModuleClient.Read<Matrix>(Offsets.dwViewMatrix));
            MatrixViewport = GfxMath.GetMatrixViewport(gameProcess.WindowRectangleClient.Size);
            MatrixViewProjectionViewport = MatrixViewProjection * MatrixViewport;

            // Read data
            ViewOffset = gameProcess.Process.Read<Vector3>(AddressBase + Offsets.m_vecViewOffset);
            EyePosition = Origin + ViewOffset;
            ViewAngles = gameProcess.Process.Read<Vector3>(gameProcess.ModuleClient.Read<IntPtr>(Offsets.dwClientState) + Offsets.dwClientState_ViewAngles);
            AimPunchAngle = gameProcess.Process.Read<Vector3>(AddressBase + Offsets.m_aimPunchAngle);
            Fov = gameProcess.Process.Read<int>(AddressBase + Offsets.m_iFOV);
            if (Fov == 0) Fov = 90; // Correct for default

            // calc data
            AimDirection = GetAimDirection(ViewAngles, AimPunchAngle);

            return true;
        }

        // Get aim direction
        private static Vector3 GetAimDirection(Vector3 viewAngles, Vector3 aimPunchAngle)
        {
            var phi = (viewAngles.X + aimPunchAngle.X * Offsets.weapon_recoil_scale).DegreeToRadian();
            var theta = (viewAngles.Y + aimPunchAngle.Y * Offsets.weapon_recoil_scale).DegreeToRadian();

            // https://en.wikipedia.org/wiki/Spherical_coordinate_system
            return Vector3.Normalize(new Vector3
            (
                (float)(Math.Cos(phi) * Math.Cos(theta)),
                (float)(Math.Cos(phi) * Math.Sin(theta)),
                (float)-Math.Sin(phi)
            ));
        }

        #endregion
    }
}