﻿using System.Runtime.InteropServices;

namespace LightCheat.Data.Raw
{
    /*
     * https://github.com/ValveSoftware/source-sdk-2013/blob/0d8dceea4310fde5706b3ce1c70609d72a38efdf/sp/src/public/studio.h#L1612
     */
    [StructLayout(LayoutKind.Sequential)]
    public struct mstudiohitboxset_t
    {
        public int sznameindex;
        public int numhitboxes;
        public int hitboxindex;
    }
}
