using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Logic.Editor
{
    static class LGMenuItem
    {
        [InitializeOnLoadMethod]
        static void InitializeOnLoadMethod()
        {
            EditorApplication.wantsToQuit -= UnityQuit;
            EditorApplication.wantsToQuit += UnityQuit;
        }

        //[MenuItem("Framework/逻辑图/打开逻辑图", priority = 99)]
        //private static void OpenLogicWindow()
        //{
        //    Debug.LogError("sadasd");
        //}

        private static bool UnityQuit()
        {
            //EditorUtility.DisplayDialog("我就是不让你关闭unity", "我就是不让你关闭unity", "呵呵");
            return true; //return true表示可以关闭unity编辑器
        }
    }
}
