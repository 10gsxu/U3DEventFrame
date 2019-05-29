using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U3DEventFrame;

public delegate void NativeResCallBack(NativeResCallBackNode tmpNode);

public class NativeResCallBackNode
{
    public NativeResCallBackNode nextValue;
    public NativeResCallBack callBack;
    public string resName;
    public string bundleName;
    public string sceneName;
    public ushort msgId;
    public bool isSingle;

    public NativeResCallBackNode(string scene, string bundle, string res, ushort msgid, bool single, NativeResCallBack tmpBack, NativeResCallBackNode tmpValue)
    {
        this.sceneName = scene;
        this.bundleName = bundle;
        this.resName = res;
        this.msgId = msgid;
        this.isSingle = single;
        this.callBack = tmpBack;
        this.nextValue = tmpValue;
    }

    public void Dispose()
    {
        nextValue = null;
        callBack = null;
        resName = null;
        bundleName = null;
        sceneName = null;
    }
}

public class NativeResCallBackManager
{
    //bundleName, 相对路径
    Dictionary<string, NativeResCallBackNode> manager = null;
    public NativeResCallBackManager()
    {
        manager = new Dictionary<string, NativeResCallBackNode>();
    }

    public bool ContainsKey(string name)
    {
        return manager.ContainsKey(name);
    }

    public void AddBundle(string bundle, NativeResCallBackNode tmpNode)
    {
        if (manager.ContainsKey(bundle))
        {
            NativeResCallBackNode topNode = manager[bundle];
            while(topNode.nextValue != null)
            {
                topNode = topNode.nextValue;
            }
            topNode.nextValue = tmpNode;
        }
        else
        {
            manager.Add(bundle, tmpNode);
        }
    }

    public void Dispose()
    {
        manager.Clear();
    }

    public void Dispose(string bundle)
    {
        if(manager.ContainsKey(bundle))
        {
            NativeResCallBackNode topNode = manager[bundle];
            //挨个释放
            while(topNode.nextValue != null)
            {
                NativeResCallBackNode curNode = topNode;
                topNode = topNode.nextValue;
                curNode.Dispose();
            }
            //最后一个节点的释放
            topNode.Dispose();

            manager.Remove(bundle);
        }
    }

    public void ResCallBack(string bundle)
    {
        if(manager.ContainsKey(bundle))
        {
            NativeResCallBackNode topNpde = manager[bundle];
            do
            {
                topNpde.callBack(topNpde);
                topNpde = topNpde.nextValue;
            } while (topNpde != null);
        } else
        {
            Debug.LogWarning("not contain bundle == " + bundle);
        }
    }
}

public class NativeLoadRes : AssetBase {

    private void Awake()
    {
        msgIds = new ushort[]
        {

            (ushort)AssetEvent.HunkRes,
            (ushort)AssetEvent.ReleaseSingleObj,
            (ushort)AssetEvent.ReleaseBundleObjes,
            (ushort)AssetEvent.ReleaseSceneObjes,
            (ushort)AssetEvent.ReleaseSingleBundle,
            (ushort)AssetEvent.ReleaseSceneBundle,
            (ushort)AssetEvent.ReleaseAll,
        };
    }

    void GetResources(string scene, string bundle, string res, bool single, ushort msgid)
    {

    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch(tmpMsg.msgId)
        {
            case (ushort)AssetEvent.HunkRes:

                break;
            case (ushort)AssetEvent.ReleaseSingleObj:

                break;
            case (ushort)AssetEvent.ReleaseBundleObjes:

                break;
            case (ushort)AssetEvent.ReleaseSceneObjes:

                break;
            case (ushort)AssetEvent.ReleaseSingleBundle:

                break;
            case (ushort)AssetEvent.ReleaseSceneBundle:

                break;
            case (ushort)AssetEvent.ReleaseAll:

                break;
        }
    }
}
