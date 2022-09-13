using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Game.Logic.Editor
{
    internal sealed class CreateNodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public event Func<SearchTreeEntry, SearchWindowContext, bool> onSelectHandler;

        private LGCategoryInfo _categoryInfo;
        public void Init(LGCategoryInfo categoryInfo)
        {
            _categoryInfo = categoryInfo;
        }
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var searchTrees = new List<SearchTreeEntry>();
            searchTrees.Add(new SearchTreeGroupEntry(new GUIContent("创建节点")));

            AddNodeTree(searchTrees);
            return searchTrees;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            bool? res = onSelectHandler?.Invoke(searchTreeEntry, context);
            return res.HasValue ? res.Value : false;
        }

        /// <summary>
        /// 添加节点树
        /// </summary>
        /// <param name="searchTrees"></param>
        private void AddNodeTree(List<SearchTreeEntry> searchTrees)
        {
            List<string> groups = new List<string>();
            foreach (LogicNodeCategory nodeConfig in _categoryInfo.Nodes)
            {
                int createIndex = int.MaxValue;

                for (int i = 0; i < nodeConfig.NodeLayers.Length - 1; i++)
                {
                    string group = nodeConfig.NodeLayers[i];
                    if (i >= groups.Count)
                    {
                        createIndex = i;
                        break;
                    }
                    if (groups[i] != group)
                    {
                        groups.RemoveRange(i, groups.Count - i);
                        createIndex = i;
                        break;
                    }
                }
                for (int i = createIndex; i < nodeConfig.NodeLayers.Length - 1; i++)
                {
                    string group = nodeConfig.NodeLayers[i];
                    groups.Add(group);
                    searchTrees.Add(new SearchTreeGroupEntry(new GUIContent(group)) { level = i + 1 });
                }

                searchTrees.Add(new SearchTreeEntry(new GUIContent(nodeConfig.NodeName)) { level = nodeConfig.NodeLayers.Length, userData = nodeConfig });
            }
        }
    }
}
