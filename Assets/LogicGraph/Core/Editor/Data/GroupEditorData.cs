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
        private string a = "New Group";
        public string Title { get => a; set => a = value; }
        [SerializeField]
        private Color b = new Color(0, 0, 0, 0.3f);
        public Color Color { get => b; set => b = value; }
        [SerializeField]
        private Vector2 c;
        public Vector2 Pos { get => c; set => c = value; }
        [SerializeField]
        private Vector2 d;
        public Vector2 Size { get => d; set => d = value; }
        [SerializeField]
        private List<int> e = new List<int>();
        public List<int> Nodes => e;
    }
}
