using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleEditor
{

    [MenuItem("Itools/BuildAssetBundle")]
    public static void BuildAssetBundle()
    {
        //streamAssetsPath/Windows
        string outPath = IPathTools.GetAssetBundlePath();
        BuildPipeline.BuildAssetBundles(outPath, 0, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    [MenuItem("Itools/MarkAssetBundle")]
    public static void MarkAssetBundle()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        string path = Application.dataPath + "/Art/Scenes/";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileSystemInfo[] fileInfos = dir.GetFileSystemInfos();
        for(int i=0; i<fileInfos.Length; ++i)
        {
            FileSystemInfo tmpFile = fileInfos[i];
            if(tmpFile is DirectoryInfo)
            {
                string tmpPath = Path.Combine(path, tmpFile.Name);
                SceneOverView(tmpPath);
            }
        }

        AssetDatabase.Refresh();
    }
    //遍历整个场景
    public static void SceneOverView(string scenePath)
    {
        string textFileName = "Record.txt";
        string tmpPath = scenePath + textFileName;
        //Debug.Log("tmpPath == " + tmpPath);
        FileStream fs = new FileStream(tmpPath, FileMode.OpenOrCreate);
        StreamWriter bw = new StreamWriter(fs);
        //存储对应关系
        Dictionary<string, string> readDict = new Dictionary<string, string>();
        ChangerHead(scenePath, readDict);

        bw.WriteLine(readDict.Count);
        foreach(string key in readDict.Keys)
        {
            bw.Write(key);
            bw.Write(" ");
            bw.Write(readDict[key]);
            bw.Write("\n");
        }

        bw.Close();
        fs.Close();
    }

    //截取相对路径    D:/ToLuaFish/Assets/Art/Scenes/  SceneOne
    // sceneone/load
    /// <summary>
    /// Changers the head.
    /// </summary>
    /// <param name="fullPath">Full path.</param>
    /// <param name="theWriter">The writer.文本记录</param>
    public static void ChangerHead(string fullPath, Dictionary<string, string> theWriter)
    {
        //得到的是D:/ToLuaFish/总长度
        int tmpCount = fullPath.IndexOf("Assets");
        int tmpLength = fullPath.Length;
        //Assets/Art/Scenes/  SceneOne
        string replacePath = fullPath.Substring(tmpCount, tmpLength - tmpCount);
        DirectoryInfo dir = new DirectoryInfo(fullPath);
        if(dir != null)
        {
            //Debug.Log("replacePath == " + replacePath);
            ListFiles(dir, replacePath, theWriter);
        } else
        {
            Debug.Log("the path is not exist");
        }
    }
    //遍历场景中的每一个功能文件夹
    public static void ListFiles(FileSystemInfo info, string replacePath, Dictionary<string, string> theWriter)
    {
        if(!info.Exists)
        {
            Debug.Log("is not exist");
            return;
        }
        DirectoryInfo dir = info as DirectoryInfo;
        FileSystemInfo[] files = dir.GetFileSystemInfos();
        for (int i = 0; i < files.Length; ++i)
        {
            FileInfo file = files[i] as FileInfo;
            //对于文件的操作
            if(file != null)
            {
                ChangerMark(file, replacePath, theWriter);
            } else//对于目录的操作
            {
                ListFiles(files[i], replacePath, theWriter);
            }
        }
    }

    public static string FixedWindowsPath(string path)
    {
        path = path.Replace("\\", "/");
        return path;
    }

    //计算mart标记值等于多少
    //string path = Application.dataPath + "/Art/Scenes"; 全是  右斜
    public static string GetBundlePath(FileInfo file, string replacePath)
    {
        //E:\\tmp\test.txt
        string tmpPath = file.FullName;
        Debug.Log("tmpPath == " + tmpPath);
        Debug.Log("replacePath == " + replacePath);
        tmpPath = FixedWindowsPath(tmpPath);
        //Assets/Art/Scenes/   SceneOne   //load
        int assetCount = tmpPath.IndexOf(replacePath);
        //Debug.Log("assetCount == " + assetCount);
        assetCount += replacePath.Length + 1;
        Debug.Log("file.Name == " + file.Name);
        int nameCount = tmpPath.LastIndexOf(file.Name);
        int tmpLength = nameCount - assetCount;
        int tmpCount = replacePath.LastIndexOf("/");
        //Debug.Log("nameCount == " + nameCount);
        //Debug.Log("tmpCount == " + tmpCount);
        string sceneHead = replacePath.Substring(tmpCount + 1, replacePath.Length - tmpCount - 1);
        Debug.Log("sceneHead == " + sceneHead);
        if(tmpLength > 0)
        {
            string subString = tmpPath.Substring(assetCount, tmpPath.Length - assetCount);
            string[] result = subString.Split("/".ToCharArray());
            return sceneHead + "/" + result[0];
        } else
        {
            return sceneHead;
        }
    }

    public static void ChangeAssetMark(FileInfo tmpFile, string markStr, Dictionary<string, string> theWriter)
    {
        string fullPath = tmpFile.FullName;
        int assetCount = fullPath.IndexOf("Assets");
        string assetPath = fullPath.Substring(assetCount, fullPath.Length - assetCount);
        //Assets/Art/Scenes/SceneOne/TestOne.Prefab
        AssetImporter importer = AssetImporter.GetAtPath(assetPath);
        //以下是改变标记
        importer.assetBundleName = markStr;
        if(tmpFile.Extension == ".unity")
        {
            importer.assetBundleVariant = "u3d";
        } else
        {
            importer.assetBundleVariant = "ld";
        }

        // Load  -- SceneOne/load
        string modelName = "";
        string[] subMark = markStr.Split("/".ToCharArray());
        if(subMark.Length > 1)
        {
            modelName = subMark[1];
        } else
        {
            // SceneOne  -- SceneOne
            modelName = markStr;
        }
        //sceneone/load.ld
        string modelPath = markStr.ToLower() + "." + importer.assetBundleVariant;
        if(!theWriter.ContainsKey(modelName))
        {
            theWriter.Add(modelName, modelPath);
        }
    }

    //改变物体的tag
    public static void ChangerMark(FileInfo tmpFile, string replacePath, Dictionary<string, string> theWriter)
    {
        if(tmpFile.Extension == ".meta")
        {
            return;
        }
        string markStr = GetBundlePath(tmpFile, replacePath);
        Debug.Log("markStr == " + markStr);
        ChangeAssetMark(tmpFile, markStr, theWriter);
    }
}
