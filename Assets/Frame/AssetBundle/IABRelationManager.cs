﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IABRelationManager
{
    List<string> dependenceBundle;

    List<string> referBundle;

    IABLoader assetLoader;

    string theBundleName;

    LoaderProgress progress;

    public IABRelationManager()
    {
        dependenceBundle = new List<string>();
        referBundle = new List<string>();
    }

    public void AddRefference(string bundleName)
    {
        referBundle.Add(bundleName);
    }

    public List<string> GetRefference()
    {
        return referBundle;
    }

    public void SetDependence(string[] dependence)
    {
        if(dependence.Length > 0)
        {
            dependenceBundle.AddRange(dependence);
        }
    }

    public List<string> GetDependence()
    {
        return dependenceBundle;
    }

    public void RemoveDependence(string bundleName)
    {
        for (int i = 0; i < dependenceBundle.Count; ++i)
        {
            if (bundleName.Equals(dependenceBundle[i]))
            {
                dependenceBundle.RemoveAt(i);
                //break;
            }
        }
    }

    bool IsLoadFinish;
    public void BundleLoadFinish(string bundleName)
    {
        IsLoadFinish = true;
    }

    public bool IsBundleLoadFinish()
    {
        return IsLoadFinish;
    }

    public string GetBundleName()
    {
        return theBundleName;
    }
    public LoaderProgress GetProgress()
    {
        return this.progress;
    }

    public void Initial(string bundleName, LoaderProgress progress)
    {
        IsLoadFinish = false;
        theBundleName = bundleName;
        this.progress = progress;
        assetLoader = new IABLoader(progress, BundleLoadFinish);
    }

    /// <summary>
    /// 是否被释放
    /// </summary>
    /// <returns><c>true</c>, if refference was removed, <c>false</c> otherwise.</returns>
    /// <param name="bundleName">Bundle name.</param>
    public bool RemoveRefference(string bundleName)
    {
        for(int i=0; i<referBundle.Count; ++i)
        {
            if(bundleName.Equals(referBundle[i]))
            {
                referBundle.RemoveAt(i);
                //break;
            }
        }
        if(referBundle.Count <=0)
        {
            Dispose();
            return true;
        }
        return false;
    }

    #region 由下层提供API

    public void DebugAsset()
    {
        if(assetLoader != null)
        {
            assetLoader.DebugLoader();
        } else
        {
            Debug.Log("assetLoader is null");
        }
    }

    //unity3d 5.3以上 协程 才可以多层传递
    public IEnumerator LoadAssetBundle()
    {
        yield return assetLoader.CommonLoad();
    }

    //释放过程
    public void Dispose()
    {
        assetLoader.Dispose();
    }

    public Object GetSingleResource(string bundleName)
    {
        return assetLoader.GetResources(bundleName);
    }

    public Object[] GetMutiResources(string bundleName)
    {
        return assetLoader.GetMutiRes(bundleName);
    }
    #endregion
}
