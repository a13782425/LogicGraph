using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    internal sealed class LogicGroupView : Group
    {
        private const string STYLE_PATH = "GraphView/LogicGroupView.uss";
        private BaseGraphView owner;
        private GroupEditorData group;
        public GroupEditorData Group => group;


        Label titleLabel;
        ColorField colorField;

        public LogicGroupView()
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
        }

        private void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //evt.StopPropagation();
        }



        public void Initialize(BaseGraphView graphView, GroupEditorData group)
        {
            this.group = group;
            owner = graphView;

            title = group.Title;
            SetPosition(new Rect(group.Pos, group.Size));

            this.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));

            headerContainer.Q<TextField>().RegisterCallback<ChangeEvent<string>>(TitleChangedCallback);
            titleLabel = headerContainer.Q<Label>();

            colorField = new ColorField { value = group.Color, name = "headerColorPicker" };
            colorField.RegisterValueChangedCallback(e =>
            {
                UpdateGroupColor(e.newValue);
            });
            UpdateGroupColor(group.Color);

            headerContainer.Add(colorField);

            InitializeInnerNodes();
        }

        void InitializeInnerNodes()
        {
            foreach (var nodeId in group.Nodes.ToList())
            {
                if (!owner.HasNodeView(nodeId))
                {
                    group.Nodes.Remove(nodeId);
                    continue;
                }
                var nodeView = owner.GetNodeView(nodeId);

                AddElement(nodeView);
            }
        }
        public override bool AcceptsElement(GraphElement element, ref string reasonWhyNotAccepted)
        {
            if (element is BaseNodeView nodeView)
            {
                if (owner.target.StartNodes.Contains(nodeView.target.OnlyId))
                {
                    reasonWhyNotAccepted = "无法将默认节点添加到组";
                    return false;
                }
            }
            return base.AcceptsElement(element, ref reasonWhyNotAccepted);
        }

        protected override void OnElementsAdded(IEnumerable<GraphElement> elements)
        {
            foreach (var element in elements)
            {
                switch (element)
                {
                    case BaseNodeView nodeView:
                        if (!group.Nodes.Contains(nodeView.target.OnlyId))
                            group.Nodes.Add(nodeView.target.OnlyId);
                        break;
                    case VarNodeView varNodeView:
                        if (!group.Nodes.Contains(varNodeView.editorData.OnlyId))
                            group.Nodes.Add(varNodeView.editorData.OnlyId);
                        break;
                    default:
                        break;
                }
            }
            base.OnElementsAdded(elements);
        }

        protected override void OnElementsRemoved(IEnumerable<GraphElement> elements)
        {
            // Only remove the nodes when the group exists in the hierarchy
            if (parent != null)
            {
                foreach (var element in elements)
                {
                    switch (element)
                    {
                        case BaseNodeView nodeView:
                            group.Nodes.Remove(nodeView.target.OnlyId);
                            break;
                        case VarNodeView varNodeView:
                            group.Nodes.Remove(varNodeView.editorData.OnlyId);
                            break;
                        default:
                            break;
                    }
                }
            }
            base.OnElementsRemoved(elements);
        }

        public void UpdateGroupColor(Color newColor)
        {
            group.Color = newColor;
            style.backgroundColor = newColor;
        }

        void TitleChangedCallback(ChangeEvent<string> e)
        {
            group.Title = e.newValue;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            group.Pos = newPos.position;
            group.Size = newPos.size;
        }
    }
}
