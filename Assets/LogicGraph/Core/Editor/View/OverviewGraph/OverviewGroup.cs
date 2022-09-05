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
        public LGEditorCache lgCache { get; private set; }
        private Label title_label { get; }
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
        /// <param name="item"></param>
        internal void Initialize(LGEditorCache item)
        {
            lgCache = item;
            title = lgCache.GraphName;
            Refresh();

        }
        internal void Refresh()
        {
            string typeName = lgCache.GraphType.FullName;
            var list = LogicProvider.LGCatalogList.Where(a => a.GraphClassName == typeName).ToList();
            foreach (var item in list)
            {
                var node = new OverviewNode(onwer);
                node.Initialize(item);
                onwer.AddElement(node);
                this.AddElement(node);
            }
            this.schedule.Execute(() =>
            {
                Debug.LogError(1);
                ResetElementPosition();
            }).StartingIn(1);
        }

        internal void ResetElementPosition()
        {
            GraphElement firstElement = this.containedElements.FirstOrDefault();
            if (firstElement == null)
            {
                return;
            }
            int index = 0;
            Rect rect = this.GetPosition();
            var width = firstElement.GetPosition().width + 5;
            Debug.LogError(firstElement.GetPosition());
            Debug.LogError(firstElement.layout);
            foreach (var item in this.containedElements)
            {
                Rect temp = item.GetPosition();
                temp.x = width * index;
                temp.y = rect.y;
                item.SetPosition(temp);
                index++;
            }
            this.MarkDirtyRepaint();
        }
        public void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction($"创建{title}", null, DropdownMenuAction.AlwaysEnabled);
            evt.StopImmediatePropagation();
        }
    }
}
