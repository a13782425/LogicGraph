using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 逻辑图目录缓存
    /// </summary>
    public class LGCatalogCache
    {
        public string OnlyId;
        /// <summary>
        /// 逻辑图名
        /// </summary>
        public string LogicName => EditorData.LogicName;
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
        /// 当前的逻辑图
        /// </summary>
        public GraphEditorData EditorData;
    }

    /// <summary>
    /// 逻辑图编辑器信息缓存
    /// 用来记录逻辑图的编辑器数据
    /// </summary>
    [Serializable]
    public sealed class LGEditorCache
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

    }

}
