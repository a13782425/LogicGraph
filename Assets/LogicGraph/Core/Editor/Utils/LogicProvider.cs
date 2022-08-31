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
    }
}
