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
    internal class OverviewGraphView : GraphView
    {
        private const string STYLE_PATH = "OverviewGraph/OverviewGraphView.uss";
        public LGWindow onwer { get; }
        public OverviewGraphView(LGWindow window)
        {
            onwer = window;
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
            var group = new OverviewGroup(this);
            this.AddElement(group);
            var node = new OverviewNode(this);
            this.AddElement(node);
            group.AddElement(node);
            node = new OverviewNode(this);
            this.AddElement(node);
            group.AddElement(node);
            node = new OverviewNode(this);
            this.AddElement(node);
            group.AddElement(node);
            this.MarkDirtyRepaint();
            this.schedule.Execute(() =>
            {
                group.ResetElementPosition();
            }).ExecuteLater(10);
        }

    }
}
