using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U3DEventFrame {
	public class MsgCenter : MonoBehaviour {

		public static MsgCenter Instance;

		private Dictionary<ManagerID, ManagerBase> managerDict = new Dictionary<ManagerID, ManagerBase>();

		void Awake() {
			Instance = this;
			managerDict.Add(ManagerID.UIManager, gameObject.AddComponent<UIManager> ());
            managerDict.Add(ManagerID.NPCManager, gameObject.AddComponent<NPCManager>());
            managerDict.Add(ManagerID.NetManager, gameObject.AddComponent<NetManager>());
            managerDict.Add(ManagerID.AudioManager, gameObject.AddComponent<AudioManager>());
            managerDict.Add(ManagerID.AssetManager, gameObject.AddComponent<AssetManager>());

            gameObject.AddComponent<LuaEventProcess>();
        }

        public void SendToMsg(MsgBase msg) {
            ManagerBase baseManager = managerDict[msg.GetManager()];
            if (baseManager == null) {
                Debug.LogError("Manager不存在");
                return;
            }
            baseManager.SendMsg(msg);
        }

	}
}