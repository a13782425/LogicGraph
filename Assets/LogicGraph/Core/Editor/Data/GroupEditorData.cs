using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Logic.Editor
{
    [Serializable]
    public sealed class GroupEditorData
    {
        [SerializeField]
        public string Title = "New Group";
        [SerializeField]
        public Color Color = new Color(0, 0, 0, 0.3f);
        [SerializeField]
        public Vector2 Pos;
        [SerializeField]
        public Vector2 Size;
        [SerializeField]
        public List<int> Nodes = new List<int>();
    }
}
