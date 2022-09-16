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
        private EditorLabelElement m_titleLabel;
        /// <summary>
        /// 运行状态
        /// </summary>
        private VisualElement _runState;
        /// <summary>
        /// 锁
        /// </summary>
        private VisualElement _lock;

        public LogicGraphView owner { get; private set; }

        /// <summary>
        /// 当前节点视图对应的节点
        /// </summary>
        public BaseLogicNode target { get; private set; }

        /// <summary>
        /// 节点编辑器数据
        /// </summary>
        public NodeEditorData editorData { get; private set; }

        public override string title { get => m_titleLabel.text; set => m_titleLabel.text = value; }

        /// <summary>
        /// 显示锁
        /// </summary>
        public virtual bool ShowLock => true;
        /// <summary>
        /// 显示状态
        /// </summary>
        public virtual bool ShowState => true;
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

        /// <summary>
        /// 当前节点视图的宽度
        /// </summary>
        public float width
        {
            get => this.style.width.value.value;
            protected set
            {
                this.style.width = value;
            }
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    partial class BaseNodeView
    {
        public BaseNodeView()
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            _contentContainer = new VisualElement();
            _contentContainer.name = "contentContainer";
            _contents = topContainer.parent;
            _contents.Add(_contentContainer);
            Port port = this.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, null);
            this.inputContainer.Add(port);
            Port port2 = this.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, null);
            this.outputContainer.Add(port2);
        }



        public void Initialize(LogicGraphView owner, NodeEditorData editorData)
        {
            this.owner = owner;
            this.target = editorData.node;
            this.editorData = editorData;
            m_initTitle();
            this.title = editorData.Title;
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
        public override void SetPosition(Rect newPos)
        {
            if (editorData.IsLock)
            {
                return;
            }
            base.SetPosition(newPos);
            editorData.Pos = newPos.position;
        }
    }

    /// <summary>
    /// 事件
    /// </summary>
    partial class BaseNodeView
    {
        /// <summary>
        /// 锁点击事件
        /// </summary>
        /// <param name="evt"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void m_lockClick(PointerUpEvent evt)
        {
            editorData.IsLock = !editorData.IsLock;
            _lock.ClearClassList();
            if (editorData.IsLock)
            {
                _lock.AddToClassList("lock");
            }
            else
                _lock.AddToClassList("unlock");
        }
    }

    /// <summary>
    /// 私有方法
    /// </summary>
    partial class BaseNodeView
    {
        /// <summary>
        /// 初始化title
        /// </summary>
        private void m_initTitle()
        {
            while (this.titleContainer.childCount > 0)
            {
                this.titleContainer[0].RemoveFromHierarchy();
            }
            m_initState();
            m_titleLabel = new EditorLabelElement();
            titleContainer.Add(m_titleLabel);
            m_initLock();
        }
        /// <summary>
        /// 初始化状态栏
        /// </summary>
        private void m_initState()
        {
            _runState = new VisualElement();
            _runState.name = "run-state";
            _runState.tooltip = "运行状态";
            titleContainer.Insert(0, _runState);
            string str = $"run-state-shape";
            _runState.AddToClassList(str);
            _runState.style.display = ShowState ? DisplayStyle.Flex : DisplayStyle.None;
        }
        /// <summary>
        /// 初始化锁
        /// </summary>
        private void m_initLock()
        {
            _lock = new Button();
            _lock.name = "lock-icon";
            _lock.ClearClassList();
            _lock.AddToClassList(editorData.IsLock ? "lock" : "unlock");
            _lock.style.display = ShowLock ? DisplayStyle.Flex : DisplayStyle.None;
            _lock.RegisterCallback<PointerUpEvent>(m_lockClick);
            titleContainer.Add(_lock);
        }



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
