using Game.Logic.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
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
        /// 编辑器路径
        /// </summary>
        public readonly static string EDITOR_RESOURCE_PATH;
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
        /// <summary>
        /// 全局事件管理器
        /// </summary>
        private readonly static LogicMessage _logicMessage;
        static LogicUtils()
        {
            Type lgType = typeof(LGWindow);
            string[] guids = AssetDatabase.FindAssets(lgType.Name);
            foreach (var item in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(item);
                MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                if (monoScript != null && monoScript.GetClass() == lgType)
                {
                    EDITOR_PATH = Path.GetDirectoryName(path);
                    break;
                }
            }
            EDITOR_STYLE_PATH = Path.Combine(EDITOR_PATH, "Style");
            EDITOR_RESOURCE_PATH = Path.Combine(EDITOR_PATH, "_Resources");
            PORT_CUBE = "cube";
            MIN_SIZE = new Vector2(640, 360);
            _logicMessage = new LogicMessage();
        }
    }

    //公共方法
    partial class LogicUtils
    {
        /// <summary>
        /// 加载_Resources文件夹下的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Load<T>(string path) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(Path.Combine(EDITOR_RESOURCE_PATH, path));
        }

        /// <summary>
        /// 加载逻辑图
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadGraph<T>(string path) where T : BaseLogicGraph
        {
            if (string.IsNullOrEmpty(path)) return null;
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
        /// <summary>
        /// 注册一个全局事件
        /// 同一个事件回调在一个事件ID中只能注册一次
        /// </summary>
        /// <param name="messageId">事件ID</param>
        /// <param name="callback">事件回调</param>
        public static void AddListener(int messageId, MessageEventHandler callback) => _logicMessage.AddListener(messageId, callback);
        /// <summary>
        /// 移除一个全局事件
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="callback"></param>
        public static void RemoveListener(int messageId, MessageEventHandler callback) => _logicMessage.RemoveListener(messageId, callback);
        /// <summary>
        /// 派发一个全局事件
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="args"></param>
        public static void OnEvent(int messageId, object args = null) => _logicMessage.OnEvent(messageId, args);
    }

    /// <summary>
    /// 内部方法
    /// </summary>
    partial class LogicUtils
    {
        internal static string FormatTime(DateTime time)
        {
            return time.ToString("yyyy.MM.dd HH:mm");
        }
        /// <summary>
        /// 获取图的颜色
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static Color GetColor(Type type)
        {
            int temp = type.FullName.GetHashCode();
            int count = Math.Abs(temp) % 10;
            Random random = new Random(temp);
            float h = 0;
            for (int i = 0; i < count; i++)
            {
                h = (float)random.NextDouble();
            }
            return Color.HSVToRGB(h, 0.3f, 1);
        }

        /// <summary>
        /// 卸载一个UnityObject
        /// </summary>
        /// <param name="obj"></param>
        internal static void UnloadObject(UnityEngine.Object obj)
        {
            if (obj != null)
            {
                Resources.UnloadAsset(obj);
            }
        }

        /// <summary>
        /// 获取逻辑图数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static (BaseLogicGraph graph, GraphEditorData editorData) GetLogicGraph(string path)
        {
            BaseLogicGraph graph = AssetDatabase.LoadAssetAtPath<BaseLogicGraph>(path);
            if (graph != null)
            {
                return (graph, graph.GetEditorData());
            }
            return (null, null);
        }

        /// <summary>
        /// 创建逻辑图 
        /// </summary>
        /// <param name="graphType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static BaseLogicGraph CreateLogicGraph(Type graphType, string path)
        {
            BaseLogicGraph graph = ScriptableObject.CreateInstance(graphType) as BaseLogicGraph;
            string file = Path.GetFileNameWithoutExtension(path);
            graph.name = file;
            var editorData = graph.GetEditorData();
            editorData.LogicName = file;
            editorData.CreateTime = DateTime.Now;
            var f = new FloatVariable();
            f.Name = "test";
            f.Value = 10;
            graph.Variables.Add(f);
            graph.SetEditorData(editorData);
            AssetDatabase.CreateAsset(graph, path);
            AssetDatabase.Refresh();
            return graph;
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
            Debug.LogError(NodeElementUtils.InputElementMapping.Count);
            LGWindow.OpenWindow();
        }
        [OnOpenAsset(0)]
        public static bool OnBaseGraphOpened(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID) as BaseLogicGraph;

            if (asset != null)
            {
                LGWindow.OpenWindow(asset.OnlyId);
                return true;
            }
            return false;
        }
        private static bool UnityQuit()
        {
            //EditorUtility.DisplayDialog("我就是不让你关闭unity", "我就是不让你关闭unity", "呵呵");
            return true; //return true表示可以关闭unity编辑器
        }
    }
}