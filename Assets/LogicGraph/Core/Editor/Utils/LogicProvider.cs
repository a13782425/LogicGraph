using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace Game.Logic.Editor
{
    public static class LogicProvider
    {
        private static List<LGEditorCache> _lgEditorList = new List<LGEditorCache>();
        /// <summary>
        /// 逻辑图模板缓存
        /// </summary>
        public static List<LGEditorCache> LGEditorList => _lgEditorList;
        static LogicProvider()
        {
            BuildGraphCache();

        }

        /// <summary>
        /// 生成逻辑图编辑器信息缓存
        /// </summary>
        private static void BuildGraphCache()
        {
            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<LogicGraphView>();
            Debug.LogError(types.Count);

            //循环查询逻辑图
            foreach (var item in types)
            {
                //如果当前类型是逻辑图
                var graphAttr = item.GetCustomAttribute<LogicGraphAttribute>();
                if (graphAttr != null)
                {
                    Debug.LogError(graphAttr.LogicName);
                    LGEditorCache graphData = new LGEditorCache();
                    graphData.GraphClassName = graphAttr.GraphType.FullName;
                    graphData.GraphViewClassName = item.FullName;
                    //graphData.DefaultNodes.Clear();
                    //graphData.DefaultNodeFullNames = graphAttr.DefaultNodes.Select(a => a.FullName).ToList();
                    graphData.GraphName = graphAttr.LogicName;
                    //graphData.GraphType = graphAttr.GraphType;
                    //graphData.ViewType = item;
                    LGEditorList.Add(graphData);
                }
            }

        }
    }
}
