using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U3DEventFrame;

public class NPCAnimator : NPCBase {
	public override void SendMsg (MsgBase msg)
	{

	}

	public override void ProcessEvent (MsgBase tmpMsg)
	{
		switch (tmpMsg.msgId) {
		case (ushort)NpcEvent.WalkFront:
			//animator.SetTrigger ("");
			Debug.Log("animator walk front");
			break;
		case (ushort)NpcEvent.WalkBack:
			//animator.SetTrigger ("");
			Debug.Log("animator walk back");
			break;
		default:
			break;
		}
	}

	void Awake() {
		
	}

	Animator animator;

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