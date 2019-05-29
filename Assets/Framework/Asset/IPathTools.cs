using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class IPathTools {

    public static string GetPlatformFolderName(RuntimePlatform platforn)
    {
        switch(platforn)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "IOS";
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                return "OSX";
            default:
                return null;
        }
    }

    public static string GetAppFilePath()
    {
        string tmpPath = "";
        if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            tmpPath = Application.streamingAssetsPath;
        } else
        {
            tmpPath = Application.persistentDataPath;
        }
        return tmpPath;
    }

    public static string GetAssetBundlePath()
    {
        string platformFolder = GetPlatformFolderName(Application.platform);
        string allPath = Path.Combine(GetAppFilePath(), platformFolder);
        return allPath;
    }
	
}
