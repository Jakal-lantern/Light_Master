using System;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using LightCheat.Data.Raw;
using LightCheat.Utils;

namespace LightCheat.Data.Internal
{
    /*
     * Entity data
     */
    public class Entity :
        EntityBase
    {
        #region // Storage
        
        // Index in entity list
        public int Index { get; }

        // Dormant state
        public bool Dormant { get; private set; } = true;

        // Pointer to studio hrd
        private IntPtr AddressStudioHdr { get; set; }

        public studiohdr_t StudioHdr { get; private set; }
        
        public mstudiohitboxset_t StudioHitBoxSet { get; private set; }

        public mstudiobox_t[] StudioHitBoxes { get; }

        public mstudiobone_t[] StudioBones { get; }

        // Bone model to world matrices
        public Matrix BoneMatrices { get; }

        // Bone positions in world
        public Vector3[] BonesPos { get; }

        // Skeleton bones
        public (int from, int to)[] Skeleton { get; }

        public int SkeletonCount { get; private set; }

        #endregion

        #region // Constructor
        
        public Entity(int index)
        {
            Index = index;
            StudioHitBoxes = new mstudiobox_t[Offsets.MAXSTUDIOBONES];
            StudioBones = new mstudiobone_t[Offsets.MAXSTUDIOBONES];
            BoneMatrices = new Matrix[Offsets.MAXSTUDIOBONES];
            BonesPos = new Vector3[Offsets.MAXSTUDIOBONES];
            Skeleton = new (int, int)[Offsets.MAXSTUDIOBONES];
        }

        #endregion

        #region // Routines

        public override bool IsAlive()
        {
            return base.IsAlive() && !Dormant;
        }

        protected override IntPtr ReadAddressBase(GameProcess gameProcess)
        {
            return gameProcess.ModuleClient.Read<IntPtr>(Offsets.dwEntityList + Index * 0x10 /* size */);
        }

        public override bool Update(GameProcess gameProcess)
        {
            if (!base.Update(gameProcess))
            {
                return false;
            }

            Dormant = gameProcess.Process.Read<bool>(AddressBase + Offsets.m_bDormant);
            if (!IsAlive())
            {
                return true;
            }

            UpdateStudioHdr(gameProcess);
            UpdateStudioHitBoxes(gameProcess);
            UpdateStudioBones(gameProcess);
            UpdateBonesMatricesAndPos(gameProcess);
            UpdateSkeleton();

            return true;
        }

        // Update 'AddressStudioHdr' and 'StudioHdr'
        private void UpdateStudioHdr(GameProcess gameProcess)
        {
            var addressToAddressStudioHdr = gameProcess.Process.Read<IntPtr>(AddressBase + Offsets.m_pStudioHdr);
            AddressStudioHdr = gameProcess.Process.Read<IntPtr>(addressToAddressStudioHdr); // deref
            StudioHdr = gameProcess.Process.Read<studiohdr_t>(AddressStudioHdr);
        }

        // Update 'StudioHitBoxSet' and 'StudioHitBoxes'
        private void UpdateStudioHitBoxes(GameProcess gameProcess)
        {
            var addressHitBoxSet = AddressStudioHdr + StudioHdr.hitboxsetindex;
            StudioHitBoxSet = gameProcess.Process.Read<mstudiohitboxset_t>(addressHitBoxSet);

            // Read
            for (var i = 0; i < StudioHitBoxSet.numhitboxes; i++)
            {
                StudioHitBoxes[i] = gameProcess.Process.Read<mstudiobbox_t>(addressHitBoxSet + StudioHitBoxSet.hitboxindex + i * Marshal.SizeOf<mstudiobbox_t>());
            }
        }

        // Update 'StudioBones'
        private void UpdateStudioBones(GameProcess gameProcess)
        {
            for (var i = 0; i < StudioHdr.numbones; i++)
            {
                StudioBones[i] = gameProcess.Process.Read<mstudiobone_t>(AddressStudioHdr + StudioHdr.boneindex + i * Marshal.SizeOf<mstudiobone_t>());
            }
        }

        // Update 'StudioHdr'
        private void UpdateBonesMatricesAndPos(GameProcess gameProcess)
        {
            var addressBoneMatrix = gameProcess.Process.Read<IntPtr>(AddressBase + Offsets.m_dwBoneMatrix);
            for (var boneId = 0; boneId < BonesPos.Length; boneId++)
            {
                var matrix = gameProcess.Process.Read<matrix3x4_t>(addressBoneMatrix + boneId * Marshal.SizeOf<matrix3x4_t>());
                BoneMatrices[boneId] = matrix.ToMatrix();
                BonesPos[boneId] = new Vector3(matrix.m30, matrix.m31, matrix.m32);
            }
        }

        // Update 'StudioHdr'
        private void UpdateSkeleton()
        {
            // Get bones to draw
            var skeletonBoneId = 0;
            for (var i = 0; i < StudioHitBoxSet.numhitboxes; i++)
            {
                var hitbox = StudioHitBoxes[i];
                var bone = StudioBones[hitbox.bone];
                if (bone.parent >= 0 && bone.parent < StudioHdr.numbones)
                {
                    // Has valid parent
                    Skeleton[skeletonBoneId] = (hitbox.bone, bone.parent);
                    skeletonBoneId++;
                }
            }
            SkeletonCount = skeletonBoneId;
        }

        #endregion
    }
}
