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
    [ScriptedImporter(0, ".logic")]
    internal class LogicGraphImporter : ScriptedImporter
    {
        public string cache = "";

        private void createGraph(AssetImportContext ctx)
        {
            File.Delete(ctx.assetPath);
            BaseLogicGraph graph = ScriptableObject.CreateInstance<BaseLogicGraph>();
            AssetDatabase.CreateAsset(graph, ctx.assetPath);
            AssetDatabase.Refresh();
        }
        [MenuItem("Assets/Create/test")]
        private static void abc()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!Directory.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }
            BaseLogicGraph graph = ScriptableObject.CreateInstance<BaseLogicGraph>();

            AssetDatabase.CreateAsset(graph, Path.Combine(path, "abc.asset"));
            AssetDatabase.Refresh();
            var a = AssetImporter.GetAtPath(Path.Combine(path, "abc.asset"));
            a.userData = "ddd";
            a.SaveAndReimport();
        }
        [MenuItem("Assets/test")]
        private static void abc1()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var a = AssetImporter.GetAtPath(path);
            
            Debug.LogError(a.userData);


        }
        public override void OnImportAsset(AssetImportContext ctx)
        {
            try
            {
                BaseLogicGraph graphData = AssetDatabase.LoadAssetAtPath<BaseLogicGraph>(ctx.assetPath);
                if (graphData == null)
                {
                    graphData = ScriptableObject.CreateInstance<BaseLogicGraph>();

                    ctx.AddObjectToAsset("main", graphData);
                    ctx.SetMainObject(graphData);
                    //File.Delete(ctx.assetPath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
    }
}
