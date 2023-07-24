using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace Game.Logic.Editor
{
    /// <summary>
    /// 逻辑图节点视图
    /// </summary>
    public partial class BaseNodeView : Node
    {
        private const string STYLE_PATH = "Uss/GraphView/BaseNodeView.uss";
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

        public BaseGraphView owner { get; private set; }

        /// <summary>
        /// 当前节点视图对应的节点
        /// </summary>
        public BaseLogicNode target { get; private set; }

        /// <summary>
        /// 节点编辑器数据
        /// </summary>
        internal NodeEditorData editorData { get; private set; }

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
        /// 入端口
        /// </summary>
        public NodePort Input { get; protected set; }

        /// <summary>
        /// 出端口
        /// </summary>
        public NodePort OutPut { get; protected set; }
        /// <summary>
        /// 用户自定义的组建的容器
        /// </summary>
        private VisualElement _nodeContent;

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

        /// <summary>
        /// 所有端口的缓存
        /// </summary>
        private List<NodePort> _cachePorts = new List<NodePort>();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    partial class BaseNodeView
    {
        public BaseNodeView()
        {
            this.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
            _nodeContent = new VisualElement();
            _nodeContent.name = "nodeContent";
            this.topContainer.parent.Add(_nodeContent);
            this.Q("node-border").style.overflow = Overflow.Visible;
        }

        internal void Initialize(BaseGraphView owner, NodeEditorData editorData)
        {
            this.owner = owner;
            this.target = editorData.target;
            this.editorData = editorData;
            m_initTitle();
            this.title = editorData.Title;
            m_initPort();
        }
    }

    #region 公共

    partial class BaseNodeView
    {
        /// <summary>
        /// 根据端口变量名获取端口
        /// </summary>
        /// <param name="fieldName">变量名</param>
        /// <param name="isIn">是否是入端口</param>
        /// <returns></returns>
        public NodePort GetPortByFieldName(string fieldName, bool isIn = true)
        {
            Direction dir = isIn ? Direction.Input : Direction.Output;
            return _cachePorts.FirstOrDefault(a =>
            {
                if (a.direction != dir)
                {
                    return false;
                }
                if (a.fieldInfo == null)
                {
                    return false;
                }
                return a.fieldInfo.Name == fieldName;
            });
        }

        /// <summary>
        /// 连接已经连接的线
        /// </summary>
        public virtual void DrawLink()
        {
            if (this.OutPut == null)
            {
                this.target.Childs.Clear();
                return;
            }

            List<int> tempList = target.Childs.ToList();

            foreach (var item in tempList)
            {
                BaseNodeView nodeView = owner.GetNodeView(item) as BaseNodeView;
                if (nodeView == null || nodeView.Input == null)
                {
                    target.Childs.Remove(item);
                    continue;
                }
                else
                {
                    NodePort.JusrLinkPort(this.OutPut, nodeView.Input);
                }
            }
        }
        protected NodePort ShowPort(string title, PortDirEnum dir)
        {
            var port = NodePort.CreatePort(owner, this, dir);
            port.portName = title;
            return port;
        }
        protected NodePort ShowPort(string title, FieldInfo info, PortDirEnum dir)
        {
            var port = NodePort.CreatePort(owner, this, info, dir);
            port.portName = title;
            return port;
        }
    }

    #endregion

    #region 重写

    /// <summary>
    /// 重写和虚方法
    /// </summary>
    partial class BaseNodeView
    {
        /// <summary>
        /// 显示UI
        /// 子类实现
        /// </summary>
        public virtual void ShowUI()
        {
            var fields = this.target.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo item in fields)
            {
                InputAttribute inputAttribute = item.GetCustomAttribute<InputAttribute>();
                OutputAttribute outputAttribute = item.GetCustomAttribute<OutputAttribute>();
                if (inputAttribute != null)
                {
                    NodePort port = ShowPort(inputAttribute.name, item, PortDirEnum.In);
                    port.onAddPort += OnPortConnect;
                    port.onDelPort += OnPortDisconnect;
                    _cachePorts.Add(port);
                    this.AddUI(port);
                }
                if (outputAttribute != null)
                {
                    NodePort port = ShowPort(outputAttribute.name, item, PortDirEnum.Out);
                    port.onAddPort += OnPortConnect;
                    port.onDelPort += OnPortDisconnect;
                    _cachePorts.Add(port);
                    this.AddUI(port);
                }
            }
        }
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (!this.selected || this.owner.selection.Count > 1)
            {
                return;
            }
            evt.menu.AppendAction("查看节点代码", m_onOpenNodeScript);
            evt.menu.AppendAction("查看界面代码", m_onOpenNodeViewScript);
            evt.menu.AppendSeparator();
            //evt.menu.AppendAction("删除", (a) => owner.DeleteSelection());
            evt.StopPropagation();
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

    #endregion

    #region 子类方法

    /// <summary>
    /// 子类方法
    /// </summary>
    partial class BaseNodeView
    {
        /// <summary>
        /// 自定义端口连接方法
        /// </summary>
        protected virtual void OnPortConnect(NodePort curPort, NodePort tarPort)
        {
            if (curPort.direction == Direction.Input)
            {
                //入端口
                if (curPort.IsBasePort)
                    return;
                switch (tarPort.node)
                {
                    case VarNodeView varNodeView:
                        {
                            if (curPort.fieldInfo == null)
                                break;
                            VarMappingData varData = new VarMappingData();
                            varData.isInput = true;
                            varData.fieldName = curPort.fieldInfo.Name;
                            varData.varName = varNodeView.target.Name;
                            this.target.VarMappings.Add(varData);
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //出端口
                if (curPort.IsBasePort)
                {
                    var tarNodeView = tarPort.node as BaseNodeView;
                    if (tarPort.IsBasePort)
                    {
                        this.target.Childs.Add(tarNodeView.target.OnlyId);
                    }
                }
                else
                {
                    switch (tarPort.node)
                    {
                        case VarNodeView varNodeView:
                            {
                                if (curPort.fieldInfo == null)
                                    break;
                                VarMappingData varData = new VarMappingData();
                                varData.isInput = false;
                                varData.fieldName = curPort.fieldInfo.Name;
                                varData.varName = varNodeView.target.Name;
                                this.target.VarMappings.Add(varData);
                            }
                            break;
                        case BaseNodeView baseNodeView:
                            {
                                if (curPort.fieldInfo == null)
                                    break;
                                if (tarPort.IsBasePort)
                                {
                                    curPort.fieldInfo.SetValue(target, baseNodeView.target);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 自定义端口断开方法
        /// </summary>
        protected virtual void OnPortDisconnect(NodePort curPort, NodePort tarPort)
        {
            if (curPort.direction == Direction.Input)
            {
                //入端口
                if (curPort.IsBasePort)
                    return;
                else
                {
                    switch (tarPort.node)
                    {
                        case VarNodeView varNodeView:
                            {
                                if (curPort.fieldInfo == null)
                                    break;
                                VarMappingData varData = this.target.VarMappings.FirstOrDefault(a => a.isInput = true && a.fieldName == curPort.fieldInfo.Name && a.varName == varNodeView.target.Name);
                                if (varData != null)
                                    this.target.VarMappings.Remove(varData);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                //出端口
                if (curPort.IsBasePort)
                {
                    var tarNodeView = tarPort.node as BaseNodeView;
                    if (tarPort.IsBasePort)
                    {
                        this.target.Childs.Remove(tarNodeView.target.OnlyId);
                    }
                }
                else
                {
                    switch (tarPort.node)
                    {
                        case VarNodeView varNodeView:
                            {
                                if (curPort.fieldInfo == null)
                                    break;
                                VarMappingData varData = this.target.VarMappings.FirstOrDefault(a => a.isInput = false && a.fieldName == curPort.fieldInfo.Name && a.varName == varNodeView.target.Name);
                                if (varData != null)
                                    this.target.VarMappings.Remove(varData);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 添加一个UI元素到节点视图中
        /// </summary>
        /// <param name="ui"></param>
        protected void AddUI(VisualElement element)
        {
            this._nodeContent.Add(element);
        }
    }

    #endregion

    #region 事件

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

    #endregion

    #region 私有方法

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
        /// 初始化端口
        /// </summary>
        private void m_initPort()
        {
            LogicNodeCategory nodeCategory = owner.categoryInfo.GetNodeCategory(target.GetType());
            if ((nodeCategory.PortType & PortDirEnum.In) > 0)
            {
                Input = ShowPort("In", PortDirEnum.In);
                Input.AddToClassList("base_port");
                Input.onAddPort += OnPortConnect;
                Input.onDelPort += OnPortDisconnect;
                inputContainer.Add(Input);
            }
            else
            {
                inputContainer.RemoveFromHierarchy();
            }
            if ((nodeCategory.PortType & PortDirEnum.Out) > 0)
            {
                OutPut = ShowPort("Out", PortDirEnum.Out);
                OutPut.AddToClassList("base_port");
                OutPut.onAddPort += OnPortConnect;
                OutPut.onDelPort += OnPortDisconnect;
                outputContainer.Add(OutPut);
            }
            else
            {
                outputContainer.RemoveFromHierarchy();
            }
        }
        /// <summary>
        /// 查看节点代码
        /// </summary>
        /// <param name="obj"></param>
        protected void m_onOpenNodeScript(DropdownMenuAction obj)
        {
            string[] guids = AssetDatabase.FindAssets(this.target.GetType().Name);
            foreach (var item in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(item);
                MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                if (monoScript != null && monoScript.GetClass() == this.target.GetType())
                {
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object)), -1);
                    break;
                }
            }
        }
        /// <summary>
        /// 查看界面代码
        /// </summary>
        /// <param name="obj"></param>
        protected void m_onOpenNodeViewScript(DropdownMenuAction obj)
        {
            string[] guids = AssetDatabase.FindAssets(this.GetType().Name);
            foreach (var item in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(item);
                MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                if (monoScript != null && monoScript.GetClass() == this.GetType())
                {
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object)), -1);
                    break;
                }
            }
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

    #endregion
}
