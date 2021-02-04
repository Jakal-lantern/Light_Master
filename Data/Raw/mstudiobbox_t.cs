using System.Runtime.InteropServices;
using Microsoft.DirectX;

namespace LightCheat.Data.Raw
{
    /*
     * https://github.com/ValveSoftware/source-sdk-2013/blob/0d8dceea4310fde5706b3ce1c70609d72a38efdf/sp/src/public/studio.h#L420
     */
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct mstudiobbox_t
    {
        public int bone;
        public int group;               // Intersection group
        public Vector3 bbmin;           // Bounding box
        public Vector3 bbmax;
        public int szhitboxnameindex;   // Offset to the name of the hitbox
        public fixed int unused[3];
        public float radius;            // When radius is -1 it's a box, otherwise it's a capsule
        public fixed int pad[4];
    }
}
