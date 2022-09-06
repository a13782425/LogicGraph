using Game.Logic.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using Random = System.Random;

namespace Game.Logic.Editor
{
    //逻辑图工具类
    internal static partial class LogicUtils
    {
        /// <summary>
        /// [大版本][小版本][Bug修复]
        /// 大版本 大于0 为正式版
        /// </summary>
        public const string VERSIONS = "0.0.1 A";
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

        /// <summary>
        /// 获取resource下的图片
        /// </summary>
        /// <param name="textureName"></param>
        /// <returns></returns>
        public static Texture GetTexture(string textureName)
        {
            return Resources.Load<Texture>(textureName);
        }
        /// <summary>
        /// 获取图的颜色
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static Color GetGraphColor(Type type)
        {
            Random random = new Random(type.FullName.GetHashCode());
            float h = (float)random.NextDouble();
            return Color.HSVToRGB(h, 0.6f, 1);
        }

        /// <summary>
        /// 初始化逻辑图编辑器数据
        /// </summary>
        internal static GraphEditorData InitGraphEditorData(BaseLogicGraph graph)
        {
            var editorData = graph.GetEditorData();
            if (editorData == null)
            {
                editorData = new GraphEditorData();
                editorData.LogicName = graph.name;
                graph.SetEditorData(editorData);
            }
            return editorData;
        }

        /// <summary>
        /// 删除逻辑图
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        internal static bool RemoveGraph(BaseLogicGraph graph)
        {
            return RemoveGraph(AssetDatabase.GetAssetPath(graph));
        }
        /// <summary>
        /// 删除逻辑图
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        internal static bool RemoveGraph(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                AssetDatabase.Refresh();
            }
            return true;
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
        [MenuItem("Framework/逻辑图/打开逻辑图1", priority = 99)]
        private static void OpenLogicWindow1()
        {
            LGCatalogCache cache = LogicProvider.LGCatalogList[0];
            BaseLogicGraph graph = AssetDatabase.LoadAssetAtPath<BaseLogicGraph>(cache.AssetPath);
            var editor = graph.GetEditorData();
            graph.SetEditorData(editor);
        }

        private static bool UnityQuit()
        {
            //EditorUtility.DisplayDialog("我就是不让你关闭unity", "我就是不让你关闭unity", "呵呵");
            return true; //return true表示可以关闭unity编辑器
        }
    }
}