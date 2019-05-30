using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class CallBackNode
{
    public string resName;
    // share/name
    public string bundleName;
    public string sceneName;
    public bool isSingle;
    public CallBackNode nextValue;

    public LuaFunction luaFunc;

    public CallBackNode(string tmpRes, string tmpBundle, string tmpScene, LuaFunction tmpFunc, bool single, CallBackNode tmpNode)
    {
        this.resName = tmpRes;
        this.bundleName = tmpBundle;
        this.sceneName = tmpScene;
        this.luaFunc = tmpFunc;
        this.isSingle = single;
        this.nextValue = tmpNode;
    }

    public void Dispose()
    {
        this.resName = null;
        this.bundleName = null;
        this.sceneName = null;
        this.luaFunc.Dispose();
        this.nextValue = null;
    }
}

public class CallBackManager
{
    Dictionary<string, CallBackNode> manager = null;

    public CallBackManager()
    {
        manager = new Dictionary<string, CallBackNode>();
    }

    public void AddBundleCallBack(string bundle, CallBackNode tmpNode)
    {
        if(manager.ContainsKey(bundle))
        {
            CallBackNode topNode = manager[bundle];
            while(topNode.nextValue != null)
            {
                topNode = topNode.nextValue;
            }
            topNode.nextValue = tmpNode;
        } else
        {
            manager.Add(bundle, tmpNode);
        }
    }

    public void Dispose(string bundle)
    {
        if(manager.ContainsKey(bundle))
        {
            CallBackNode topNode = manager[bundle];
            while(topNode.nextValue != null)
            {
                CallBackNode curNode = topNode;
                topNode = topNode.nextValue;
                curNode.Dispose();
            }
            topNode.Dispose();
            manager.Remove(bundle);
        }
    }

    public void CallBackRes(string bundle)
    {
        if (manager.ContainsKey(bundle))
        {
            CallBackNode topNpde = manager[bundle];
            do
            {
                if(topNpde.isSingle)
                {
                    object tmpObj = null;
                    topNpde.luaFunc.Call(topNpde.sceneName, topNpde.bundleName, topNpde.resName, tmpObj);
                } else
                {
                    object[] tmpObjs = null;
                    topNpde.luaFunc.Call(topNpde.sceneName, topNpde.bundleName, topNpde.resName, tmpObjs);
                }
                topNpde = topNpde.nextValue;
            } while (topNpde != null);
        }
        else
        {
            Debug.LogWarning("not contain bundle == " + bundle);
        }
    }

}

public class LuaLoadRes {
    private static LuaLoadRes instance;
    public static LuaLoadRes Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new LuaLoadRes();
            }
            return instance;
        }
    }

    private CallBackManager callbackManager;
    public CallBackManager CBManager
    {
        get
        {
            if (callbackManager == null)
            {
                callbackManager = new CallBackManager();
            }
            return callbackManager;
        }
    }

    // sceneone/load.ld
    void LoadingProgress(string bundleName, float progress)
    {
        if (progress > 1.0f)
        {
            //上层的回调
            CBManager.CallBackRes(bundleName);
            CBManager.Dispose(bundleName);
        }
    }

    public void GetResources(string sceneName, string bundleName, string resName, bool single, LuaFunction tmpFunc)
    {
        //没有加载
        if (!ILoadManager.Instance.IsLoadingAssetBundle(sceneName, bundleName))
        {
            ILoadManager.Instance.LoadAsset(sceneName, bundleName, LoadingProgress);
            string bundleFullName = ILoadManager.Instance.GetBundleRelaName(sceneName, bundleName);
            if (bundleFullName != null)
            {
                CallBackNode tmpNode = new CallBackNode(sceneName, bundleName, resName, tmpFunc, single, null);

                //bundleName == Load
                //bundleFullName == sceneone/load.ld

                //链式结构，请求命令
                CBManager.AddBundleCallBack(bundleFullName, tmpNode);
            }
            else
            {
                Debug.LogWarning("do not contain bundle ==" + bundleFullName);
            }
        }
        //表示已经加载完成
        else if (ILoadManager.Instance.IsLoadingFinish(sceneName, bundleName))
        {
            if (single)
            {
                Object tmpObj = ILoadManager.Instance.GetSingleResource(sceneName, bundleName, resName);
                tmpFunc.Call(sceneName, bundleName, resName, tmpObj);
            }
            else
            {
                Object[] tmpObj = ILoadManager.Instance.GetMutiResources(sceneName, bundleName, resName);
                tmpFunc.Call(sceneName, bundleName, resName, tmpObj);
            }
        }
        else
        {
            //表示已经加载，但是没有完成
            //把命令存下来
            string bundleFullName = ILoadManager.Instance.GetBundleRelaName(sceneName, bundleName);
            if (bundleFullName != null)
            {
                CallBackNode tmpNode = new CallBackNode(sceneName, bundleName, resName, tmpFunc, single, null);
                CBManager.AddBundleCallBack(bundleFullName, tmpNode);
            }
            else
            {
                Debug.LogWarning("do not contain bundle ==" + bundleFullName);
            }
        }
    }

    public void UnLoadResObj(string sceneName, string bundleName, string resName)
    {
        ILoadManager.Instance.UnLoadResObj(sceneName, bundleName, resName);
    }
}
