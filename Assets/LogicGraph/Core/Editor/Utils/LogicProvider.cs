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
        private static List<LGCategoryInfo> _lgCategoryList = new List<LGCategoryInfo>();
        /// <summary>
        /// 逻辑图分类信息缓存
        /// </summary>
        public static List<LGCategoryInfo> LGCategoryList => _lgCategoryList;


        private static List<LGSummaryInfo> _lgSummaryList = new List<LGSummaryInfo>();
        /// <summary>
        /// 逻辑图对象的简介和编辑器信息缓存
        /// </summary>
        public static List<LGSummaryInfo> LGSummaryList => _lgSummaryList;

        /// <summary>
        /// 获取逻辑图的简介和编辑器信息
        /// </summary>
        /// <param name="logic"></param>
        /// <returns></returns>
        public static LGSummaryInfo GetSummaryInfo(BaseLogicGraph logic) => GetSummaryInfo(logic.OnlyId);
        /// <summary>
        /// 获取逻辑图的简介和编辑器信息
        /// </summary>
        /// <param name="onlyId"></param>
        /// <returns></returns>
        public static LGSummaryInfo GetSummaryInfo(string onlyId) => LGSummaryList.FirstOrDefault(a => a.OnlyId == onlyId);

        /// <summary>
        /// 获得对应逻辑图的分类信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static LGCategoryInfo GetCategoryInfo(LGSummaryInfo info) => GetCategoryInfo(info.GraphClassName);
        /// <summary>
        /// 获得对应逻辑图的分类信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static LGCategoryInfo GetCategoryInfo(BaseLogicGraph info) => GetCategoryInfo(info.GetType().FullName);
        /// <summary>
        /// 获得对应逻辑图的分类信息
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static LGCategoryInfo GetCategoryInfo(string className) => LGCategoryList.FirstOrDefault(a => a.GraphType.FullName == className);

        static LogicProvider()
        {
            m_buildGraphCache();
            m_buildFormat();
            m_buildNode();
            m_buildGraphSummary();
        }





        /// <summary>
        /// 生成逻辑图编辑器信息缓存
        /// </summary>
        private static void m_buildGraphCache()
        {
            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<BaseGraphView>();

            //循环查询逻辑图
            foreach (var item in types)
            {
                //如果当前类型是逻辑图
                var graphAttr = item.GetCustomAttribute<LogicGraphAttribute>();
                if (graphAttr != null)
                {
                    LGCategoryInfo graphData = new LGCategoryInfo();
                    graphData.GraphType = graphAttr.GraphType;
                    graphData.ViewType = item;
                    graphData.GraphName = graphAttr.LogicName;
                    graphData.GraphColor = graphAttr.Color.HasValue ? graphAttr.Color.Value : LogicUtils.GetColor(item);
                    LGCategoryList.Add(graphData);
                }
            }
            LGCategoryList.Sort((a, b) =>
            {
                if (a.GraphType.FullName.GetHashCode() < b.GraphType.FullName.GetHashCode())
                    return -1;
                else if (a.GraphType.FullName.GetHashCode() > b.GraphType.FullName.GetHashCode())
                    return 1;
                return 0;
            });
            for (int i = 0; i < LGCategoryList.Count; i++)
            {
                LGCategoryList[i].Index = i;
            }
        }
        /// <summary>
        /// 生成导出信息缓存
        /// </summary>
        private static void m_buildFormat()
        {
            List<MethodInfo> methodInfos = TypeCache.GetMethodsWithAttribute<GraphFormatAttribute>().ToList();
            Type retuenType = typeof(bool);
            Type paramType1 = typeof(BaseLogicGraph);
            Type paramType2 = typeof(string);
            foreach (var item in methodInfos)
            {
                if (!item.IsStatic)
                {
                    Debug.LogError($"方法:{item.Name}不是静态方法");
                    continue;
                }
                if (item.ReturnType != retuenType)
                {
                    Debug.LogError($"方法:{item.Name}返回类型错误,返回类型应为:bool");
                    continue;
                }

                ParameterInfo[] @params = item.GetParameters();

                if (@params.Length != 2)
                {
                    Debug.LogError($"方法:{item.Name}参数数量错误,参数数量应为:2");
                    continue;
                }
                ParameterInfo info1 = @params[0];
                ParameterInfo info2 = @params[1];
                if (info1.ParameterType != paramType1)
                {
                    Debug.LogError($"方法:{item.Name}第一个参数类型错误,第一个参数类型应为:BaseLogicGraph");
                    continue;
                }
                if (info2.ParameterType != paramType2)
                {
                    Debug.LogError($"方法:{item.Name}第二个参数类型错误,第二个参数类型应为:string");
                    continue;
                }
                GraphFormatAttribute formatAttr = item.GetCustomAttribute<GraphFormatAttribute>();
                LGCategoryInfo categoryInfo = GetCategoryInfo(formatAttr.GraphType.FullName);
                if (categoryInfo != null)
                {
                    LogicFormatCategory formatCache = new LogicFormatCategory();
                    formatCache.FormatName = formatAttr.LogicName;
                    formatCache.Extension = formatAttr.Extension;
                    formatCache.Method = item;
                    categoryInfo.Formats.Add(formatCache);
                }
            }
        }
        /// <summary>
        /// 生成导出节点缓存
        /// </summary>
        private static void m_buildNode()
        {
            List<Type> nodeViewTypes = TypeCache.GetTypesDerivedFrom<BaseNodeView>().ToList();
            foreach (Type nodeViewType in nodeViewTypes)
            {
                var nodeAttr = nodeViewType.GetCustomAttribute<LogicNodeAttribute>();
                if (nodeAttr != null)
                {
                    if (!nodeAttr.IsEnable)
                        continue;
                    LogicNodeCategory nodeCategory = new LogicNodeCategory();
                    string[] strs = nodeAttr.MenuText.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    nodeCategory.NodeLayers = strs;
                    nodeCategory.NodeName = strs[strs.Length - 1];
                    nodeCategory.PortType = nodeAttr.PortType;
                    nodeCategory.NodeType = nodeAttr.NodeType;
                    nodeCategory.ViewType = nodeViewType;
                    m_praseNodeField(nodeCategory);
                    LGCategoryList.ForEach(a =>
                    {
                        if (nodeAttr.HasType(a.GetType()))
                            a.Nodes.Add(nodeCategory);
                    });
                }
            }
            foreach (LGCategoryInfo item in LGCategoryList)
            {
                item.Nodes.Sort((entry1, entry2) =>
                {
                    for (var i = 0; i < entry1.NodeLayers.Length; i++)
                    {
                        if (i >= entry2.NodeLayers.Length)
                            return 1;
                        var value = entry1.NodeLayers[i].CompareTo(entry2.NodeLayers[i]);
                        if (value != 0)
                        {
                            // Make sure that leaves go before nodes
                            if (entry1.NodeLayers.Length != entry2.NodeLayers.Length && (i == entry1.NodeLayers.Length - 1 || i == entry2.NodeLayers.Length - 1))
                                return entry1.NodeLayers.Length < entry2.NodeLayers.Length ? -1 : 1;
                            return value;
                        }
                    }
                    return 0;
                });
            }
        }

        /// <summary>
        /// 解析节点内的字段信息
        /// </summary>
        /// <param name="nodeCategory"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static void m_praseNodeField(LogicNodeCategory nodeCategory)
        {
            FieldInfo[] fields = nodeCategory.NodeType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                nodeCategory.FieldInfos.Add(field.Name, field);
            }
        }

        /// <summary>
        /// 生成逻辑图信息缓存
        /// </summary>
        private static void m_buildGraphSummary()
        {
            HashSet<string> hashKey = new HashSet<string>();
            LGSummaryList.Clear();
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
                LGSummaryInfo graphCache = new LGSummaryInfo();
                graphCache.GraphClassName = logicGraph.GetType().FullName;
                graphCache.AssetPath = assetPath;
                graphCache.OnlyId = logicGraph.OnlyId;
                graphCache.SetData(editorData);

                hashKey.Add(logicGraph.OnlyId);
                LGSummaryList.Add(graphCache);
            }
        }
        /// <summary>
        /// 在简介中移动逻辑图
        /// </summary>
        /// <param name="path"></param>
        /// <param name="logicGraph"></param>

        internal static bool MoveGraphToSummary(string path, BaseLogicGraph logicGraph)
        {
            var catalog = LGSummaryList.FirstOrDefault(a => a.OnlyId == logicGraph.OnlyId);
            if (catalog != null)
                catalog.AssetPath = path;
            return true;
        }
        /// <summary>
        /// 在简介中新增逻辑图
        /// </summary>
        /// <param name="path"></param>
        /// <param name="logicGraph"></param>
        internal static bool AddGraphToSummary(string path, BaseLogicGraph logicGraph)
        {
            logicGraph.ResetGUID();
            GraphEditorData editorData = logicGraph.GetEditorData();
            editorData.LogicName = logicGraph.name;
            editorData.CreateTime = DateTime.Now;
            logicGraph.SetEditorData(editorData);
            LGSummaryInfo graphCache = new LGSummaryInfo();
            graphCache.GraphClassName = logicGraph.GetType().FullName;
            graphCache.AssetPath = path;
            graphCache.OnlyId = logicGraph.OnlyId;
            graphCache.LogicName = editorData.LogicName;
            graphCache.SetData(editorData);
            LGSummaryList.Add(graphCache);
            return true;
        }
        /// <summary>
        /// 在简介中删除逻辑图
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static bool DeleteGraphToSummary(string path)
        {
            var catalog = LGSummaryList.FirstOrDefault(a => a.AssetPath == path);
            if (catalog != null)
                LGSummaryList.Remove(catalog);
            return true;
        }
    }
}
