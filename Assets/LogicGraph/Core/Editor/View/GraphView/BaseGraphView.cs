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
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public partial class BaseGraphView : GraphView
    {
        private const string STYLE_PATH = "GraphView/LogicGraphView.uss";

        private const int NODE_START_ID = 10000;

        /// <summary>
        /// 默认节点
        /// </summary>
        public virtual List<Type> DefaultNodes => new List<Type>();
        /// <summary>
        /// 默认变量
        /// </summary>
        public virtual List<IVariable> DefaultVars => new List<IVariable>();
        /// <summary>
        /// 当前逻辑图可以用的变量
        /// </summary>
        public virtual List<Type> VarTypes => new List<Type>();
        /// <summary>
        /// 当前逻辑图的分类信息
        /// </summary>
        public LGCategoryInfo categoryInfo => owner.operateData.categoryInfo;
        /// <summary>
        /// 当前逻辑的简介信息和编辑器信息
        /// </summary>
        public LGSummaryInfo summaryInfo => owner.operateData.summaryInfo;
        /// <summary>
        /// 当前逻辑图的编辑器信息
        /// </summary>
        internal GraphEditorData editorData => owner.operateData.editorData;
        /// <summary>
        /// 当前逻辑图
        /// </summary>
        public BaseLogicGraph target => owner.operateData.logicGraph;

        private EdgeConnectorListener _connectorListener;
        /// <summary>
        /// 端口连接监听器
        /// </summary>
        internal EdgeConnectorListener ConnectorListener => _connectorListener;
        /// <summary>
        /// 当前逻辑图所属的窗口
        /// </summary>
        public LGWindow owner { get; private set; }

        /// <summary>
        /// 创建节点的搜索窗口
        /// </summary>
        private CreateNodeSearchWindow _createNodeSearch = null;

        /// <summary>
        /// 逻辑图变量面板
        /// </summary>
        private GraphVariableView _variableView;
        /// <summary>
        /// 节点唯一ID
        /// </summary>
        private int _nodeUniqueId = NODE_START_ID;
        /// <summary>
        /// 没有使用到的Id
        /// </summary>
        private Queue<int> _unusedIds = new Queue<int>();

        private Dictionary<int, Node> _nodeCacheDic = new Dictionary<int, Node>();
        /// <summary>
        /// 可以连接的端口
        /// </summary>
        private List<Port> _canLinkPorts = new List<Port>();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    partial class BaseGraphView
    {
        public BaseGraphView()
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            Input.imeCompositionMode = IMECompositionMode.On;
            m_addGridBackGround();
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
            this.RegisterCallback<DragPerformEvent>(m_onDragPerformEvent);
            this.RegisterCallback<DragUpdatedEvent>(m_onDragUpdatedEvent);
            this.RegisterCallback<KeyDownEvent>(m_onKeyDownEvent);
            _connectorListener = new EdgeConnectorListener(this);
            _variableView = new GraphVariableView();
            this.Add(_variableView);
            //扩展大小与父对象相同
            this.StretchToParentSize();
        }

        public void Initialize(LGWindow lgWindow)
        {
            owner = lgWindow;
            m_initNodeUniqueId();
            editorData.NodeDatas.RemoveAll(a => target.GetNode(a.OnlyId) == null);
            target.Nodes.RemoveAll(a => editorData.NodeDatas.FirstOrDefault(b => a.OnlyId == b.OnlyId) == null);
            editorData.NodeDatas.ForEach(a =>
            {
                a.target = target.GetNode(a.OnlyId);
                m_showNodeView(a);
            });
            editorData.VarDatas.RemoveAll(a => target.GetVar(a.Name) == null);
            target.Variables.RemoveAll(a => editorData.VarDatas.FirstOrDefault(b => a.Name == b.Name) == null);
            editorData.VarDatas.ForEach(a => { a.target = target.GetVar(a.Name); });
            viewTransform.position = editorData.Pos;
            viewTransform.scale = editorData.Scale;
            graphViewChanged = m_onGraphViewChanged;
            viewTransformChanged = m_onViewTransformChanged;
            _variableView.InitializeGraphView(this);
            _variableView.Show();
            editorData.VarNodeDatas.ForEach(a =>
            {
                a.owner = editorData.VarDatas.FirstOrDefault(c => a.Name == c.Name);
                m_showVarNodeView(a);
            });
            editorData.GroupDatas.ForEach(a => m_showGroupView(a));
        }
    }

    #region 公共方法

    //公共方法
    partial class BaseGraphView
    {
        /// <summary>
        /// 是否存在一个节点视图
        /// </summary>
        /// <param name="nodeId">节点唯一ID</param>
        /// <returns></returns>
        public bool HasNodeView(int nodeId)
        {
            return _nodeCacheDic.ContainsKey(nodeId);
        }
        /// <summary>
        /// 获取一个节点视图
        /// </summary>
        /// <param name="nodeId">节点唯一ID</param>
        /// <returns></returns>
        public Node GetNodeView(int nodeId) => HasNodeView(nodeId) ? _nodeCacheDic[nodeId] : null;
        /// <summary>
        /// 添加一个节点
        /// </summary>
        public BaseLogicNode AddNode(Type nodeType)
        {
            return AddNode(nodeType, Vector2.zero);
        }
        /// <summary>
        /// 添加一个节点
        /// </summary>
        public BaseLogicNode AddNode(Type nodeType, Vector2 pos, bool isDefault = false)
        {
            BaseLogicNode logicNode = Activator.CreateInstance(nodeType) as BaseLogicNode;
            LogicNodeCategory nodeCategory = categoryInfo.Nodes.FirstOrDefault(a => a.NodeType == nodeType);
            NodeEditorData data = new NodeEditorData();
            data.target = logicNode;
            data.Pos = pos;
            data.Title = nodeCategory.NodeName;
            m_setNodeId(logicNode);
            data.OnlyId = logicNode.OnlyId;
            this.target.Nodes.Add(logicNode);
            this.editorData.NodeDatas.Add(data);
            if (isDefault)
            {
                this.target.StartNodes.Add(logicNode.OnlyId);
            }
            m_showNodeView(data);
            return logicNode;
        }
        /// <summary>
        /// 添加一个变量
        /// </summary>
        /// <param name="varName">变量名</param>
        /// <param name="varType">变量类型</param>
        /// <exception cref="NotImplementedException"></exception>
        public void AddVariable(string varName, Type varType)
        {
            var variable = Activator.CreateInstance(varType) as IVariable;
            variable.Name = varName;
            target.Variables.Add(variable);
            var varData = new VarEditorData();
            varData.target = variable;
            varData.Name = varName;
            editorData.VarDatas.Add(varData);
            this.owner.OnEvent(LogicEventId.VAR_ADD, new VarAddEventArgs() { var = variable });
        }
        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            this.style.display = DisplayStyle.Flex;
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            this.style.display = DisplayStyle.None;
        }
        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            target.SetEditorData(editorData);
            summaryInfo.SetData(editorData);
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    #endregion

    #region 重写

    //重写
    partial class BaseGraphView
    {
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("创建节点", m_onCreateNodeWindow, DropdownMenuAction.AlwaysEnabled);
            evt.menu.AppendAction("创建分组", m_onCreateGroup, DropdownMenuAction.AlwaysEnabled);
            if (DefaultNodes.Count > 0)
            {
                evt.menu.AppendAction("创建默认节点", m_onCreateDefaultNode, DropdownMenuAction.AlwaysEnabled);
            }
            evt.menu.AppendSeparator();
            foreach (var item in categoryInfo.Formats)
            {
                evt.menu.AppendAction("导出: " + item.FormatName, null, DropdownMenuAction.AlwaysEnabled, item);
            }
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            _canLinkPorts.Clear();
            if (startPort.direction == Direction.Input)
            {
                goto End;
            }
            if (startPort is NodePort nodePort)
            {
                foreach (var port in ports.ToList())
                {
                    if (port.direction == Direction.Output)
                    {
                        continue;
                    }
                    if (nodePort.node == port.node)
                    {
                        continue;
                    }
                    if (nodePort.connections.FirstOrDefault(a => a.input == port) != null)
                    {
                        continue;
                    }
                    var tarPort = port as NodePort;
                    if (tarPort == null)
                    {
                        continue;
                    }
                    bool isResult = nodePort.CanLinkPort(tarPort);
                    //if (isResult)
                    //    tarPort.AcceptPort(nodePort);
                    if (isResult)
                        _canLinkPorts.Add(tarPort);
                }
            }
        End: return _canLinkPorts;
        }

    }

    #endregion

    #region 回调

    //回调
    partial class BaseGraphView
    {
        private GraphViewChange m_onGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                List<GraphElement> removeList = graphViewChange.elementsToRemove.ToList();
                List<GraphElement> removeList2 = removeList.ToList();
                foreach (GraphElement item in removeList)
                {
                    switch (item)
                    {
                        case EdgeView edgeView:
                            //NodePort input = edgeView.input as NodePort;
                            //NodePort output = edgeView.output as NodePort;
                            //output.DelPort(input);
                            //input.DelPort(output);
                            break;
                        case VarNodeView varNodeView:
                            this.m_recycleUniqueId(varNodeView.editorData.OnlyId);
                            this.editorData.VarNodeDatas.Remove(varNodeView.editorData);
                            break;
                        default:
                            break;
                    }
                }

            }
            return graphViewChange;
        }
        private void m_onViewTransformChanged(GraphView graphView)
        {
            editorData.Pos = viewTransform.position;
            editorData.Scale = viewTransform.scale;
        }
        /// <summary>
        /// 键盘按键
        /// </summary>
        /// <param name="evt"></param>
        private void m_onKeyDownEvent(KeyDownEvent evt)
        {
            if (evt.ctrlKey)
            {
                switch (evt.keyCode)
                {
                    case KeyCode.S:
                        {
                            //保存
                            this.Save();
                            evt.StopPropagation();
                        }
                        break;
                    //case KeyCode.Z:
                    //    {
                    //        //撤销
                    //        _undo.PopUndo();
                    //        evt.StopPropagation();
                    //    }
                    //    break;
                    //case KeyCode.D:
                    //    {
                    //        //复制
                    //        op_Duplicate(evt);
                    //        evt.StopPropagation();
                    //    }
                    //    break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 节点搜索窗
        /// </summary>
        /// <param name="obj"></param>
        protected void m_onCreateNodeWindow(DropdownMenuAction obj)
        {
            if (_createNodeSearch == null)
            {
                _createNodeSearch = ScriptableObject.CreateInstance<CreateNodeSearchWindow>();
                _createNodeSearch.Init(categoryInfo);
                _createNodeSearch.onSelectHandler += m_onCreateNodeSelectEntry;
            }

            Vector2 screenPos = owner.GetScreenPosition(obj.eventInfo.mousePosition);
            SearchWindow.Open(new SearchWindowContext(screenPos), _createNodeSearch);
        }
        /// <summary>
        /// 节点搜索窗回调
        /// </summary>
        /// <param name="searchTreeEntry"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool m_onCreateNodeSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            //经过计算得出节点的位置
            var windowMousePosition = this.ChangeCoordinatesTo(this, context.screenMousePosition - owner.position.position);
            var nodePosition = this.contentViewContainer.WorldToLocal(windowMousePosition);
            if (searchTreeEntry.userData is LogicNodeCategory nodeCategory)
            {
                BaseLogicNode node = AddNode(nodeCategory.NodeType, nodePosition);
            }

            return true;
        }
        /// <summary>
        /// 创建默认节点
        /// </summary>
        /// <param name="obj"></param>
        private void m_onCreateDefaultNode(DropdownMenuAction obj)
        {
            int createNum = 0;
            Vector2 screenPos = owner.GetScreenPosition(obj.eventInfo.mousePosition);
            //经过计算得出节点的位置
            var windowMousePosition = owner.rootVisualElement.ChangeCoordinatesTo(owner.rootVisualElement.parent, screenPos - owner.position.position);
            var nodePosition = this.contentViewContainer.WorldToLocal(windowMousePosition);
            foreach (var item in categoryInfo.DefaultNodes)
            {
                if (target.Nodes.FirstOrDefault(a => a.GetType().FullName == item) != null)
                {
                    continue;
                }
                AddNode(categoryInfo.GetNodeCategory(item).NodeType, nodePosition, true);
                createNum++;
            }
            if (createNum == 0)
            {
                owner.ShowNotification(new GUIContent("当前逻辑图已创建所有默认节点"));
            }
        }
        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void m_onCreateGroup(DropdownMenuAction obj)
        {
            Vector2 screenPos = owner.GetScreenPosition(obj.eventInfo.mousePosition);
            //经过计算得出节点的位置
            var windowMousePosition = owner.rootVisualElement.ChangeCoordinatesTo(owner.rootVisualElement.parent, screenPos - owner.position.position);
            var groupPos = this.contentViewContainer.WorldToLocal(windowMousePosition);
            GroupEditorData group = new GroupEditorData();

            group.Pos = groupPos;
            group.Title = "默认分组";

            editorData.GroupDatas.Add(group);
            m_showGroupView(group);
        }
        private void m_onDragPerformEvent(DragPerformEvent evt)
        {
            var mousePos = (evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
            var dragData = DragAndDrop.GetGenericData("DragSelection") as List<ISelectable>;
            if (dragData != null)
            {
                var exposedFieldViews = dragData.OfType<GraphVariableFieldView>();
                if (exposedFieldViews.Any())
                {
                    foreach (GraphVariableFieldView varFieldView in exposedFieldViews)
                    {
                        Debug.LogError(varFieldView.varData.Name);
                        VarNodeEditorData varNodeData = new VarNodeEditorData();
                        varNodeData.owner = varFieldView.varData;
                        varNodeData.Pos = mousePos;
                        varNodeData.Name = varFieldView.varData.Name;
                        varNodeData.OnlyId = m_getId();
                        editorData.VarNodeDatas.Add(varNodeData);
                        m_showVarNodeView(varNodeData);
                    }
                }
            }
        }
        private void m_onDragUpdatedEvent(DragUpdatedEvent evt)
        {
            List<ISelectable> dragData = DragAndDrop.GetGenericData("DragSelection") as List<ISelectable>;
            bool dragging = false;
            if (dragData != null)
                dragging = dragData.OfType<GraphVariableFieldView>().Any();
            if (dragging)
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
        }
    }

    #endregion

    #region 私有

    //私有
    partial class BaseGraphView
    {
        private void m_showNodeView(NodeEditorData editorData)
        {
            LogicNodeCategory nodeCategory = categoryInfo.GetNodeCategory(editorData.target.GetType());
            BaseNodeView nodeView = Activator.CreateInstance(nodeCategory.ViewType) as BaseNodeView;
            this.AddElement(nodeView);
            nodeView.Initialize(this, editorData);
            nodeView.ShowUI();
            nodeView.SetPosition(new Rect(editorData.Pos, Vector2.one));
            _nodeCacheDic.Add(editorData.OnlyId, nodeView);
        }

        /// <summary>
        /// 添加变量视图
        /// </summary>
        private void m_showVarNodeView(VarNodeEditorData varNodeEditor)
        {
            VarNodeView nodeView = new VarNodeView();
            this.AddElement(nodeView);
            nodeView.Initialize(this, varNodeEditor);
            _nodeCacheDic.Add(varNodeEditor.OnlyId, nodeView);
        }

        /// <summary>
        /// 添加分组视图
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private LogicGroupView m_showGroupView(GroupEditorData group)
        {
            LogicGroupView groupView = new LogicGroupView();
            groupView.Initialize(this, group);
            this.AddElement(groupView);
            return groupView;
        }
        /// <summary>
        /// 获取节点唯一ID
        /// </summary>
        /// <returns></returns>
        private int m_getId()
        {
            int id = 0;
            if (_unusedIds.Count > 0)
            {
                id = _unusedIds.Dequeue();
            }
            else
            {
                id = _nodeUniqueId;
                _nodeUniqueId++;
            }
            return id;
        }

        /// <summary>
        /// 设置唯一Id
        /// </summary>
        /// <param name="node"></param>
        private void m_setNodeId(BaseLogicNode node)
        {
            var field = typeof(BaseLogicNode).GetField("_onlyId", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                field.SetValue(node, m_getId());
            }
            else
            {
                Debug.LogError("节点没有找到字段");
            }
        }
        /// <summary>
        /// 设置唯一Id
        /// </summary>
        private void m_recycleUniqueId(int id)
        {
            _unusedIds.Enqueue(id);
        }
        /// <summary>
        /// 设置唯一Id
        /// </summary>
        /// <param name="node"></param>
        private void m_initNodeUniqueId()
        {
            if (target.Nodes.Count > 0)
            {

                int node = Math.Max(target.Nodes.Max(a => a.OnlyId), editorData.VarNodeDatas.Count == 0 ? 0 : editorData.VarNodeDatas.Max(a => a.OnlyId));
                _nodeUniqueId = node + 1;
                for (int i = NODE_START_ID; i < _nodeUniqueId; i++)
                {
                    if (target.GetNode(i) == null && editorData.GetVarNodeEditorData(i) == null)
                    {
                        _unusedIds.Enqueue(i);
                    }
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// 背景网格
    /// </summary>
    partial class BaseGraphView
    {
        private class LGPanelViewGrid : GridBackground { }
        /// <summary>
        /// 添加背景网格
        /// </summary>
        private void m_addGridBackGround()
        {
            //添加网格背景
            GridBackground gridBackground = new LGPanelViewGrid();
            gridBackground.name = "GridBackground";
            Insert(0, gridBackground);
            //设置背景缩放范围
            ContentZoomer contentZoomer = new ContentZoomer();
            contentZoomer.minScale = 0.05f;
            contentZoomer.maxScale = 2f;
            contentZoomer.scaleStep = 0.05f;
            this.AddManipulator(contentZoomer);
            //扩展大小与父对象相同
            this.StretchToParentSize();
        }
    }
}
