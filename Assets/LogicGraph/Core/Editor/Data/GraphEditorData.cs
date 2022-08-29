﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Logic.Editor
{
    [Serializable]
    public sealed class GraphEditorData
    {
        /// <summary>
        /// 当前图的节点编辑器数据
        /// </summary>
        [SerializeField]
        public List<NodeEditorData> NodeDatas = new List<NodeEditorData>();
        /// <summary>
        /// 当前图的分组编辑器数据
        /// </summary>
        [SerializeField]
        public List<GroupEditorData> GroupDatas = new List<GroupEditorData>();
        /// <summary>
        /// 最后一次Format位置
        /// </summary>
        [SerializeField]
        public string LastFormatPath;
        /// <summary>
        /// 当前图坐标
        /// </summary>
        [SerializeField]
        public Vector3 Pos = Vector3.zero;
        /// <summary>
        /// 当前图的缩放
        /// </summary>
        [SerializeField]
        public Vector3 Scale = Vector3.one;
    }
}
