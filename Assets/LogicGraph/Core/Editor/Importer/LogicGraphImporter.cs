using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 每次资源变化调用
    /// </summary>
    internal sealed class LogicGraphImporter : AssetPostprocessor
    {
        /// <summary>
        /// 所有的资源的导入，删除，移动，都会调用此方法，注意，这个方法是static的
        /// </summary>
        /// <param name="importedAsset">导入或者发生改变的资源</param>
        /// <param name="deletedAssets">删除的资源</param>
        /// <param name="movedAssets">移动后资源路径</param>
        /// <param name="movedFromAssetPaths">移动前资源路径</param>
        public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            bool hasAsset = false;
            LogicAssetsChangedEventArgs refreshViewEvent = new LogicAssetsChangedEventArgs();
            foreach (var str in movedFromAssetPaths)
            {
                //移动前资源路径
            }
            foreach (var str in movedAssets)
            {
                //移动后资源路径
                if (Path.GetExtension(str) == ".asset")
                {
                    var logicGraph = AssetDatabase.LoadAssetAtPath<BaseLogicGraph>(str);
                    if (logicGraph != null)
                    {
                        refreshViewEvent.moveGraphs.Add(str);
                        bool res = LogicProvider.MoveGraphToSummary(str, logicGraph);
                        if (!hasAsset)
                            hasAsset = res;
                    }
                }
            }
            foreach (string str in importedAsset)
            {
                //导入或者发生改变的资源
                if (Path.GetExtension(str) == ".asset")
                {
                    var logicGraph = AssetDatabase.LoadAssetAtPath<BaseLogicGraph>(str);
                    if (logicGraph != null)
                    {
                        LGSummaryInfo summary = LogicProvider.GetSummaryInfo(logicGraph.OnlyId);
                        if (summary != null)
                        {
                            //当前逻辑图ID已存在,需要判断路径是否相同
                            if (summary.AssetPath == str)
                            {
                                //如果路径相同则不操作
                                continue;
                            }
                        }
                        refreshViewEvent.addGraphs.Add(str);
                        bool res = LogicProvider.AddGraphToSummary(str, logicGraph);
                        if (res)
                            EditorUtility.SetDirty(logicGraph);
                        if (!hasAsset)
                            hasAsset = res;
                    }
                }
            }
            foreach (string str in deletedAssets)
            {
                if (Path.GetExtension(str) == ".asset")
                {
                    refreshViewEvent.deletedGraphs.Add(str);
                    bool res = LogicProvider.DeleteGraphToSummary(str);
                    if (!hasAsset)
                        hasAsset = res;
                }
            }
            if (hasAsset)
            {
                Debug.LogError("有文件发生改变");
                AssetDatabase.SaveAssets();
                LogicUtils.OnEvent(LogicEventId.LOGIC_ASSETS_CHANGED, refreshViewEvent);
            }
        }
    }
}
