﻿using Game.Logic.Runtime;
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
        private const string STYLE_PATH = "Uss/OverviewGraph/OverviewNode.uss";
        public OverviewGraphView onwer { get; }
        public OverviewGroup group { get; }
        private EditorLabelElement title_element { get; }
        public override string title { get => title_element.text; set => title_element.text = value; }
        private VisualElement _contentContainer;
        public override VisualElement contentContainer => _contentContainer;

        public LGSummaryInfo data { get; private set; }
        private Label _createTimeLabel;
        private Label _modifyTimeLabel;
        private EditorLabelElement _desLabel;
        public OverviewNode(OverviewGraphView view, OverviewGroup group)
        {
            onwer = view;
            this.group = group;
            base.capabilities = Capabilities.Selectable;
            this.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
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
        public void Initialize(LGSummaryInfo item)
        {
            data = item;
            title = item.LogicName;
            _desLabel.text = item.Describe;
            _createTimeLabel.text = "创建时间:  " + item.CreateTime;
            _modifyTimeLabel.text = "修改时间:  " + item.ModifyTime;
        }

        private void InitUI()
        {
            var separator = new SeparatorElement(ElementDirection.Horizontal);
            separator.thickness = 2;
            separator.color = group.data.GraphColor;

            contentContainer.Add(separator);
            _desLabel = new EditorLabelElement();
            _desLabel.text = "这里是描述,";
            _desLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
            _desLabel.style.color = new Color(137 / 255f, 137 / 255f, 137 / 255f);
            _desLabel.style.fontSize = 10;
            _desLabel.Q<Label>("title_label").style.whiteSpace = WhiteSpace.Normal;
            _desLabel.maxLength = 20;
            _desLabel.onRename += m_onDesRename;
            contentContainer.Add(_desLabel);
            _createTimeLabel = new Label();
            _createTimeLabel.AddToClassList("time_label");
            _modifyTimeLabel = new Label();
            _modifyTimeLabel.AddToClassList("time_label");
            contentContainer.Add(_createTimeLabel);
            contentContainer.Add(_modifyTimeLabel);
        }

        private void m_onDesRename(string arg1, string arg2)
        {
            var (graph, editorData) = LogicUtils.GetLogicGraph(data.AssetPath);
            if (graph != null)
            {
                editorData.Describe = arg2;
                graph.SetEditorData(editorData);
                data.Describe = arg2;
                EditorUtility.SetDirty(graph);
                AssetDatabase.SaveAssets();
                LogicUtils.UnloadObject(graph);
            }
            else
                onwer.onwer.ShowNotification(new GUIContent("需要修改描述的逻辑图没有找到"), 1);
        }

        private void m_onRename(string arg1, string arg2)
        {
            var (graph, editorData) = LogicUtils.GetLogicGraph(data.AssetPath);
            if (graph != null)
            {
                editorData.LogicName = arg2;
                graph.SetEditorData(editorData);
                data.LogicName = arg2;
                EditorUtility.SetDirty(graph);
                AssetDatabase.SaveAssets();
                LogicUtils.UnloadObject(graph);
            }
            else
                onwer.onwer.ShowNotification(new GUIContent("需要改名的逻辑图没有找到"), 1);
        }

        private void m_onClick(MouseDownEvent evt)
        {
            if (evt.clickCount == 2)
            {
                onwer.onwer.ShowLogicGraph(data.OnlyId);
                Debug.LogError("打开逻辑图");
            }
        }
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.StopImmediatePropagation();
        }

    }
}
