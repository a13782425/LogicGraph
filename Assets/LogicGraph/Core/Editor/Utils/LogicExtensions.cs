using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public static GraphEditorData GetEditorData(this BaseLogicGraph graph, string logicPath)
        {
            AssetImporter importer = AssetImporter.GetAtPath(logicPath);
            return importer == null ? null : JsonUtility.FromJson<GraphEditorData>(importer.userData);
        }
        /// <summary>
        /// 设置逻辑图编辑器数据
        /// </summary>
        /// <param name="logicPath"></param>
        /// <returns></returns>
        public static bool SetEditorData(this BaseLogicGraph graph, string logicPath, GraphEditorData editorData)
        {
            AssetImporter importer = AssetImporter.GetAtPath(logicPath);
            if (importer == null)
            {
                return false;
            }
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
    }
}
