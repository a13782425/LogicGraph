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
        public delegate bool CheckPortFunc(bool result, NodePort port);
        private const BindingFlags FLAG = BindingFlags.Instance | BindingFlags.Public;
        private const string STYLE_PATH = "GraphView/NodePort.uss";

        public BaseGraphView owner { get; private set; }

        public BaseNodeView nodeView { get; private set; }

        public FieldInfo fieldInfo { get; private set; }
        /// <summary>
        /// 端口朝向
        /// </summary>
        public PortDirEnum portDir { get; private set; }

        /// <summary>
        /// 变量视图
        /// </summary>
        private VisualElement _varElement = null;
        /// <summary>
        /// 可以连接到某个端口
        /// Out端口调用
        /// </summary>
        public event CheckPortFunc onCanLinkPort;
        /// <summary>
        /// 是否接受某个端口的连接
        /// In端口调用
        /// </summary>
        public event CheckPortFunc onAcceptPort;
        /// <summary>
        /// 添加一个端口
        /// </summary>
        public event Action<NodePort> onAddPort;
        /// <summary>
        /// 删除一个端口
        /// </summary>
        public event Action<NodePort> onDelPort;


        public NodePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            this.AddToClassList("Port_" + direction);
        }
        /// <summary>
        /// 添加一个端口
        /// 进出端口都会调用
        /// </summary>
        /// <param name="port">添加的端口</param>
        internal void AddPort(NodePort port)
        {
            if (onAddPort != null)
            {
                onAddPort.Invoke(port);
            }
            if (_varElement != null)
            {
                _varElement.Hide();
            }
        }

        /// <summary>
        /// 删除一个端口
        /// 进出端口都会调用
        /// </summary>
        /// <param name="port">删除的端口</param>
        internal void DelPort(NodePort port)
        {
            if (onDelPort != null)
            {
                onDelPort.Invoke(port);
            }
            if (_varElement != null)
            {
                _varElement.Show();
            }
        }

        public override void Connect(Edge edge)
        {
            base.Connect(edge);
            NodePort tempPort = edge.input == this ? edge.output as NodePort : edge.input as NodePort;
            if (onAddPort != null)
            {
                onAddPort.Invoke(tempPort);
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
                onDelPort.Invoke(tempPort);
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

        /// <summary>
        /// 是否接受某个端口的连接
        /// In端口调用
        /// </summary>
        /// <param name="port">等待连接的Out端口</param>
        internal bool AcceptPort(NodePort port)
        {
            bool result = m_internalCheckLink(port);
            if (onAcceptPort != null)
            {
                result = onAcceptPort.Invoke(result, port);
            }
            return result;
        }

        internal void m_initialize()
        {
            if (fieldInfo != null)
            {
                //if (!this.fieldInfo.FieldType.IsAssignableFrom(typeof(BaseLogicNode)))
                {
                    this.AddToClassList(LogicUtils.PORT_CUBE);
                    this.portColor = LogicUtils.GetColor(fieldInfo.FieldType);
                    if (this.direction == Direction.Input)
                    {
                        m_createLinkElement();
                    }
                }
            }
        }
        private void m_createLinkElement()
        {
            void onValueChange(object value)
            {
                fieldInfo?.SetValue(nodeView.target, value);
            }
            Type type = this.fieldInfo.FieldType;
            if (NodeElementUtils.InputElementMapping.ContainsKey(type))
            {
                Type elementType = NodeElementUtils.InputElementMapping[type];
                IInputElement visual = Activator.CreateInstance(elementType) as IInputElement;
                visual.value = fieldInfo.GetValue(nodeView.target);
                visual.onValueChanged += onValueChange;
                _varElement = visual as VisualElement;
                _varElement.AddToClassList("port-input-element");
                this.Insert(0, _varElement);
            }
            //else
            //{
            //    throw new NotSupportedException($"暂不支持:{type.Name}类型,请联系开发者");
            //}
        }
        /// <summary>
        /// 默认的检测是否可以连接
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private bool m_internalCheckLink(NodePort port)
        {
            bool result = false;

            if (this.node is VarNodeView varNodeView)
            {
                //当前端口所处节点为变量节点

                if (port.node is VarNodeView)
                {
                    //目标端口所在节点是变量节点
                    result = false;
                }
                else
                {
                    //目标端口所在节点不是变量节点
                    if (port.fieldInfo == null)
                    {
                        //目标端口所在节点没有字段关联
                        result = false;
                    }
                    else
                    {
                        //目标端口所在节点有字段关联
                        if (port.fieldInfo.FieldType == varNodeView.target.GetValueType())
                        {
                            result = true;
                        }
                        else if (port.fieldInfo.FieldType.IsAssignableFrom(varNodeView.target.GetValueType()))
                        {
                            result = true;
                        }
                        else
                            result = false;
                    }
                }
            }
            else
            {
                //当前端口所处节点不是变量节点
                if (port.node is VarNodeView varView)
                {
                    //目标端口所在节点是变量节点
                }
                else
                {
                    //目标端口所在节点不是变量节点
                    if (port.fieldInfo == null)
                    {
                        result = true;
                    }

                }

            }

            //if (this.fieldInfo != null)
            //{
            //    //当前端口绑定了变量

            //    if (this.fieldInfo.FieldType.IsValueType)
            //    {
            //        //当前变量类型是值类型
            //        if (port.fieldInfo == null)
            //        {
            //            result = false;
            //        }
            //        else if (port.fieldInfo.FieldType.IsValueType)
            //        {

            //        }
            //    }
            //    else
            //    {
            //        //当前变量类型是引用类型

            //    }

            //    if (this.fieldInfo.FieldType == typeof(BaseLogicNode))
            //    {
            //        //如果当前变量类型是

            //    }
            //}
            //else
            //{
            //    //当前端口没有绑定变量

            //    m_noFieldCheck(port, ref result);

            //}
            return result;
        }

        /// <summary>
        /// 没有字段判断
        /// </summary>
        /// <param name="tarPort">目标端口</param>
        /// <param name="result"></param>
        private void m_noFieldCheck(NodePort tarPort, ref bool result)
        {
            if (tarPort.node is VarNodeView)
            {
                result = false;
            }
            else
            {
                if (tarPort.fieldInfo == null)
                {
                    //目标端口没有变量
                    result = true;
                }
                else if (tarPort.fieldInfo.FieldType.IsValueType)
                {
                    //目标端口有变量,但变量类型为值类型
                    result = false;
                }
                else if (typeof(BaseLogicNode).IsAssignableFrom(tarPort.fieldInfo.FieldType))
                {
                    result = true;
                }
            }
        }

        protected override void ExecuteDefaultAction(EventBase evt)
        {
            if (direction == Direction.Output)
            {
                base.ExecuteDefaultAction(evt);
            }
        }

        public static NodePort CreatePort(BaseGraphView graphView, Node node, PortDirEnum dir, EdgeConnectorListener edgeConnectorListener)
        {
            var port = new NodePort(Orientation.Horizontal, dir == PortDirEnum.In ? Direction.Input : Direction.Output, Capacity.Multi, null);
            port.m_EdgeConnector = new BaseEdgeConnector(edgeConnectorListener);
            port.nodeView = node as BaseNodeView;
            port.portDir = dir;
            port.owner = graphView;
            port.AddManipulator(port.m_EdgeConnector);
            port.m_initialize();
            return port;
        }
        public static NodePort CreatePort(BaseGraphView graphView, Node node, FieldInfo field, EdgeConnectorListener edgeConnectorListener)
        {
            if (field == null)
            {
                throw new ArgumentException($"当前重载字段参数不能为空");
            }
            InputAttribute input = field.GetCustomAttribute<InputAttribute>();
            OutputAttribute output = field.GetCustomAttribute<OutputAttribute>();
            PortDirEnum dir = PortDirEnum.All;
            dir = dir & (input != null ? PortDirEnum.In : PortDirEnum.All);
            dir = dir & (output != null ? PortDirEnum.Out : PortDirEnum.All);
            if (dir == PortDirEnum.None || dir == PortDirEnum.All)
            {
                throw new ArgumentException($"节点:{node.GetType().FullName}中{field.Name}字段没有Input或Output特性或者有多个Input或Output特性,请检查或使用其他重载");
            }
            return CreatePort(graphView, node, field, dir, edgeConnectorListener);
        }
        public static NodePort CreatePort(BaseGraphView graphView, Node node, FieldInfo field, PortDirEnum dir, EdgeConnectorListener edgeConnectorListener)
        {
            var port = new NodePort(Orientation.Horizontal, dir == PortDirEnum.In ? Direction.Input : Direction.Output, Capacity.Multi, null);
            port.m_EdgeConnector = new BaseEdgeConnector(edgeConnectorListener);
            port.fieldInfo = field;
            port.nodeView = node as BaseNodeView;
            port.portDir = dir;
            port.owner = graphView;
            port.AddManipulator(port.m_EdgeConnector);
            port.m_initialize();
            return port;
        }
    }
}
