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
    /// 总览图
    /// </summary>
    internal class OverviewNode : Node
    {
        private const string STYLE_PATH = "OverviewGraph/OverviewNode.uss";
        public OverviewGraphView onwer { get; }
        public OverviewGroup group { get; }
        private EditorLabelElement title_element { get; }
        public override string title { get => title_element.text; set => title_element.text = value; }
        private VisualElement _contentContainer;
        public override VisualElement contentContainer => _contentContainer;

        public LGCatalogCache data { get; private set; }
        private Label _createTimeLabel;
        private Label _modifyTimeLabel;
        public OverviewNode(OverviewGraphView view, OverviewGroup group)
        {
            onwer = view;
            this.group = group;
            base.capabilities = Capabilities.Selectable;
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            _contentContainer = new VisualElement();
            this.topContainer.parent.Add(_contentContainer);
            _contentContainer.name = "center";
            _contentContainer.style.backgroundColor = new Color(0, 0, 0, 0.5f);
            titleButtonContainer.RemoveFromHierarchy();
            topContainer.RemoveFromHierarchy();
            while (this.titleContainer.childCount > 0)
            {
                this.titleContainer[0].RemoveFromHierarchy();
            }
            title_element = new EditorLabelElement("默认逻辑图");
            title_element.onRename += m_onRename;
            titleContainer.Add(title_element);
            InitUI();
            this.RegisterCallback<MouseDownEvent>(m_onClick);
        }




        /// <summary>
        /// 设置逻辑图简介
        /// </summary>
        public void Initialize(LGCatalogCache item)
        {
            data = item;
            title = item.LogicName;
            _createTimeLabel.text = "创建时间:  " + item.EditorData.CreateTime;
            _modifyTimeLabel.text = "修改时间:  " + item.EditorData.ModifyTime;
        }

        private void InitUI()
        {
            var separator = new SeparatorElement(SeparatorElement.SeparatorDirection.Horizontal);
            separator.thickness = 2;
            separator.color = group.data.GraphColor;

            contentContainer.Add(separator);
            _createTimeLabel = new Label();
            _createTimeLabel.AddToClassList("time_label");
            _modifyTimeLabel = new Label();
            _modifyTimeLabel.AddToClassList("time_label");
            contentContainer.Add(_createTimeLabel);
            contentContainer.Add(_modifyTimeLabel);
        }
        private void m_onRename(string arg1, string arg2)
        {
            data.EditorData.LogicName = arg2;
            BaseLogicGraph logicGraph = AssetDatabase.LoadAssetAtPath<BaseLogicGraph>(data.AssetPath);
            if (logicGraph != null)
                logicGraph.SetEditorData(data.EditorData);
            else
                onwer.onwer.ShowNotification(new GUIContent("需要改名的逻辑图没有找到"), 1);
        }

        private void m_onClick(MouseDownEvent evt)
        {
            if (evt.clickCount == 2)
            {
                Debug.LogError("打开逻辑图");
            }
        }
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.StopImmediatePropagation();
        }

    }
}
