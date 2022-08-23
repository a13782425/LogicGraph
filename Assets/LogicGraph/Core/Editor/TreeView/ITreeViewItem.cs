using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public interface ITreeViewItem
    {
        int id { get; }
        string name { get; set; }
        object userData { get; set; }
        TreeView treeView { get; }
        VisualElement visualElement { get; }
        Action<ITreeViewItem> onClick { get; }

        ITreeViewItem parent { get; }

        IEnumerable<ITreeViewItem> children { get; }

        bool hasChildren { get; }

        void AddChild(ITreeViewItem child);

        void AddChildren(IList<ITreeViewItem> children);

        void RemoveChild(ITreeViewItem child);
    }

}
