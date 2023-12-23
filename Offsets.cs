using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bonesp
{
    public static class Offsets
    {
        //offserts.cs
        public static int dwLocalPlayerPawn = 0x16C8F38;
        public static int dwEntityList = 0x17C1950;
        public static int dwViewMatrix = 0x1820150;

        //client dll
        public static int m_hPlayerPawn = 0x7EC;
        public static int m_vOldOrigin = 0x1224;
        public static int m_iTeamNum = 0x3BF;
        public static int m_lifeState = 0x330;
        public static int m_modelState = 0x160;
        public static int m_pGameSceneNode = 0x310;
    }
}
