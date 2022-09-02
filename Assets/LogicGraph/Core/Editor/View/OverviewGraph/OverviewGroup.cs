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
        public OverviewGroup(OverviewGraphView view)
        {
            base.capabilities |= Capabilities.Selectable | Capabilities.Movable;
            onwer = view;
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            this.headerContainer.Add(new Label("默认逻辑图"));
            //this.title = "默认逻辑图";
            //var m_TitleEditor = this.Q("titleField");
            //m_TitleEditor.RemoveFromHierarchy();
        }

        internal void ResetElementPosition()
        {
            int index = 0;
            Rect rect = this.GetPosition();
            var width = this.containedElements.First().GetPosition().width + 5;
            Debug.LogError(this.containedElements.First().GetPosition());
            Debug.LogError(this.containedElements.First().layout);
            foreach (var item in this.containedElements)
            {
                Rect temp = item.GetPosition();
                temp.x = width * index;
                temp.y = rect.y;
                item.SetPosition(temp);
                index++;
            }
            this.UpdatePresenterPosition();
        }
    }
}
