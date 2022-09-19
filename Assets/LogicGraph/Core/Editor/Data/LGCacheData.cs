using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 单个逻辑图对象的简介和编辑器信息
    /// </summary>
    public class LGSummaryInfo
    {
        public string OnlyId;
        /// <summary>
        /// 逻辑图名
        /// </summary>
        public string LogicName;
        /// <summary>
        /// 图类型名全称,含命名空间
        /// </summary>
        public string GraphClassName;
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName;
        /// <summary>
        /// 资源路径
        /// </summary>
        public string AssetPath;
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime;
        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifyTime;

        internal void SetData(GraphEditorData editorData)
        {
            this.LogicName = editorData.LogicName;
            this.CreateTime = LogicUtils.FormatTime(editorData.CreateTime);
            this.ModifyTime = LogicUtils.FormatTime(editorData.ModifyTime);
        }
    }

    /// <summary>
    /// 逻辑图分类信息
    /// 针对一类逻辑图公用信息的缓存
    /// </summary>
    [Serializable]
    public sealed class LGCategoryInfo
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 图名
        /// </summary>
        public string GraphName { get; set; }
        /// <summary>
        /// 图的颜色
        /// </summary>
        internal Color GraphColor { get; set; }
        /// <summary>
        /// 逻辑图类型
        /// </summary>
        public Type GraphType { get; set; }
        /// <summary>
        /// 视图类型
        /// </summary>
        public Type ViewType { get; set; }

        private List<LogicFormatCategory> _formats = new List<LogicFormatCategory>();
        /// <summary>
        /// 当前逻辑图适用的格式化
        /// </summary>
        public List<LogicFormatCategory> Formats => _formats;

        private List<LogicNodeCategory> _nodes = new List<LogicNodeCategory>();
        /// <summary>
        /// 当前逻辑图所对应的节点
        /// </summary>
        public List<LogicNodeCategory> Nodes => _nodes;

        /// <summary>
        /// 获得当前节点的信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public LogicNodeCategory GetNodeCategory(Type nodeType) => Nodes.FirstOrDefault(a => a.NodeType == nodeType);
 
    }
    /// <summary>
    /// 逻辑图节点分类信息缓存
    /// </summary>
    public sealed class LogicNodeCategory
    {
        /// <summary>
        /// 节点名
        /// </summary>
        public string NodeName;
        /// <summary>
        /// 节点层级
        /// </summary>
        public string[] NodeLayers;
        /// <summary>
        /// 节点类型
        /// </summary>
        public Type NodeType { get; set; }
        /// <summary>
        /// 视图类型
        /// </summary>
        public Type ViewType { get; set; }
        /// <summary>
        /// 节点端口类型
        /// </summary>
        public PortDirEnum PortType { get; set; }
    }
    /// <summary>
    /// 逻辑图格式化缓存
    /// </summary>
    public sealed class LogicFormatCategory
    {
        /// <summary>
        /// 格式化方法
        /// </summary>
        public MethodInfo Method { get; set; }
        /// <summary>
        /// 格式化名
        /// </summary>
        public string FormatName { get; set; }
        /// <summary>
        /// 格式化后缀
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="graph">逻辑图</param>
        /// <param name="path">路径</param>
        /// <returns>是否导出成功</returns>
        public bool ToFormat(BaseLogicGraph graph, string path)
        {
            object res = Method.Invoke(null, new object[] { graph, path });
            return (bool)res;
        }
    }
}
