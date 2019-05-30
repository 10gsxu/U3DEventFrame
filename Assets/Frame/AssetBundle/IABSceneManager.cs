using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class IABSceneManager {
    IABManager abManager;
    public IABSceneManager(string sceneName)
    {

    }

    private Dictionary<string, string> allAsset;

    /// <summary>
    /// 场景名字
    /// </summary>
    public void ReadConfiger(string sceneName)
    {
        string textFileName = "Record.txt";
        allAsset = new Dictionary<string, string>();
        string path = IPathTools.GetAssetBundlePath() + "/AssetBundle/" + sceneName + textFileName;
        abManager = new IABManager(sceneName);
        ReadConfig(path);
    }

    private void ReadConfig(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);
        StreamReader br = new StreamReader(fs);
        string line = br.ReadLine();
        int allCount = int.Parse(line);
        for(int i=0; i<allCount; ++i)
        {
            string tmpStr = br.ReadLine();
            string[] tmpArr = tmpStr.Split(" ".ToCharArray());
            //bundleName -- bundleFullName
            //Load -- sceneone/load.ld
            allAsset.Add(tmpArr[0], tmpArr[1]);
        }
        br.Close();
        fs.Close();
    }

    public void LoadAsset(string bundleName, LoaderProgress progress, LoadAssetBundleCallBack callBack)
    {
        if(allAsset.ContainsKey(bundleName))
        {
            string tmpValue = allAsset[bundleName];
            abManager.LoadAssetBundle(tmpValue, progress, callBack);
        } else
        {
            Debug.Log("Donot contain the bundle == " + bundleName);
        }
    }

    #region 由下层提供功能
    public IEnumerator LoadAssetSys(string bundleName)
    {
        yield return abManager.LoadAssetBundle(bundleName);
    }

    public UnityEngine.Object GetSingleResource(string bundleName, string resName)
    {
        if(allAsset.ContainsKey(bundleName))
        {
            return abManager.GetSingleResource(allAsset[bundleName], resName);
        } else
        {
            Debug.Log("Donot contain the bundle == " + bundleName);
            return null;
        }
    }

    public UnityEngine.Object[] GetMutiResources(string bundleName, string resName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return abManager.GetMutiResources(allAsset[bundleName], resName);
        }
        else
        {
            Debug.Log("Donot contain the bundle == " + bundleName);
            return null;
        }
    }

    public void DisposeResObj(string bundleName, string resName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            abManager.DisposeResObj(allAsset[bundleName], resName);
        }
        else
        {
            Debug.Log("Donot contain the bundle == " + bundleName);
        }
    }

    public void DisposeBundleRes(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            abManager.DisposeResObj(allAsset[bundleName]);
        }
        else
        {
            Debug.Log("Donot contain the bundle == " + bundleName);
        }
    }

    public void DisposeAllRes()
    {
        abManager.DisposeAllObj();
    }

    public void DisposeBundle(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            abManager.DisposeBundle(bundleName);
        }
    }

    public void DisposeAllBundle()
    {
        abManager.DisposeAllBundle();

        allAsset.Clear();
    }

    public void DisposeAllBundleAndRes()
    {
        abManager.DisposeAllBundleAndRes();

        allAsset.Clear();
    }

    public void DebugAllAsset()
    {
        List<string> keys = new List<string>();
        keys.AddRange(allAsset.Keys);
        for(int i=0; i<keys.Count; ++i)
        {
            abManager.DebugAssetBundle(allAsset[keys[i]]);
        }
    }

    public bool IsLoadingFinish(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return abManager.IsLoadingFinish(allAsset[bundleName]);
        }
        else
        {
            Debug.Log("is not contain bundle == " + bundleName);
        }
        return false;
    }

    public bool IsLoadingAssetBundle(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return abManager.IsLoadingAssetBundle(allAsset[bundleName]);
        }
        else
        {
            Debug.Log("is not contain bundle == " + bundleName);
        }
        return false;
    }

    public string GetBundleRelaName(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return allAsset[bundleName];
        }
        else
        {
            return null;
        }
    }
    #endregion
}
