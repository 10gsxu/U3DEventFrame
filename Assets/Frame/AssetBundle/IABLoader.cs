using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LoaderProgress(string bundle, float progress);
public delegate void LoadFinish(string bundle);

public class IABLoader {

    private string bundleName;
    private string commonBundlePath = "";
    private WWW commonLoader;
    private float commResLoaderProgress;

    private LoaderProgress loaderProgress;
    private LoadFinish loadFinish;

    private IABResLoader abResLoader;

    public IABLoader()
    {
        commonBundlePath = "";
        bundleName = "";
        commResLoaderProgress = 0;

        loaderProgress = null;
        loadFinish = null;

        abResLoader = null;
    }

    public IABLoader(LoaderProgress loaderProgress, LoadFinish loadFinish) : this()
    {
        this.loaderProgress = loaderProgress;
        this.loadFinish = loadFinish;
    }

    public void SetBundleName(string bundle)
    {
        this.bundleName = bundle;
    }

    public void LoadResources(string path)
    {
        commonBundlePath = path;
    }
    //携程加载
    public IEnumerator CommonLoad()
    {
        commonLoader = new WWW(commonBundlePath);
        while(!commonLoader.isDone)
        {
            commResLoaderProgress = commonLoader.progress;

            if(loaderProgress != null)
            {
                loaderProgress(bundleName, commResLoaderProgress);
            }

            yield return commonLoader.progress;
            commResLoaderProgress = commonLoader.progress;
        }

        if(commResLoaderProgress>=1.0f)//表示加载完成
        {
            if (loaderProgress != null)
            {
                loaderProgress(bundleName, commResLoaderProgress);
            }
            if (loadFinish != null)
            {
                loadFinish(bundleName);
            }

            abResLoader = new IABResLoader(commonLoader.assetBundle);
        }
        else
        {
            Debug.LogError("load bundle error == " + bundleName);
        }

        commonLoader = null;
    }

    //Debug
    public void DebugLoader()
    {
        if(commonLoader != null)
        {
            abResLoader.DebugAllRes();
        }
    }

    #region  下层提供功能
    //获取单个资源
    public UnityEngine.Object GetResources(string name)
    {
        if (abResLoader == null)
            return null;
        return abResLoader[name];
    }
    //获取多个资源
    public UnityEngine.Object[] GetMutiRes(string name)
    {
        if (abResLoader == null)
            return null;
        return abResLoader.LoadResources(name);
    }
    //释放资源
    public void Dispose()
    {
        if (abResLoader != null)
        {
            abResLoader.Dispose();
            abResLoader = null;
        }
    }
    //卸载单个资源
    public void UnLoadRes(UnityEngine.Object tmpObj)
    {
        if (abResLoader != null)
        {
            abResLoader.UnLoadRes(tmpObj);
        }
    }
    #endregion
}
