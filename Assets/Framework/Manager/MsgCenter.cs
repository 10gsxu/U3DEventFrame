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
        }

        public void SendToMsg(MsgBase msg) {
            byte tmpId = (byte)msg.GetManager();
            if (tmpId < 127) {
                LuaEventProcess.Instance.ProcessEvent(msg);
            } else {
                ManagerBase baseManager = managerDict[msg.GetManager()];
                if (baseManager == null) {
                    Debug.LogError("Manager不存在");
                    return;
                }
                baseManager.SendMsg(msg);
            }

        }

	}
}