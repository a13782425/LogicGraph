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
        private VisualElement _contentContainer;
        public override VisualElement contentContainer => _contentContainer;
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
            this.title = "测试逻辑图";
            InitUI();
        }

        /// <summary>
        /// 设置逻辑图简介
        /// </summary>
        public void SetLogicData()
        {

        }

        private void InitUI()
        {
            contentContainer.style.backgroundColor = new Color(0, 0, 0, 0.5f);
            contentContainer.Add(new Label("创建日期:\r\n" + DateTime.Now.ToString()));
            contentContainer.Add(new Label("修改日期:\r\n" + DateTime.Now.ToString()));
        }
    }
}
