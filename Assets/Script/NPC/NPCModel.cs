using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U3DEventFrame;

public class NPCModel : NPCBase {

	public override void SendMsg (MsgBase msg)
	{
		
	}

	public override void ProcessEvent (MsgBase tmpMsg)
	{
		switch (tmpMsg.msgId) {
		case (ushort)NpcEvent.WalkFront:
			Debug.Log("model walk front");
			break;
		case (ushort)NpcEvent.WalkBack:
			Debug.Log("model walk back");
			break;
		default:
			break;
		}
	}

	void Awake() {
		
	}

	//状态机

	// Use this for initialization
	void Start () {
		msgIds = new ushort[] {
			(ushort)NpcEvent.WalkFront,
			(ushort)NpcEvent.WalkBack
		};
		RegistSelf (this, msgIds);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
