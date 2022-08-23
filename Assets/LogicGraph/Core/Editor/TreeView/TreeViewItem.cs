using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public class TreeViewItem : ITreeViewItem
    {
        public TreeViewItem _parent;

        private List<ITreeViewItem> m_Children;

        public int id { get; private set; }

        public ITreeViewItem parent => _parent;

        public IEnumerable<ITreeViewItem> children => m_Children;

        public bool hasChildren => m_Children != null && m_Children.Count > 0;

        public Action<ITreeViewItem> onClick { get; set; }
        public Action<ITreeViewItem> onDoubleClick { get; set; }

        public object userData { get; set; }
        public string name { get; set; }
        private VisualElement _visualElement;
        public VisualElement visualElement
        {
            get => _visualElement;
            internal set
            {
                if (_visualElement != null)
                {
                    _visualElement.UnregisterCallback<ClickEvent>(m_onClick);
                }
                _visualElement = value;
            }
        }
        public TreeView treeView { get; internal set; }

       

        public TreeViewItem(int id, List<ITreeViewItem> children = null)
        {
            this.id = id;
            if (children == null)
            {
                return;
            }
            foreach (TreeViewItem child in children)
            {
                AddChild(child);
            }
        }

        public void AddChild(ITreeViewItem child)
        {
            TreeViewItem treeViewItem = child as TreeViewItem;
            if (treeViewItem != null)
            {
                if (m_Children == null)
                {
                    m_Children = new List<ITreeViewItem>();
                }
                m_Children.Add(treeViewItem);
                treeViewItem._parent = this;
            }
        }

        public void AddChildren(IList<ITreeViewItem> children)
        {
            foreach (ITreeViewItem child in children)
            {
                AddChild(child);
            }
        }

        public void RemoveChild(ITreeViewItem child)
        {
            if (m_Children != null)
            {
                TreeViewItem treeViewItem = child as TreeViewItem;
                if (treeViewItem != null)
                {
                    m_Children.Remove(treeViewItem);
                }
            }
        }

        private void m_onClick(ClickEvent evt)
        {
        }
    }
}
