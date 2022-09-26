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
        private const BindingFlags FLAG = BindingFlags.Instance | BindingFlags.Public;
        private const string STYLE_PATH = "GraphView/NodePort.uss";

        public BaseNodeView nodeView { get; private set; }

        public FieldInfo fieldInfo { get; private set; }

        /// <summary>
        /// 添加一个端口
        /// </summary>
        public event Action<NodePort> onAddPort;

        public NodePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            this.AddToClassList("Port_" + direction);
        }
        /// <summary>
        /// 添加一个端口
        /// </summary>
        /// <param name="port">添加的端口</param>
        internal void AddPort(NodePort port)
        {
            if (onAddPort != null)
            {
                onAddPort.Invoke(port);
                return;
            }
        }

        internal void Initialize(BaseNodeView nodeView, FieldInfo fieldInfo, string title)
        {
            this.nodeView = nodeView;
            this.portName = title;
            this.fieldInfo = fieldInfo;
            if (fieldInfo != null)
            {
                this.AddToClassList(LogicUtils.PORT_CUBE);
                this.portColor = LogicUtils.GetColor(fieldInfo.FieldType);
                var _linkFieldElement = m_createLinkElement();
                _linkFieldElement.AddToClassList("port-input-element");
                this.Insert(0, _linkFieldElement);
            }
        }
        private VisualElement m_createLinkElement()
        {
            void onValueChange(object value)
            {
                fieldInfo?.SetValue(nodeView.target, value);
            }
            Type type = this.fieldInfo.FieldType;
            if (type == typeof(int))
            {
                IntegerField element = new IntegerField();
                element.RegisterCallback<ChangeEvent<int>>(e => onValueChange(e.newValue));
                element.value = (int)fieldInfo.GetValue(nodeView.target);
                return element;
            }
            else if (type == typeof(float))
            {
                FloatField element = new FloatField();
                element.RegisterCallback<ChangeEvent<float>>(e => onValueChange(e.newValue));
                element.value = (float)fieldInfo.GetValue(nodeView.target);
                return element;
            }
            else if (type == typeof(string))
            {
                TextField element = new TextField();
                element.RegisterCallback<ChangeEvent<string>>(e => onValueChange(e.newValue));
                element.value = (string)fieldInfo.GetValue(nodeView.target);
                return element;
            }
            else if (type == typeof(double))
            {
                DoubleField element = new DoubleField();
                element.RegisterCallback<ChangeEvent<double>>(e => onValueChange(e.newValue));
                element.value = (int)fieldInfo.GetValue(nodeView.target);
                return element;
            }
            else if (type == typeof(bool))
            {
                Toggle element = new Toggle();
                element.RegisterCallback<ChangeEvent<bool>>(e => onValueChange(e.newValue));
                element.value = (bool)fieldInfo.GetValue(nodeView.target);
                return element;
            }
            else if (type == typeof(Vector2))
            {
                Vector2Field element = new Vector2Field();
                element.RegisterCallback<ChangeEvent<Vector2>>(e => onValueChange(e.newValue));
                element.value = (Vector2)fieldInfo.GetValue(nodeView.target);
                return element;
            }
            else if (type == typeof(Vector3))
            {
                Vector3Field element = new Vector3Field();
                element.RegisterCallback<ChangeEvent<Vector3>>(e => onValueChange(e.newValue));
                element.value = (Vector3)fieldInfo.GetValue(nodeView.target);
                return element;
            }
            else if (type == typeof(Vector4))
            {
                Vector4Field element = new Vector4Field();
                element.RegisterCallback<ChangeEvent<Vector4>>(e => onValueChange(e.newValue));
                element.value = (Vector4)fieldInfo.GetValue(nodeView.target);
                return element;
            }
            else if (type == typeof(Color))
            {
                ColorField element = new ColorField();
                element.RegisterCallback<ChangeEvent<Color>>(e => onValueChange(e.newValue));
                element.value = (Color)fieldInfo.GetValue(nodeView.target);
                return element;
            }
            else
            {
                throw new NotSupportedException($"暂不支持:{type.Name}类型,请联系开发者");
            }
        }
        protected override void ExecuteDefaultAction(EventBase evt)
        {
            if (direction == Direction.Output)
            {
                base.ExecuteDefaultAction(evt);
            }
        }
        public static NodePort CreatePort(PortDirEnum dir, EdgeConnectorListener edgeConnectorListener)
        {
            var port = new NodePort(Orientation.Horizontal, dir == PortDirEnum.In ? Direction.Input : Direction.Output, Capacity.Multi, null);
            port.m_EdgeConnector = new BaseEdgeConnector(edgeConnectorListener);
            port.AddManipulator(port.m_EdgeConnector);
            return port;
        }


    }
}
