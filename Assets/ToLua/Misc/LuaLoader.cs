using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LuaInterface;
using LeoHui;

public class LuaLoader : MonoBehaviour
{
    private void Awake()
    {
        LuaFileUtils.Instance.beZip = UpdateConfig.Instance.LuaBundleMode;
    }

    /// <summary>
    /// 添加打入Lua代码的AssetBundle
    /// </summary>
    /// <param name="bundle"></param>
    public void AddBundle(string bundleName)
    {
        string url = PathTools.DataPath + bundleName.ToLower();
        if (File.Exists(url))
        {
            var bytes = File.ReadAllBytes(url);
            AssetBundle bundle = AssetBundle.LoadFromMemory(bytes);
            if (bundle != null)
            {
                bundleName = bundleName.Replace("lua/", "").Replace(".ab", "");
                LuaFileUtils.Instance.AddSearchBundle(bundleName.ToLower(), bundle);
            }
        }
    }
}
