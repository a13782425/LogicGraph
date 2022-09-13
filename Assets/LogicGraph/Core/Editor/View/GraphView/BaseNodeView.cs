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
    /// 逻辑图节点视图
    /// </summary>
    public partial class BaseNodeView : Node
    {
        private const string STYLE_PATH = "GraphView/BaseNodeView.uss";
        /// <summary>
        /// 可编辑的标题
        /// </summary>
        private readonly EditorLabelElement m_titleLabel;


        /// <summary>
        /// 当前节点视图对应的节点
        /// </summary>
        public BaseLogicNode target { get; private set; }

        public override string title { get => m_titleLabel.text; set => m_titleLabel.text = value; }

        /// <summary>
        /// 用户自定义的组建的容器
        /// </summary>
        private VisualElement _contentContainer;
        //public override VisualElement contentContainer => _contentContainer;
        /// <summary>
        /// 节点所有内容的容器
        /// </summary>
        private VisualElement _contents;
        /// <summary>
        /// 端口的是上下结构还是左右结构
        /// </summary>
        public Orientation PortLayout { get; set; }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    partial class BaseNodeView
    {
        public BaseNodeView()
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            while (this.titleContainer.childCount > 0)
            {
                this.titleContainer[0].RemoveFromHierarchy();
            }
            m_titleLabel = new EditorLabelElement();
            titleContainer.Add(m_titleLabel);
            m_titleLabel.text = "萨达萨达所多";
            _contentContainer = new VisualElement();
            _contentContainer.name = "contentContainer";
            _contents = topContainer.parent;
            _contents.Add(_contentContainer);
            Port port = this.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, null);
            this.inputContainer.Add(port);
            Port port2 = this.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, null);
            this.outputContainer.Add(port2);
        }
    }
    /// <summary>
    /// 重写
    /// </summary>
    partial class BaseNodeView
    {
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {

        }
    }

    /// <summary>
    /// 私有方法
    /// </summary>
    partial class BaseNodeView
    {
        /// <summary>
        /// 端口垂直布局
        /// 暂时不做
        /// </summary>
        private void m_portVerticalLayout()
        {
            this.Insert(0, inputContainer);
            this.Add(outputContainer);
            this.Insert(1, titleContainer);
            this.AddToClassList("vertical-layout");
            //outputContainer.AddToClassList("vertical-layout");
        }
        /// <summary>
        /// 端口水平布局
        /// 暂时不做
        /// </summary>
        private void m_portHorizontalLayout()
        {
            topContainer.Insert(0, inputContainer);
            topContainer.Add(outputContainer);
            inputContainer.RemoveFromClassList("vertical-layout");
            outputContainer.RemoveFromClassList("vertical-layout");
        }
    }
}
