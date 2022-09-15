using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// 逻辑图唯一ID的字段
        /// </summary>
        readonly static FieldInfo GRAPH_ONLY_ID = typeof(BaseLogicGraph).GetField("_onlyId", BindingFlags.NonPublic | BindingFlags.Instance);
        /// <summary>
        /// 逻辑图编辑器数据的字段
        /// </summary>
        readonly static FieldInfo GRAPH_EDIOTR_DATA = typeof(BaseLogicGraph).GetField("__editorData", BindingFlags.NonPublic | BindingFlags.Instance);
        /// <summary>
        /// 获取逻辑图编辑器数据
        /// 如果没有逻辑图数据则初始化新的
        /// </summary>
        /// <param name="logicPath"></param>
        /// <returns></returns>
        public static GraphEditorData GetEditorData(this BaseLogicGraph graph)
        {
            GraphEditorData data = null;
            if (GRAPH_EDIOTR_DATA != null)
            {
                string str = (string)GRAPH_EDIOTR_DATA.GetValue(graph);
                if (string.IsNullOrWhiteSpace(str))
                {
                    data = new GraphEditorData();
                    data.LogicName = graph.name;
                    SetEditorData(graph, data);
                }
                else
                {
                    data = JsonUtility.FromJson<GraphEditorData>(str);
                }
            }
            else
            {
                data = new GraphEditorData();
                data.LogicName = graph.name;
                Debug.LogError("没有找到编辑器数据字段");
            }
            return data;
        }
        /// <summary>
        /// 设置逻辑图编辑器数据
        /// </summary>
        /// <param name="logicPath"></param>
        /// <returns></returns>
        public static bool SetEditorData(this BaseLogicGraph graph, GraphEditorData editorData)
        {
            if (GRAPH_EDIOTR_DATA != null)
            {
                editorData.ModifyTime = DateTime.Now;
                string json = JsonUtility.ToJson(editorData);
                GRAPH_EDIOTR_DATA.SetValue(graph, json);
            }
            else
            {
                Debug.LogError("没有找到编辑器数据字段");
            }
            return false;
        }
        /// <summary>
        /// 重置唯一ID
        /// </summary>
        /// <param name="graph"></param>
        public static void ResetGUID(this BaseLogicGraph graph)
        {
            if (GRAPH_ONLY_ID != null)
            {
                GRAPH_ONLY_ID.SetValue(graph, Guid.NewGuid().ToString());
            }
            else
            {
                Debug.LogError("没有找到字段");
            }
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
