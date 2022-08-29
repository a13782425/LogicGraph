using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Logic.Editor
{
    [Serializable]
    public sealed class NodeEditorData
    {
        [NonSerialized]
        public BaseLogicNode node;

        [SerializeField]
        public Vector2 Pos = Vector2.zero;
        /// <summary>
        /// 是否上锁
        /// </summary>
        [SerializeField]
        public bool IsLock = false;
        /// <summary>
        /// 节点描述
        /// </summary>
        [SerializeField]
        public string Describe = "";

    }
}
