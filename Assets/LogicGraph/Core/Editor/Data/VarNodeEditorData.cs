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
    internal sealed class VarNodeEditorData
    {
        /// <summary>
        /// 当前变量节点是那个变量的
        /// </summary>
        public VarEditorData owner { get; set; }

        //[SerializeField]
        //private int a = 0;
        ///// <summary>
        ///// 变量节点的ID
        ///// </summary>
        //public int OnlyId { get => a; set => a = value; }

        [SerializeField]
        private Vector2 b;
        public Vector2 Pos { get => b; set => b = value; }


        [SerializeField]
        private List<VarEdgeEditorData> c = new List<VarEdgeEditorData>();
        public List<VarEdgeEditorData> Edges => c;
        [SerializeField]
        private string d = "";
        public string Name { get => d; set => d = value; }
    }
}
