using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 总览图
    /// </summary>
    internal class OverviewGroup : Scope
    {
        private const string STYLE_PATH = "OverviewGraph/OverviewGroup.uss";
        public OverviewGraphView onwer { get; }

        /// <summary>
        /// 逻辑图编辑器信息缓存
        /// </summary>
        public LGEditorCache data { get; private set; }
        private Label title_label { get; }
        private List<OverviewNode> _nodes = new List<OverviewNode>();
        public override string title { get => title_label.text; set => title_label.text = value; }
        public OverviewGroup(OverviewGraphView view)
        {
            base.capabilities |= Capabilities.Selectable | Capabilities.Movable;
            onwer = view;
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            title_label = new Label("默认逻辑图");
            this.headerContainer.Add(title_label);
            this.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="editorCache"></param>
        internal void Initialize(LGEditorCache editorCache)
        {
            data = editorCache;
            title = data.GraphName;
            title_label.style.color = editorCache.GraphColor;
            string typeName = data.GraphType.FullName;
            var list = LogicProvider.LGCatalogList.Where(a => a.GraphClassName == typeName).ToList();
            foreach (var item in list)
            {
                var node = new OverviewNode(onwer, this);
                node.Initialize(item);
                onwer.AddElement(node);
                this.AddElement(node);
                _nodes.Add(node);
            }
            ResetElementPosition();
        }

        public void Show()
        {
            LogicMessage.AddListener(LogicEventId.LOGIC_ASSETS_CHANGED, m_onLogicAssetsChanged);
        }

        public void Hide()
        {
            LogicMessage.RemoveListener(LogicEventId.LOGIC_ASSETS_CHANGED, m_onLogicAssetsChanged);
        }

        private bool m_onLogicAssetsChanged(object arg)
        {
            var changed = arg as LogicAssetsChangedEventArgs;

            foreach (var item in changed.addGraphs)
            {
                LGCatalogCache catalog = LogicProvider.LGCatalogList.FirstOrDefault(a => a.AssetPath == item);
                if (catalog != null && catalog.GraphClassName == data.GraphType.FullName)
                {
                    var node = new OverviewNode(onwer, this);
                    node.Initialize(catalog);
                    onwer.AddElement(node);
                    this.AddElement(node);
                    _nodes.Add(node);
                }
            }
            var temp = containedElements as List<GraphElement>;
            foreach (var item in changed.deletedGraphs)
            {
                OverviewNode node = _nodes.FirstOrDefault(a => a.data.AssetPath == item);
                if (node != null)
                {
                    _nodes.Remove(node);
                    this.RemoveElement(node);
                    onwer.RemoveElement(node);
                }
            }
            ResetElementPosition();
            return true;
        }
        internal void ResetElementPosition()
        {
            int index = 0;
            foreach (var item in this.containedElements)
            {
                Rect temp = item.GetPosition();
                temp.x = 145 * index;
                temp.y = data.Index * 160;
                item.SetPosition(temp);
                index++;
            }
        }
        public void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction($"创建{title}", null, DropdownMenuAction.AlwaysEnabled);
            evt.StopImmediatePropagation();
        }
    }
}
