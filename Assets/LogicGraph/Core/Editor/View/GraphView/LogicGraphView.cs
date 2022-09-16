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
    public partial class LogicGraphView : GraphView
    {
        private const string STYLE_PATH = "GraphView/LogicGraphView.uss";

        private const int NODE_START_ID = 10000;

        /// <summary>
        /// 开始节点
        /// </summary>
        public virtual List<BaseLogicNode> StartNodes => new List<BaseLogicNode>();
        /// <summary>
        /// 默认变量
        /// </summary>
        public virtual List<BaseVariable> DefaultVars => new List<BaseVariable>();
        /// <summary>
        /// 当前逻辑图可以用的变量
        /// </summary>
        public virtual List<Type> VarTypes => new List<Type>();
        /// <summary>
        /// 当前逻辑图的公共信息缓存
        /// </summary>
        public LGCategoryInfo categoryInfo => owner.operateData.categoryInfo;
        /// <summary>
        /// 当前逻辑的简介信息和编辑器信息
        /// </summary>
        public LGSummaryInfo summaryInfo => owner.operateData.summaryInfo;
        /// <summary>
        /// 当前逻辑图的编辑器信息
        /// </summary>
        public GraphEditorData editorData => owner.operateData.editorData;
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
        /// 当前节点视图对应的节点
        /// </summary>
        public BaseLogicGraph target { get; private set; }
        /// <summary>
        /// 创建节点的搜索窗口
        /// </summary>
        private CreateNodeSearchWindow _createNodeSearch = null;

        /// <summary>
        /// 节点唯一ID
        /// </summary>
        private int _nodeUniqueId = NODE_START_ID;
        /// <summary>
        /// 没有使用到的Id
        /// </summary>
        private Queue<int> _unusedIds = new Queue<int>();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    partial class LogicGraphView
    {
        public LogicGraphView()
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            Input.imeCompositionMode = IMECompositionMode.On;
            m_addGridBackGround();
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
        }

        public void Initialize(LGWindow lgWindow, BaseLogicGraph graph)
        {
            owner = lgWindow;
            target = graph;
            m_initNodeUniqueId();
            editorData.NodeDatas.RemoveAll(a => graph.GetNode(a.OnlyId) == null);
            target.Nodes.RemoveAll(a => editorData.NodeDatas.FirstOrDefault(b => a.OnlyId == b.OnlyId) == null);
            editorData.NodeDatas.ForEach(a =>
            {
                a.node = graph.GetNode(a.OnlyId);
                m_showNodeView(a);
            });
            viewTransform.position = editorData.Pos;
            viewTransform.scale = editorData.Scale;
            graphViewChanged = m_onGraphViewChanged;
            viewTransformChanged = m_onViewTransformChanged;
        }
    }

    #region 公共方法

    //公共方法
    partial class LogicGraphView
    {
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
        public BaseLogicNode AddNode(Type nodeType, Vector2 pos)
        {
            BaseLogicNode logicNode = Activator.CreateInstance(nodeType) as BaseLogicNode;
            LogicNodeCategory nodeCategory = categoryInfo.Nodes.FirstOrDefault(a => a.NodeType == nodeType);
            if (nodeCategory != null)
            {
            }
            NodeEditorData data = new NodeEditorData();
            data.node = logicNode;
            data.Pos = pos;
            data.Title = nodeCategory.NodeName;
            m_setNodeId(logicNode);
            data.OnlyId = logicNode.OnlyId;
            this.target.Nodes.Add(logicNode);
            this.editorData.NodeDatas.Add(data);
            m_showNodeView(data);
            return logicNode;
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
    partial class LogicGraphView
    {
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("创建节点", m_onCreateNodeWindow, DropdownMenuAction.AlwaysEnabled);
            evt.menu.AppendAction("创建分组", null, DropdownMenuAction.AlwaysEnabled);
            evt.menu.AppendAction("创建默认节点", null, DropdownMenuAction.AlwaysEnabled);
            evt.menu.AppendSeparator();
            foreach (var item in categoryInfo.Formats)
            {
                evt.menu.AppendAction("导出: " + item.FormatName, null, DropdownMenuAction.AlwaysEnabled, item);
            }
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList();
        }

    }

    #endregion

    #region 回调

    //回调
    partial class LogicGraphView
    {
        private GraphViewChange m_onGraphViewChanged(GraphViewChange graphViewChange)
        {
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
    }

    #endregion

    #region 私有

    //私有
    partial class LogicGraphView
    {
        private void m_showNodeView(NodeEditorData editorData)
        {
            BaseNodeView nodeView = new BaseNodeView();
            this.AddElement(nodeView);
            nodeView.Initialize(this, editorData);
            nodeView.SetPosition(new Rect(editorData.Pos, Vector2.one));
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
                field.SetValue(node, id);
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
                int node = target.Nodes.Max(a => a.OnlyId);
                _nodeUniqueId = node + 1;
                for (int i = NODE_START_ID; i < _nodeUniqueId; i++)
                {
                    if (target.GetNode(i) == null)
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
    partial class LogicGraphView
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
            ////扩展大小与父对象相同
            //this.StretchToParentSize();
        }
    }
}
