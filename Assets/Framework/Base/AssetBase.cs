using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U3DEventFrame {
    public class AssetBase : MonoBase {
        public override void SendMsg(MsgBase msg)
        {
            AssetManager.Instance.SendMsg(msg);
        }

        public override void ProcessEvent(MsgBase tmpMsg)
        {
            //throw new System.NotImplementedException ();
        }

        public void RegistSelf(MonoBase mono, params ushort[] msgs)
        {
            AssetManager.Instance.RegistMsg(mono, msgs);
        }

        public void UnRegistSelf(MonoBase mono, params ushort[] msgs)
        {
            AssetManager.Instance.UnRegistMsg(mono, msgs);
        }

        public ushort[] msgIds;

        void OnDestroy()
        {
            if (msgIds != null)
            {
                UnRegistSelf(this, msgIds);
            }
        }
    }
}
