using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ILoadManager : MonoBehaviour {
    public static ILoadManager Instance;

    private Dictionary<string, IABSceneManager> loadManager = new Dictionary<string, IABSceneManager>();

    private void Awake()
    {
        Instance = this;
        //第一步 加载 IABManifest
        StartCoroutine(IABManifestLoader.Instance.LoadManifest());
    }

    private void OnDestroy()
    {
        loadManager.Clear();
        System.GC.Collect();
    }
    //第二步，读取配置文件
    public void ReadConfiger(string sceneName)
    {
        if(!loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = new IABSceneManager(sceneName);
            tmpManager.ReadConfiger(sceneName);
            loadManager.Add(sceneName, tmpManager);
        }
    }

    public void LoadCallBack(string sceneName, string bundleName)
    {
        if(loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            StartCoroutine(tmpManager.LoadAssetSys(bundleName));
        }
        else
        {
            Debug.Log("bundleName is not contain == " + bundleName);
        }
    }

    //提供加载功能
    public void LoadAsset(string sceneName, string bundleName, LoaderProgress progress)
    {
        if(!loadManager.ContainsKey(sceneName))
        {
            ReadConfiger(sceneName);
        }
        IABSceneManager tmpManager = loadManager[sceneName];
        tmpManager.LoadAsset(bundleName, progress, LoadCallBack);
    }

    #region 由下层API提供
    public string GetBundleRelaName(string sceneName, string bundleName)
    {
        IABSceneManager tmpManager = loadManager[sceneName];
        if(tmpManager != null)
        {
            return tmpManager.GetBundleRelaName(bundleName);
        } else
        {
            return null;
        }
    }
    /// <summary>
    /// Gets the single resource.
    /// </summary>
    /// <returns>The single resource.</returns>
    /// <param name="sceneName">Scene name.</param>
    /// <param name="bundleName">Bundle name.</param>
    /// <param name="resName">Res name.</param>
    public UnityEngine.Object GetSingleResource(string sceneName, string bundleName, string resName)
    {
        if(loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            return tmpManager.GetSingleResource(bundleName, resName);
        } else
        {
            Debug.Log("sceneName == "+sceneName+"Bundle Name == " + bundleName + "is not load");
            return null;
        }
    }

    public UnityEngine.Object[] GetMutiResources(string sceneName, string bundleName, string resName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            return tmpManager.GetMutiResources(bundleName, resName);
        }
        else
        {
            Debug.Log("sceneName == " + sceneName + "Bundle Name == " + bundleName + "is not load");
            return null;
        }
    }

    public void UnLoadResObj(string sceneName, string bundleName, string resName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DisposeResObj(bundleName, resName);
        }
    }

    public void UnLoadBundleResObjs(string sceneName, string bundleName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DisposeBundleRes(bundleName);
        }
    }

    public void UnLoadAllResObjs(string sceneName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DisposeAllRes();
        }
    }

    public void UnLoadAssetBundle(string sceneName, string bundleName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DisposeBundle(bundleName);
        }
    }

    public void UnLoadAllAssetBundle(string sceneName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DisposeAllBundle();
            System.GC.Collect();
        }
    }

    public void UnLoadAllAssetBundleAndResObjs(string sceneName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DisposeAllBundleAndRes();
            System.GC.Collect();
        }
    }

    public void DebugAllAssetBundle(string sceneName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DebugAllAsset();
        }
    }

    public bool IsLoadingFinish(string sceneName, string bundleName)
    {
        bool tmpBool = loadManager.ContainsKey(sceneName);
        if (tmpBool)
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            return tmpManager.IsLoadingFinish(bundleName);
        }
        return false;
    }

    public bool IsLoadingAssetBundle(string sceneName, string bundleName)
    {
        bool tmpBool = loadManager.ContainsKey(sceneName);
        if(tmpBool)
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            return tmpManager.IsLoadingAssetBundle(bundleName);
        }
        return false;
    }
    #endregion
}
