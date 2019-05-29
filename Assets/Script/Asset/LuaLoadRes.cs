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

    private static CallBackManager callbackManager;
    public static CallBackManager CBManager
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

    public void GetResources(string sceneName, string bundleName, string resName, bool single, LuaFunction tmpFunc)
    {
        if(!ILoadManager.Instance.IsLoadingAssetBundle(sceneName, bundleName)) {

        }
    }
}
