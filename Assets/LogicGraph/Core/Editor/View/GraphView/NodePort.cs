using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public sealed class NodePort : Port
    {
        private const BindingFlags FLAG = BindingFlags.Instance | BindingFlags.Public;
        private const string STYLE_PATH = "GraphView/NodePort.uss";

        public BaseNodeView nodeView { get; private set; }

        public NodePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            this.AddToClassList("Port_" + direction);
        }
        internal void Initialize(BaseNodeView nodeView, FieldInfo fieldInfo, string title)
        {
            this.nodeView = nodeView;
            this.portName = title;
            if (fieldInfo != null)
            {
                this.AddToClassList(LogicUtils.PORT_CUBE);
                this.portColor = LogicUtils.GetColor(fieldInfo.FieldType);
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
