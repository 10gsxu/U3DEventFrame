using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LoadAssetBundleCallBack(string sceneName, string bundleName);

//单个存取
public class AssetObj
{
    public List<Object> objs;
    public AssetObj(params Object[] objects)
    {
        objs = new List<Object>();
        objs.AddRange(objects);
    }
    public void ReleaseObj()
    {
        for(int i=0; i<objs.Count; ++i)
        {
            Resources.UnloadAsset(objs[i]);
        }
    }
}
//多个存取
public class AssetResObj
{
    public Dictionary<string, AssetObj> resObjs;
    public AssetResObj(string name, AssetObj tmp)
    {
        resObjs = new Dictionary<string, AssetObj>();
        resObjs.Add(name, tmp);
    }

    public void AddResObj(string name, AssetObj tmpObj)
    {
        resObjs.Add(name, tmpObj);
    }
    public void ReleaseAllObj()
    {
        List<string> keys = new List<string>();
        keys.AddRange(resObjs.Keys);
        for(int i=0; i<keys.Count; ++i)
        {
            ReleaseResObj(keys[i]);
        }
    }
    public void ReleaseResObj(string name)
    {
        if(resObjs.ContainsKey(name))
        {
            AssetObj tmpObj = resObjs[name];
            tmpObj.ReleaseObj();
        } else
        {
            Debug.Log("not contain");
        }
    }
    public List<Object> GetResObj(string name)
    {
        if(resObjs.ContainsKey(name))
        {
            AssetObj tmpObj = resObjs[name];
            return tmpObj.objs;
        } else
        {
            return null;
        }
    }
}

//对一个场景的所有Bundle包管理
public class IABManager {
    //把每一个包都存起来
    Dictionary<string, IABRelationManager> loadHelper = new Dictionary<string, IABRelationManager>();

    Dictionary<string, AssetResObj> loadObj = new Dictionary<string, AssetResObj>();

    string sceneName;

    public IABManager(string sceneName)
    {
        this.sceneName = sceneName;
    }

