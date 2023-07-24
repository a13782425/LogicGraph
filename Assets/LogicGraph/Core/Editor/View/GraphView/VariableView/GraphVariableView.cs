using Game.Logic.Runtime;
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
    /// 逻辑图变量面板
    /// </summary>
    internal sealed class GraphVariableView : Blackboard
    {
        private const string STYLE_PATH = "Uss/GraphView/GraphVariableView.uss";
        private BaseGraphView onwer;

        private VisualElement root;
        private VisualElement content;

        private ScrollView scrollView;

        public GraphVariableView()
        {
            this.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
            scrollView = new ScrollView(ScrollViewMode.Vertical);
            scrollView.horizontalScroller.RemoveFromHierarchy();
            root = this.Q("content");

            content = this.Q<VisualElement>(name: "contentContainer");

            style.overflow = Overflow.Hidden;

            AddToClassList("lgvariable");
            style.position = Position.Absolute;
            content.RemoveFromHierarchy();
            root.Add(scrollView);
            scrollView.Add(content);
            AddToClassList("scrollable");

            this.Q<Button>("addButton").clicked += m_onAddClicked;
            this.Q("subTitleLabel").RemoveFromHierarchy();
            title = "逻辑图变量";
            this.Hide();
        }

        public void InitializeGraphView(BaseGraphView graphView)
        {
            this.onwer = graphView;
            SetPosition(new Rect(new Vector2(0, 20), new Vector2(180, 320)));
            this.onwer.owner.AddListener(LogicEventId.VAR_ADD, m_onVarRefresh);
            this.onwer.owner.AddListener(LogicEventId.VAR_DEL, m_onVarRefresh);
        }

        public override void AddToSelection(ISelectable selectable)
        {
            if (onwer.selection.Count > 0)
            {
                onwer.ClearSelection();
            }
            base.AddToSelection(selectable);
        }
        public void Hide()
        {
            this.content.Clear();
            this.visible = false;
        }

        public void Show()
        {
            this.visible = true;
            m_updateVariableList();
        }
        private void m_onAddClicked()
        {
            var parameterType = new GenericMenu();

            foreach (var varType in m_getVariableTypes())
                parameterType.AddItem(new GUIContent(m_getNiceNameFromType(varType)), false, () =>
                {
                    string uniqueName = "New" + m_getNiceNameFromType(varType);
                    uniqueName = m_getUniqueName(uniqueName);
                    onwer.AddVariable(uniqueName, varType);
                });

            parameterType.ShowAsContext();
        }


        /// <summary>
        /// 变量刷新
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool m_onVarRefresh(object args)
        {
            m_updateVariableList();
            return true;
        }

        private void m_updateVariableList()
        {
            content.Clear();

            foreach (var variable in onwer.editorData.VarDatas)
            {
                var row = new BlackboardRow(new GraphVariableFieldView(onwer, variable), new GraphVariablePropView(onwer, variable));
                row.expanded = false;

                content.Add(row);
            }
        }

        private IEnumerable<Type> m_getVariableTypes()
        {
            if (onwer != null)
            {
                List<Type> types = onwer.VarTypes.Where(a => a.IsAssignableFrom(typeof(IVariable))).Distinct().ToList();
                if (types.Count == 0)
                {
                    types = TypeCache.GetTypesDerivedFrom<IVariable>().ToList();
                }
                foreach (var type in types)
                {
                    if (type.IsAbstract || type.IsInterface)
                        continue;
                    yield return type;
                }
            }
        }
        private string m_getNiceNameFromType(Type type)
        {
            var variable = Activator.CreateInstance(type) as IVariable;
            return variable.GetValueType().Name;
        }
        private string m_getUniqueName(string name)
        {
            // Generate unique name
            string uniqueName = name;
            int i = 0;
            while (onwer.target.Variables.Any(e => e.Name == name))
                name = uniqueName + (i++);
            return name;
        }
    }
}
