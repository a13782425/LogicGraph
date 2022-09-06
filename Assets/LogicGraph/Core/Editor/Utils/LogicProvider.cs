using Game.Logic.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace Game.Logic.Editor
{
    /// <summary>
    /// 对于编辑器一些信息的提供
    /// </summary>
    public static class LogicProvider
    {
        private static List<LGEditorCache> _lgEditorList = new List<LGEditorCache>();
        /// <summary>
        /// 逻辑图模板缓存
        /// </summary>
        public static List<LGEditorCache> LGEditorList => _lgEditorList;


        private static List<LGCatalogCache> _lgCatalogList = new List<LGCatalogCache>();
        /// <summary>
        /// 逻辑图目录缓存
        /// </summary>
        public static List<LGCatalogCache> LGCatalogList => _lgCatalogList;
        static LogicProvider()
        {
            BuildGraphCache();
            BuildGraphCatalog();
        }


        /// <summary>
        /// 生成逻辑图编辑器信息缓存
        /// </summary>
        private static void BuildGraphCache()
        {
            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<LogicGraphView>();

            //循环查询逻辑图
            foreach (var item in types)
            {
                //如果当前类型是逻辑图
                var graphAttr = item.GetCustomAttribute<LogicGraphAttribute>();
                if (graphAttr != null)
                {
                    LGEditorCache graphData = new LGEditorCache();
                    graphData.GraphType = graphAttr.GraphType;
                    graphData.ViewType = item;
                    graphData.GraphName = graphAttr.LogicName;
                    graphData.GraphColor = graphAttr.Color.HasValue ? graphAttr.Color.Value : LogicUtils.GetGraphColor(item);
                    LGEditorList.Add(graphData);
                }
            }
            LGEditorList.Sort((a, b) =>
            {
                if (a.GraphType.FullName.GetHashCode() < b.GraphType.FullName.GetHashCode())
                    return -1;
                else if (a.GraphType.FullName.GetHashCode() > b.GraphType.FullName.GetHashCode())
                    return 1;
                return 0;
            });
            for (int i = 0; i < LGEditorList.Count; i++)
            {
                LGEditorList[i].Index = i;
            }
        }


        /// <summary>
        /// 生成逻辑图信息缓存
        /// </summary>
        private static void BuildGraphCatalog()
        {
            HashSet<string> hashKey = new HashSet<string>();
            LGCatalogList.Clear();
            string[] guids = AssetDatabase.FindAssets("t:BaseLogicGraph");
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                BaseLogicGraph logicGraph = AssetDatabase.LoadAssetAtPath<BaseLogicGraph>(assetPath);
                if (logicGraph == null)
                    continue;
                if (hashKey.Contains(logicGraph.OnlyId))
                {
                    logicGraph.ResetGUID();
                }
                var editorData = logicGraph.GetEditorData();
                LGCatalogCache graphCache = new LGCatalogCache();
                graphCache.GraphClassName = logicGraph.GetType().FullName;
                graphCache.AssetPath = assetPath;
                graphCache.OnlyId = logicGraph.OnlyId;
                graphCache.EditorData = editorData;
                hashKey.Add(logicGraph.OnlyId);
                LGCatalogList.Add(graphCache);
            }
        }
        /// <summary>
        /// 移动逻辑图
        /// </summary>
        /// <param name="path"></param>
        /// <param name="logicGraph"></param>

        internal static bool MoveGraph(string path, BaseLogicGraph logicGraph)
        {
            var catalog = LGCatalogList.FirstOrDefault(a => a.OnlyId == logicGraph.OnlyId);
            if (catalog != null)
                catalog.AssetPath = path;
            return true;
        }
        /// <summary>
        /// 新增逻辑图
        /// </summary>
        /// <param name="path"></param>
        /// <param name="logicGraph"></param>
        internal static bool AddGraph(string path, BaseLogicGraph logicGraph)
        {
            var catalog = LGCatalogList.FirstOrDefault(a => a.OnlyId == logicGraph.OnlyId);
            if (catalog != null)
                return false;
            GraphEditorData graphEditor = LogicUtils.InitGraphEditorData(logicGraph);
            path = path.Replace(Application.dataPath, "Assets");
            LGCatalogCache graphCache = new LGCatalogCache();
            graphCache.GraphClassName = logicGraph.GetType().FullName;
            graphCache.AssetPath = path;
            graphCache.OnlyId = logicGraph.OnlyId;
            graphCache.EditorData = graphEditor;
            LGCatalogList.Add(graphCache);
            return true;
        }
        /// <summary>
        /// 删除逻辑图
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static bool DeleteGraph(string path)
        {
            var catalog = LGCatalogList.FirstOrDefault(a => a.AssetPath == path);
            if (catalog != null)
                LGCatalogList.Remove(catalog);
            return true;
        }
    }
}
