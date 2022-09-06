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
        /// <param name="importedAsset">导入的资源</param>
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
                        bool res = LogicProvider.MoveGraph(str, logicGraph);
                        if (!hasAsset)
                            hasAsset = res;
                    }
                }
            }
            foreach (string str in importedAsset)
            {
                if (Path.GetExtension(str) == ".asset")
                {
                    var logicGraph = AssetDatabase.LoadAssetAtPath<BaseLogicGraph>(str);
                    if (logicGraph != null)
                    {
                        if (logicGraph.GetEditorData() == null)
                        {
                            logicGraph.ResetGUID();
                            refreshViewEvent.addGraphs.Add(str);
                            bool res = LogicProvider.AddGraph(str, logicGraph);
                            if (!hasAsset)
                                hasAsset = res;
                        }
                    }
                }
            }
            foreach (string str in deletedAssets)
            {
                if (Path.GetExtension(str) == ".asset")
                {
                    refreshViewEvent.deletedGraphs.Add(str);
                    bool res = LogicProvider.DeleteGraph(str);
                    if (!hasAsset)
                        hasAsset = res;
                }
            }

            if (hasAsset)
            {
                Debug.LogError("有文件发生改变");
                LogicMessage.OnEvent(LogicEventId.LOGIC_ASSETS_CHANGED, refreshViewEvent);
            }
        }
    }
    //[ScriptedImporter(0, ".logic")]
    //internal class LogicGraphImporter : ScriptedImporter
    //{
    //    public string cache = "";

    //    private void createGraph(AssetImportContext ctx)
    //    {
    //        File.Delete(ctx.assetPath);
    //        BaseLogicGraph graph = ScriptableObject.CreateInstance<BaseLogicGraph>();
    //        AssetDatabase.CreateAsset(graph, ctx.assetPath);
    //        AssetDatabase.Refresh();
    //    }
    //    [MenuItem("Assets/Create/test")]
    //    private static void abc()
    //    {
    //        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
    //        if (!Directory.Exists(path))
    //        {
    //            path = Path.GetDirectoryName(path);
    //        }
    //        BaseLogicGraph graph = ScriptableObject.CreateInstance<BaseLogicGraph>();

    //        AssetDatabase.CreateAsset(graph, Path.Combine(path, "abc.asset"));
    //        AssetDatabase.Refresh();
    //        var a = AssetImporter.GetAtPath(Path.Combine(path, "abc.asset"));
    //        a.userData = "ddd";
    //        a.SaveAndReimport();
    //    }
    //    [MenuItem("Assets/test")]
    //    private static void abc1()
    //    {
    //        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
    //        var a = AssetImporter.GetAtPath(path);

    //        Debug.LogError(a.userData);


    //    }
    //    public override void OnImportAsset(AssetImportContext ctx)
    //    {
    //        try
    //        {
    //            BaseLogicGraph graphData = AssetDatabase.LoadAssetAtPath<BaseLogicGraph>(ctx.assetPath);
    //            if (graphData == null)
    //            {
    //                graphData = ScriptableObject.CreateInstance<BaseLogicGraph>();

    //                ctx.AddObjectToAsset("main", graphData);
    //                ctx.SetMainObject(graphData);
    //                //File.Delete(ctx.assetPath);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.LogError(ex.Message);
    //        }
    //    }
    //}
}
