using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Game.Logic.Editor
{
    internal static class LogicExtensions
    {
        /// <summary>
        /// 获取逻辑图编辑器数据
        /// </summary>
        /// <param name="logicPath"></param>
        /// <returns></returns>
        public static GraphEditorData GetEditorData(this BaseLogicGraph graph)
        {
            AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(graph));
            Debug.LogError(importer);
            return importer == null ? null : JsonUtility.FromJson<GraphEditorData>(importer.userData);
        }
        /// <summary>
        /// 设置逻辑图编辑器数据
        /// </summary>
        /// <param name="logicPath"></param>
        /// <returns></returns>
        public static bool SetEditorData(this BaseLogicGraph graph, GraphEditorData editorData)
        {
            AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(graph));
            if (importer == null)
            {
                return false;
            }
            editorData.ModifyTime = DateTime.Now.ToString("yyyy.MM.dd");

            importer.userData = JsonUtility.ToJson(editorData);
            importer.SaveAndReimport();
            return true;
        }
        /// <summary>
        /// 根据窗体获取位置
        /// </summary>
        /// <param name="window"></param>
        /// <param name="localPos">本地位置</param>
        public static Vector2 GetScreenPosition(this UnityEditor.EditorWindow window, Vector2 localPos)
        {
            return window.position.position + localPos;
        }

        /// <summary>
        /// 重置唯一ID
        /// </summary>
        /// <param name="graph"></param>
        public static void ResetGUID(this BaseLogicGraph graph)
        {
            var field = typeof(BaseLogicGraph).GetField("_onlyId", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                field.SetValue(graph, Guid.NewGuid().ToString());
            }
            else
            {
                Debug.LogError("没有找到字段");
            }
        }
    }
}
