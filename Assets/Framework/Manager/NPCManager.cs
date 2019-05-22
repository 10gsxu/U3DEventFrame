using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U3DEventFrame {
	public class NPCManager : ManagerBase {

		public static NPCManager Instance;

		void Awake() {
			Instance = this;
		}

		public override void SendMsg(MsgBase msg) {
			if (msg.GetManager () == ManagerID.NPCManager) {
				//ManagerBase 本模块自己处理
				ProcessEvent (msg);
			} else {
				MsgCenter.Instance.SendToMsg (msg);
			}
		}

	}
}