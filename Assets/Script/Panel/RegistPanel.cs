using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U3DEventFrame;

public enum RegistEvent {
	Idle = ManagerID.UIManager+10,
	Load
}
public class RegistPanel : UIBase {

	public override void ProcessEvent (MsgBase tmpMsg)
	{
		//base.ProcessEvent (tmpMsg);
		switch (tmpMsg.msgId) {
		case (ushort)RegistEvent.Idle:
			break;
		case (ushort)RegistEvent.Load:
			break;
		}
	}

	void Start () {
		msgIds = new ushort[] {
			(ushort)RegistEvent.Idle,
			(ushort)RegistEvent.Load
		};
		RegistSelf (this, msgIds);
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.C)) {
			MsgBase tmpMsg = new MsgBase ((ushort)LoginEvent.Load);
			SendMsg (tmpMsg);
		}
	}
}
