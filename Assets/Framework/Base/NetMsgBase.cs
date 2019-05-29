using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U3DEventFrame;

namespace U3DEventFrame
{
    public class NetMsgBase : MsgBase
    {

        public NetMsgBase(ushort tmpMsg) : base(tmpMsg)
        {

        }

        public override byte GetState()
        {
            return base.GetState();
        }
    }
}
