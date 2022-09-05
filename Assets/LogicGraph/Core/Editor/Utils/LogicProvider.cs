using Game.Logic.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
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
                    LGEditorList.Add(graphData);
                }
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

    }
}
