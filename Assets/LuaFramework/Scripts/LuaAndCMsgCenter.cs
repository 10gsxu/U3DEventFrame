using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U3DEventFrame;
using LuaInterface;

public class LuaAndCMsgCenter : MonoBase {

    private static LuaAndCMsgCenter instance;
    public static LuaAndCMsgCenter Instance
    {
        get
        {
            return instance;
        }
    }

    LuaFunction callBack = null;

    void Awake()
    {
        instance = this;
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        if(callBack != null)
        {
            //从网络来
            if (tmpMsg.GetState() != 127)
            {
                NetMsgBase tmpBase = (NetMsgBase)tmpMsg;
                byte[] proto = tmpBase.GetProtoBuffer();
                LuaByteBuffer buffer = new LuaByteBuffer(proto);
                callBack.Call(true, tmpBase.msgId, tmpBase.GetState(), buffer);
            } else//从框架其他模块来的
            {
                callBack.Call(false, tmpMsg);
            }
        }
    }

    public void SettingLuaCallBack(LuaFunction luaFunc )
    {
        callBack = luaFunc;
    }

}
