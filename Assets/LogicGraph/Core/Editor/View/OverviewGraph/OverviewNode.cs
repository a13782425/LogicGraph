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
        private EditorLabelElement title_element { get; }
        public override string title { get => title_element.text; set => title_element.text = value; }
        private VisualElement _contentContainer;
        public override VisualElement contentContainer => _contentContainer;

        private Label _createTimeLabel;
        private Label _modifyTimeLabel;
        public OverviewNode(OverviewGraphView view)
        {
            onwer = view;
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
            titleContainer.Add(title_element);
            InitUI();
        }

        /// <summary>
        /// 设置逻辑图简介
        /// </summary>
        public void Initialize(LGCatalogCache item)
        {
            title = item.LogicName;
            _createTimeLabel.text = "创建时间:  " + item.EditorData.CreateTime;
            _modifyTimeLabel.text = "修改时间:  " + item.EditorData.ModifyTime;

        }

        private void InitUI()
        {
            contentContainer.style.backgroundColor = new Color(0, 0, 0, 0.5f);
            var separator = new SeparatorElement(SeparatorElement.SeparatorDirection.Horizontal);
            separator.thickness = 4;
            separator.color = Color.red;

            contentContainer.Add(separator);
            _createTimeLabel = new Label();
            _createTimeLabel.AddToClassList("time_label");
            _modifyTimeLabel = new Label();
            _modifyTimeLabel.AddToClassList("time_label");
            contentContainer.Add(_createTimeLabel);
            contentContainer.Add(_modifyTimeLabel);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.StopImmediatePropagation();
        }

    }
}
