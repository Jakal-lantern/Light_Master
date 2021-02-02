using System.Runtime.InteropServices;
using Microsoft.DirectX;

namespace LightCheat.Data.Raw
{
    /*
     * https://github.com/ValveSoftware/source-sdk-2013/blob/0d8dceea4310fde5706b3ce1c70609d72a38efdf/sp/src/public/studio.h#L238
     */
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct mstudiobone_t
    {
        public int sznameindex;
        public int parent;                  // Parent bone
        public fixed int bonecontroller[6]; // Bone controller index, -1 == none
        public Vector3 pos;
        public Quaternion quat;
        public Vector3 rot;
        public Vector3 posscale;
        public Vector3 rotscale;
        public matrix3x4_t poseToBone;
        public Quaternion qAlignment;
        public int flags;
        public int proctype;
        public int procindex;               // Procedural rule
        public int physicsbone;             // Index into physically simulated bone
        public int surfacepropidx;          // Index into string 'tablefor' property name
        public int contents;                // See BSPFlags.h for contents flags
        public fixed int unused[8];         // remove as appropriate
    }
}
