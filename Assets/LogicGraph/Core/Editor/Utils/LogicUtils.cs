using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.Logic.Editor
{
    //逻辑图工具类
    internal static partial class LogicUtils
    {
        /// <summary>
        /// 编辑器路径
        /// </summary>
        public readonly static string EDITOR_PATH;
        /// <summary>
        /// 编辑器样式路径
        /// </summary>
        public readonly static string EDITOR_STYLE_PATH;

        /// <summary>
        /// 方块端口样式
        /// </summary>
        public readonly static string PORT_CUBE;

        /// <summary>
        /// 窗口最小大小
        /// </summary>
        public readonly static Vector2 MIN_SIZE;
        static LogicUtils()
        {
            Type lgType = typeof(LGWindow);
            string[] guids = AssetDatabase.FindAssets(lgType.Name);
            foreach (var item in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(item);
                MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                if (monoScript.GetClass() == lgType)
                {
                    EDITOR_PATH = Path.GetDirectoryName(path);
                    break;
                }
            }
            EDITOR_STYLE_PATH = Path.Combine(EDITOR_PATH, "Style");
            PORT_CUBE = "cube";
            MIN_SIZE = new Vector2(640, 360);
        }
    }


    //编辑器拓展
    partial class LogicUtils
    {
        [InitializeOnLoadMethod]
        static void InitializeOnLoadMethod()
        {
            //Unity退出时候调用
            EditorApplication.wantsToQuit -= UnityQuit;
            EditorApplication.wantsToQuit += UnityQuit;
        }

        [MenuItem("Framework/逻辑图/打开逻辑图", priority = 99)]
        private static void OpenLogicWindow()
        {
            LGWindow.ShowLogic();
        }

        private static bool UnityQuit()
        {
            //EditorUtility.DisplayDialog("我就是不让你关闭unity", "我就是不让你关闭unity", "呵呵");
            return true; //return true表示可以关闭unity编辑器
        }
    }
}