using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U3DEventFrame;

public class HunkAssetRes : MsgBase {

    public string sceneName;
    public string bundleName;
    public string resName;
    public ushort backMsgId;
    public bool isSingle;

    public HunkAssetRes (bool single, ushort msgid, string scene, string bundle, string res, ushort backmsgid)
    {
        this.isSingle = single;
        this.msgId = msgid;
        this.sceneName = scene;
        this.bundleName = bundle;
        this.resName = res;
        this.backMsgId = backmsgid;
    }
	
}

public class HunkAssetResBack : MsgBase
{
    public UnityEngine.Object[] value;
    public HunkAssetResBack()
    {
        this.msgId = 0;
        this.value = null;
    }

    public void Changer(ushort msgid, params UnityEngine.Object[] tmpValue)
    {
        this.msgId = msgid;
        this.value = tmpValue;
    }

    public void Changer(ushort msgid)
    {
        this.msgId = msgid;
    }

    public void Changer(params UnityEngine.Object[] tmpValue)
    {
        this.value = tmpValue;
    }
}