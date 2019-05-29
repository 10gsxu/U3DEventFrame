using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U3DEventFrame
{
    public class AssetManager : ManagerBase
    {

        public static AssetManager Instance;

        void Awake()
        {
            Instance = this;
        }

        public override void ProcessEvent(MsgBase tmpMsg)
        {
        }

    }
}
