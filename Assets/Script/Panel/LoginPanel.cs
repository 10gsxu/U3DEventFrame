using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U3DEventFrame;

public enum LoginEvent {
	Idle = ManagerID.UIManager+1,
	Load,
	DoubleTouch
}

public class LoginPanel : UIBase {

	public enum ILoadState
	{
		Idle,
		Load
	}

	ILoadState loadState = ILoadState.Idle;

	public override void ProcessEvent (MsgBase tmpMsg)
	{
		//base.ProcessEvent (tmpMsg);
		if (tmpMsg.msgId == (ushort)LoginEvent.Idle) {
			Debug.Log ("Idle");
			loadState = ILoadState.Idle;
		} else if(tmpMsg.msgId == (ushort)LoginEvent.Load) {
			Debug.Log ("Load");
			loadState = ILoadState.Load;
		}
	}

	void Start() {
		msgIds = new ushort[] {
			(ushort)LoginEvent.Idle,
			(ushort)LoginEvent.Load
		};
		RegistSelf (this, msgIds);
	}

	void Update() {
		if (loadState == ILoadState.Idle) {
		}
	}
}
