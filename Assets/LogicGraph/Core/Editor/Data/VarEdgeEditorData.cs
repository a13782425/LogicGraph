using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 变量节点连线数据
    /// </summary>
    [Serializable]
    internal class VarEdgeEditorData
    {
        [SerializeField]
        private int a = 0;
        /// <summary>
        /// 连接的节点Id
        /// </summary>
        public int NodeId { get => a; set => a = value; }
        [SerializeField]
        private string b = "";
        /// <summary>
        /// 连接到节点的变量
        /// </summary>
        public string NodeFieldName { get => b; set => b = value; }
        [SerializeField]
        private bool c = false;
        /// <summary>
        /// 是否是进入
        /// </summary>
        public bool IsIn { get => c; set => c = value; }
    } 
}