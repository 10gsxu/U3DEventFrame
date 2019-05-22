using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U3DEventFrame {
	public class NPCBase : MonoBase {

		public override void SendMsg(MsgBase msg) {
			NPCManager.Instance.SendMsg (msg);
		}

		public override void ProcessEvent (MsgBase tmpMsg)
		{
			//throw new System.NotImplementedException ();
		}

		public void RegistSelf(MonoBase mono, params ushort[] msgs) {
			NPCManager.Instance.RegistMsg (mono, msgs);
		}

		public void UnRegistSelf(MonoBase mono, params ushort[] msgs) {
			NPCManager.Instance.UnRegistMsg (mono, msgs);
		}

		public ushort[] msgIds;

		void OnDestroy() {
			if (msgIds != null) {
				UnRegistSelf (this, msgIds);
			}
		}
	}
}