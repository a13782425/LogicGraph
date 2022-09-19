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
        private int a;
        public int OnlyId { get => a; set => a = value; }

        [SerializeField]
        private string b;
        public string Title { get => b; set => b = value; }

        [SerializeField]
        private Vector2 c;
        public Vector2 Pos { get => c; set => c = value; }
        /// <summary>
        /// 是否上锁
        /// </summary>
        [SerializeField]
        private bool d;
        public bool IsLock { get => d; set => d = value; }
        /// <summary>
        /// 节点描述
        /// </summary>
        [SerializeField]
        private string e;
        public string Describe { get => e; set => e = value; }

    }
}
