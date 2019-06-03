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
    public ushort backMsgId;
    public bool isSingle;

    public NativeResCallBackNode(string scene, string bundle, string res, ushort msgid, bool single, NativeResCallBack tmpBack, NativeResCallBackNode tmpValue)
    {
        this.sceneName = scene;
        this.bundleName = bundle;
        this.resName = res;
        this.backMsgId = msgid;
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
        RegistSelf(this, msgIds);
    }

    HunkAssetResBack resBackMsg = null;
    HunkAssetResBack ReleaseBack
    {
        get
        {
            if(resBackMsg == null)
            {
                resBackMsg = new HunkAssetResBack();
            }
            return this.resBackMsg;
        }
    }

    NativeResCallBackManager callBackManager = null;
    NativeResCallBackManager CallBackManager
    {
        get
        {
            if (callBackManager == null)
            {
                callBackManager = new NativeResCallBackManager();
            }
            return this.callBackManager;
        }
    }

    public void SendToBackMsg(NativeResCallBackNode tmpNode)
    {
        if(tmpNode.isSingle)
        {
            UnityEngine.Object tmpObj = ILoadManager.Instance.GetSingleResource(tmpNode.sceneName, tmpNode.bundleName, tmpNode.resName);
            this.ReleaseBack.Changer(tmpNode.backMsgId, tmpObj);
            SendMsg(ReleaseBack);
        } else
        {
            UnityEngine.Object[] tmpObj = ILoadManager.Instance.GetMutiResources(tmpNode.sceneName, tmpNode.bundleName, tmpNode.resName);
            this.ReleaseBack.Changer(tmpNode.backMsgId, tmpObj);
            SendMsg(ReleaseBack);
        }
    }

    // sceneone/load.ld
    void LoadingProgress(string bundleName, float progress)
    {
        if(progress > 1.0f)
        {
            //上层的回调
            CallBackManager.ResCallBack(bundleName);
            CallBackManager.Dispose();
        }
    }

    void GetResources(string sceneName, string bundleName, string resName, bool single, ushort backid)
    {
        //没有加载
        if(!ILoadManager.Instance.IsLoadingAssetBundle(sceneName, bundleName))
        {
            ILoadManager.Instance.LoadAsset(sceneName, bundleName, LoadingProgress);
            string bundleFullName = ILoadManager.Instance.GetBundleRelaName(sceneName, bundleName);
            if(bundleFullName != null)
            {
                NativeResCallBackNode tmpNode = new NativeResCallBackNode(sceneName, bundleName, resName, backid, single, SendToBackMsg, null);

                //bundleName == Load
                //bundleFullName == sceneone/load.ld

                //链式结构，请求命令
                CallBackManager.AddBundle(bundleFullName, tmpNode);
            } else
            {
                Debug.LogWarning("do not contain bundle ==" + bundleFullName);
            }
        }
        //表示已经加载完成
        else if(ILoadManager.Instance.IsLoadingFinish(sceneName, bundleName))
        {
            if(single)
            {
                Object tmpObj = ILoadManager.Instance.GetSingleResource(sceneName, bundleName, resName);
                this.ReleaseBack.Changer(backid, tmpObj);
                SendMsg(ReleaseBack);
            } else
            {
                Object[] tmpObj = ILoadManager.Instance.GetMutiResources(sceneName, bundleName, resName);
                this.ReleaseBack.Changer(backid, tmpObj);
                SendMsg(ReleaseBack);
            }
        } else
        {
            //表示已经加载，但是没有完成
            //把命令存下来
            string bundleFullName = ILoadManager.Instance.GetBundleRelaName(sceneName, bundleName);
            if (bundleFullName != null)
            {
                NativeResCallBackNode tmpNode = new NativeResCallBackNode(sceneName, bundleName, resName, backid, single, SendToBackMsg, null);
                CallBackManager.AddBundle(bundleFullName, tmpNode);
            }
            else
            {
                Debug.LogWarning("do not contain bundle ==" + bundleFullName);
            }
        }
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch(tmpMsg.msgId)
        {
            case (ushort)AssetEvent.HunkRes:
                {
                    HunkAssetRes tmpHunkMsg = (HunkAssetRes)tmpMsg;
                    GetResources(tmpHunkMsg.sceneName, tmpHunkMsg.bundleName, tmpHunkMsg.resName, tmpHunkMsg.isSingle, tmpHunkMsg.backMsgId);
                }
                break;
            case (ushort)AssetEvent.ReleaseSingleObj:
                {
                    HunkAssetRes tmpHunkMsg = (HunkAssetRes)tmpMsg;
                    ILoadManager.Instance.UnLoadResObj(tmpHunkMsg.sceneName, tmpHunkMsg.bundleName, tmpHunkMsg.resName);
                }
                break;
            case (ushort)AssetEvent.ReleaseBundleObjes:

                break;
            case (ushort)AssetEvent.ReleaseSceneObjes:

                break;
            case (ushort)AssetEvent.ReleaseSingleBundle:
                {
                    HunkAssetRes tmpHunkMsg = (HunkAssetRes)tmpMsg;
                    ILoadManager.Instance.UnLoadAssetBundle(tmpHunkMsg.sceneName, tmpHunkMsg.bundleName);
                }
                break;
            case (ushort)AssetEvent.ReleaseSceneBundle:

                break;
            case (ushort)AssetEvent.ReleaseAll:
                {
                    HunkAssetRes tmpHunkMsg = (HunkAssetRes)tmpMsg;
                    ILoadManager.Instance.UnLoadAllAssetBundleAndResObjs(tmpHunkMsg.sceneName);
                }
                break;
        }
    }
}
