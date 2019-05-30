using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PythonTools {

    [MenuItem("Itools/TestPython1")]
    public static void TestPython1()
    {
        string command = "/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal";
        string shell = Application.dataPath.Replace("Assets", "Itools") + "/test.sh";
        string arg1 = "unity";
        string arg2 = Application.dataPath.Replace("Assets", "Itools") + "/test.log";
        string argss = shell + " " + arg1 + " " + arg2;
        //有人说把空格  和 全部 用冒号 括起来， 但是还是没能成功。
        //string argss =  "\"" + shell +" "+ arg1 +" " + arg2 +"\"";
        //string argss =  shell +"\" \""+ arg1 +"\" \""+ arg2;
        //string argss =  "\"" + shell +"\" \""+ arg1 +"\" \""+ arg2+"\"";
        System.Diagnostics.Process.Start(command, argss);
        UnityEngine.Debug.Log(argss);    
    }

    [MenuItem("Itools/TestPython2")]
    public static void TestPython2()
    {
        string shell = Application.dataPath.Replace("Assets", "Itools") + "/test.sh";
        Debug.Log(shell);
        string arg1 = "unity";
        string arg2 = Application.dataPath.Replace("Assets", "Itools") + "/test.log";
        string argss = shell + " " + arg1 + " " + arg2;
        System.Diagnostics.Process.Start("/bin/bash", argss);
    }
}
