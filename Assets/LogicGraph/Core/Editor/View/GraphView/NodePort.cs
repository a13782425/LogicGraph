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
    public sealed class NodePort : Port
    {
        /// <summary>
        /// 检测一个端口是否可以被链接或者连接
        /// </summary>
        /// <param name="result">默认计算的结果</param>
        /// <param name="port">等待计算的端口</param>
        /// <returns></returns>
        public delegate bool PortCanLinkFunc(bool result, NodePort port);
        /// <summary>
        /// 当端口连接状态发生改变时候
        /// </summary>
        /// <param name="curPort">当前端口</param>
        /// <param name="tarPort">目标端口</param>
        public delegate void PortModifyAction(NodePort curPort, NodePort tarPort);
        private const string STYLE_PATH = "Uss/GraphView/NodePort.uss";
        private const string PORT_TYPE_CLASS = "base_port";

        public BaseGraphView owner { get; private set; }

        public new Node node { get; private set; }

        private FieldInfo _fieldInfo = null;

        /// <summary>
        /// 当前节点关联的端口
        /// </summary>
        public FieldInfo fieldInfo
        {
            get { return _fieldInfo; }
            set
            {
                if (_fieldInfo != null)
                {
                    this.RemoveFromClassList(LogicUtils.PORT_CUBE);
                    this.OnCustomStyleResolved(this.customStyle);
                }
                _fieldInfo = value;
                if (value != null)
                {
                    this.AddToClassList(LogicUtils.PORT_CUBE);
                    this.portColor = LogicUtils.GetColor(fieldInfo.FieldType);
                    if (this.direction == Direction.Input)
                    {
                        m_createLinkElement();
                    }
                    this.portType = _fieldInfo.FieldType;
                }
            }
        }
        /// <summary>
        /// 是否是基础的端口
        /// </summary>
        public bool IsBasePort => this.ClassListContains(PORT_TYPE_CLASS);
        ///// <summary>
        ///// 端口朝向
        ///// </summary>
        //public PortDirEnum portDir { get; private set; }

        /// <summary>
        /// 变量视图
        /// </summary>
        private VisualElement _varElement = null;
        /// <summary>
        /// 可以连接到某个端口
        /// Out端口调用
        /// </summary>
        public event PortCanLinkFunc onCanLinkPort;
        /// <summary>
        /// 是否接受某个端口的连接
        /// In端口调用
        /// </summary>
        //public event PortCanLinkFunc onAcceptPort;
        /// <summary>
        /// 添加一个端口
        /// </summary>
        public event PortModifyAction onAddPort;
        /// <summary>
        /// 删除一个端口
        /// </summary>
        public event PortModifyAction onDelPort;


        public NodePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
            this.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
            this.AddToClassList("Port_" + direction);
        }
        ///// <summary>
        ///// 添加一个端口
        ///// 进出端口都会调用
        ///// </summary>
        ///// <param name="port">添加的端口</param>
        //internal void AddPort(NodePort port)
        //{
        //    if (onAddPort != null)
        //    {
        //        onAddPort.Invoke(this, port);
        //    }
        //    if (_varElement != null)
        //    {
        //        _varElement.Hide();
        //    }
        //}

        ///// <summary>
        ///// 删除一个端口
        ///// 进出端口都会调用
        ///// </summary>
        ///// <param name="port">删除的端口</param>
        //internal void DelPort(NodePort port)
        //{
        //    if (onDelPort != null)
        //    {
        //        onDelPort.Invoke(this, port);
        //    }
        //    if (_varElement != null)
        //    {
        //        _varElement.Show();
        //    }
        //}

        public override void Connect(Edge edge)
        {
            base.Connect(edge);
            NodePort tempPort = edge.input == this ? edge.output as NodePort : edge.input as NodePort;
            if (onAddPort != null)
            {
                onAddPort.Invoke(this, tempPort);
            }
            if (_varElement != null)
            {
                _varElement.Hide();
            }
        }
        public override void Disconnect(Edge edge)
        {
            base.Disconnect(edge);
            NodePort tempPort = edge.input == this ? edge.output as NodePort : edge.input as NodePort;
            if (onDelPort != null)
            {
                onDelPort.Invoke(this, tempPort);
            }
            if (_varElement != null)
            {
                _varElement.Show();
            }
        }



        /// <summary>
        /// 可以连接到某个端口
        /// Out端口调用
        /// </summary>
        /// <param name="port">等待连接的In端口</param>
        /// <returns></returns>
        internal bool CanLinkPort(NodePort port)
        {
            bool result = m_internalCheckLink(port);

            if (onCanLinkPort != null)
            {
                result = onCanLinkPort.Invoke(result, port);
            }
            return result;
        }

        ///// <summary>
        ///// 是否接受某个端口的连接
        ///// In端口调用
        ///// </summary>
        ///// <param name="port">等待连接的Out端口</param>
        //internal bool AcceptPort(NodePort port)
        //{
        //    bool result = m_internalCheckLink(port);
        //    if (onAcceptPort != null)
        //    {
        //        result = onAcceptPort.Invoke(result, port);
        //    }
        //    return result;
        //}

        internal void m_initialize()
        {
            if (node is VarNodeView varNodeView)
            {
                this.portType = varNodeView.target.GetValueType();
            }
            else
            {
                this.portType = (this.node as BaseNodeView).target.GetType();
            }
        }
        private void m_createLinkElement()
        {
            void onValueChange(object value)
            {
                fieldInfo?.SetValue((node as BaseNodeView).target, value);
            }
            Type type = this.fieldInfo.FieldType;
            if (NodeElementUtils.InputElementMapping.ContainsKey(type))
            {
                Type elementType = NodeElementUtils.InputElementMapping[type];
                IInputElement visual = Activator.CreateInstance(elementType) as IInputElement;
                visual.value = fieldInfo.GetValue((node as BaseNodeView).target);
                visual.onValueChanged += onValueChange;
                _varElement = visual as VisualElement;
                _varElement.AddToClassList("port-input-element");
                this.Insert(0, _varElement);
            }
        }
        /// <summary>
        /// 默认的检测是否可以连接
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private bool m_internalCheckLink(NodePort port)
        {
            bool result = false;

            bool curBase = this.IsBasePort;
            bool tarBase = port.IsBasePort;
            if (curBase && tarBase)
            {
                result = true;
            }
            else if (port.direction == Direction.Input && port.fieldInfo != null && port.connections.Count() > 0)
            {
                result = false;
            }
            else if (this.portType == port.portType)
            {
                if (this.node is VarNodeView && port.node is VarNodeView)
                    result = false;
                else if (this.node is BaseNodeView && port.node is BaseNodeView && !curBase && !tarBase)
                    result = false;
                else
                    result = true;
            }
            else if (this.portType == typeof(BaseLogicNode))
            {
                result = typeof(BaseLogicNode).IsAssignableFrom(port.portType);
            }
            else if (port.portType.IsAssignableFrom(this.portType))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected override void ExecuteDefaultAction(EventBase evt)
        {
            if (direction == Direction.Output)
            {
                base.ExecuteDefaultAction(evt);
            }
        }
        private void m_drawLink(EdgeView edge)
        {
            if (this._varElement != null)
            {
                this._varElement.Hide();
            }
            base.Connect(edge);
        }
        public static NodePort CreatePort(BaseGraphView graphView, Node node, PortDirEnum dir)
        {
            return CreatePort(graphView, node, null, dir);
        }
        public static NodePort CreatePort(BaseGraphView graphView, Node node, FieldInfo field, PortDirEnum dir)
        {
            if (graphView == null || node == null)
            {
                throw new ArgumentException("参数错误,graphView和node不能为空");
            }
            var port = new NodePort(Orientation.Horizontal, dir == PortDirEnum.In ? Direction.Input : Direction.Output, Capacity.Multi, null);
            port.m_EdgeConnector = new BaseEdgeConnector(graphView.ConnectorListener);
            port.node = node;
            port.owner = graphView;
            port.AddManipulator(port.m_EdgeConnector);
            port.m_initialize();
            port.fieldInfo = field;
            return port;
        }

        /// <summary>
        /// 仅链接两个端口，不触发任何事件
        /// </summary>
        /// <param name="outPut"></param>
        /// <param name="input"></param>
        public static void JusrLinkPort(NodePort outPut, NodePort input)
        {
            EdgeView edgeView = new EdgeView();
            edgeView.input = input;
            edgeView.output = outPut;

            outPut.m_drawLink(edgeView);
            input.m_drawLink(edgeView);

            outPut.owner.AddElement(edgeView);
        }
    }
}