    public bool IsLoadingFinish(string bundleName)
    {
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager tmpManager = loadHelper[bundleName];
            return tmpManager.IsBundleLoadFinish();
        }
        else
        {
            return false;
        }
    }

    //表示是否加载了Bundle
    public bool IsLoadingAssetBundle(string bundleName)
    {
        if (!loadHelper.ContainsKey(bundleName))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public Object GetSingleResource(string bundleName, string resName)
    {
        if(loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpRes = loadObj[bundleName];
            List<Object> tmpObj = tmpRes.GetResObj(resName);
            if(tmpObj != null && tmpObj.Count > 0)
            {
                return tmpObj[0];
            }
        }

        //表示已经加载过Bundle包
        if(loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            Object tmpObj = loader.GetSingleResource(resName);
            AssetObj tmpAssetObj = new AssetObj(tmpObj);
            //缓存里面是否已经有这个包
            if(loadObj.ContainsKey(bundleName))
            {
                AssetResObj tmpRes = loadObj[bundleName];
                tmpRes.AddResObj(resName, tmpAssetObj);
            } else
            {
                //没有加载过这个包
                AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);
                loadObj.Add(bundleName, tmpRes);
            }
            return tmpObj;
        }

        return null;
    }

    public Object[] GetMutiResources(string bundleName, string resName)
    {
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpRes = loadObj[bundleName];
            List<Object> tmpObj = tmpRes.GetResObj(resName);
            if (tmpObj != null && tmpObj.Count > 0)
            {
                return tmpObj.ToArray();
            }
        }

        //表示已经加载过Bundle包
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            Object[] tmpObj = loader.GetMutiResources(resName);
            AssetObj tmpAssetObj = new AssetObj(tmpObj);
            //缓存里面是否已经有这个包
            if (loadObj.ContainsKey(bundleName))
            {
                AssetResObj tmpRes = loadObj[bundleName];
                tmpRes.AddResObj(resName, tmpAssetObj);
            }
            else
            {
                //没有加载过这个包
                AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);
                loadObj.Add(bundleName, tmpRes);
            }
            return tmpObj;
        }

        return null;
    }

    #region 释放缓存物体

    public void DisposeResObj(string bundleName, string resName)
    {
        if(loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpRes = loadObj[bundleName];
            tmpRes.ReleaseResObj(resName);
        }
    }

    public void DisposeResObj(string bundleName)
    {
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpRes = loadObj[bundleName];
            tmpRes.ReleaseAllObj();
        }
        Resources.UnloadUnusedAssets();
    }
    public void DisposeAllObj()
    {
        List<string> keys = new List<string>();
        keys.AddRange(loadObj.Keys);
        for(int i=0; i<keys.Count; ++i)
        {
            DisposeResObj(keys[i]);
        }
        loadObj.Clear();
    }

    public void DisposeBundle(string bundleName)
    {
        if(loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            List<string> dependences = loader.GetDependence();
            for(int i=0; i<dependences.Count; ++i)
            {
                if(loadHelper.ContainsKey(dependences[i]))
                {
                    IABRelationManager dependence = loadHelper[dependences[i]];
                    if(dependence.RemoveRefference(bundleName))
                    {
                        DisposeBundle(dependence.GetBundleName());
                    }
                }
            }
            if(loader.GetRefference().Count <=0)
            {
                loader.Dispose();
                loadHelper.Remove(bundleName);
            }
        }
    }

    public void DisposeAllBundle()
    {
        List<string> keys = new List<string>();
        keys.AddRange(loadHelper.Keys);
        for (int i = 0; i < keys.Count; ++i)
        {
            IABRelationManager loader = loadHelper[keys[i]];
            loader.Dispose();
        }
        loadHelper.Clear();
    }

    public void DisposeAllBundleAndRes()
    {
        DisposeAllBundle();
        DisposeAllObj();
    }
    #endregion

    public string[] GetDependence(string bundleName)
    {
        return IABManifestLoader.Instance.GetDependences(bundleName);
    }

    //对外的接口
    public void LoadAssetBundle(string bundleName, LoaderProgress progress, LoadAssetBundleCallBack callBack)
    {
        if(!loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = new IABRelationManager();
            loader.Initial(bundleName, progress);
            loadHelper.Add(bundleName, loader);
            callBack(sceneName, bundleName);
        } else
        {
            Debug.Log("IABManager not contain bundle");
        }
    }

    public IEnumerator LoadAssetBundleDependences(string bundleName, string refName, LoaderProgress progress)
    {
        if(!loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = new IABRelationManager();
            loader.Initial(bundleName, progress);

            if(refName != null)
            {
                loader.AddRefference(refName);
            }

            loadHelper.Add(bundleName, loader);

            yield return LoadAssetBundle(bundleName);
        } else
        {
            if (refName != null)
            {
                IABRelationManager loader = loadHelper[bundleName];
                loader.AddRefference(refName);
            }
        }
    }

    /// <summary>
    /// 加载AssetBundle,必须先加载Manifest
    /// </summary>
    /// <returns>The asset bundle.</returns>
    /// <param name="bundleName">Bundle name.</param>
    public IEnumerator LoadAssetBundle(string bundleName)
    {
        while(!IABManifestLoader.Instance.IsLoadFinish())
        {
            yield return null;
        }
        IABRelationManager loader = loadHelper[bundleName];
        string[] dependence = GetDependence(bundleName);
        loader.SetDependence(dependence);
        for(int i=0; i<dependence.Length; ++i) {
            yield return LoadAssetBundleDependences(dependence[i], bundleName, loader.GetProgress());
        }
        yield return loader.LoadAssetBundle();
    }

    #region 由下层提供API
    public void DebugAssetBundle(string bundleName)
    {
        if(loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            loader.DebugAsset();
        } else
        {
            Debug.Log("IABRelationManager not contain");
        }
    }
    #endregion
}
