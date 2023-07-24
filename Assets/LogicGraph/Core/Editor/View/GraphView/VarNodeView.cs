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
    /// 变量节点
    /// </summary>
    internal sealed class VarNodeView : Node
    {
        private const string STYLE_PATH = "Uss/GraphView/VarNodeView.uss";

        /// <summary>
        /// 入端口
        /// </summary>
        internal NodePort Input { get; private set; }

        /// <summary>
        /// 出端口
        /// </summary>
        internal NodePort OutPut { get; private set; }
        internal BaseGraphView owner { get; private set; }
        internal IVariable target { get; private set; }
        internal VarNodeEditorData editorData { get; private set; }

        internal VarNodeView()
        {
            this.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
        }
        internal void Initialize(BaseGraphView owner, VarNodeEditorData editorData)
        {
            this.owner = owner;
            this.target = editorData.owner.target;
            this.editorData = editorData;
            m_initNodeView();
            Input.portColor = LogicUtils.GetColor(this.target.GetValueType());
            Input.onAddPort += Input_onAddPort;
            Input.onDelPort += Input_onDelPort;
            OutPut.portColor = LogicUtils.GetColor(this.target.GetValueType());
            OutPut.onAddPort += OutPut_onAddPort;
            OutPut.onDelPort += OutPut_onDelPort;
            this.title = this.editorData.owner.Name;
            this.SetPosition(new Rect(this.editorData.Pos, Vector2.zero));
            this.owner.owner.AddListener(LogicEventId.VAR_MODIFY, onVarModify);
            editorData.Edges.RemoveAll(a =>
            {
                var nodeView = owner.GetNodeView(a.NodeId) as BaseNodeView;
                if (nodeView == null)
                    return true;
                return nodeView.GetPortByFieldName(a.NodeFieldName, !a.IsIn) == null;
            });
            editorData.Edges.ForEach(a =>
            {
                var nodeView = owner.GetNodeView(a.NodeId) as BaseNodeView;
                var nodePort = nodeView.GetPortByFieldName(a.NodeFieldName, !a.IsIn);
                if (a.IsIn)
                    NodePort.JustLinkPort(nodePort, Input);
                else
                    NodePort.JustLinkPort(OutPut, nodePort);
            });
        }

        private void OutPut_onDelPort(NodePort curPort, NodePort tarPort)
        {
            int nodeId = (tarPort.node as BaseNodeView).target.OnlyId;
            string fieldName = tarPort.fieldInfo.Name;
            this.editorData.Edges.RemoveAll(a => a.NodeId == nodeId && a.NodeFieldName == fieldName && !a.IsIn);
        }

        private void Input_onDelPort(NodePort curPort, NodePort tarPort)
        {
            int nodeId = (tarPort.node as BaseNodeView).target.OnlyId;
            string fieldName = tarPort.fieldInfo.Name;
            this.editorData.Edges.RemoveAll(a => a.NodeId == nodeId && a.NodeFieldName == fieldName && a.IsIn);
        }

        private void OutPut_onAddPort(NodePort cur, NodePort tarPort)
        {
            VarEdgeEditorData edgeEditorData = new VarEdgeEditorData();
            edgeEditorData.IsIn = false;
            edgeEditorData.NodeId = (tarPort.node as BaseNodeView).target.OnlyId;
            edgeEditorData.NodeFieldName = tarPort.fieldInfo.Name;
            this.editorData.Edges.Add(edgeEditorData);
        }

        private void Input_onAddPort(NodePort cur, NodePort tarPort)
        {
            VarEdgeEditorData edgeEditorData = new VarEdgeEditorData();
            edgeEditorData.IsIn = true;
            edgeEditorData.NodeId = (tarPort.node as BaseNodeView).target.OnlyId;
            edgeEditorData.NodeFieldName = tarPort.fieldInfo.Name;
            this.editorData.Edges.Add(edgeEditorData);
        }

        private bool onVarModify(object args)
        {
            if (args is VarModifyEventArgs varModify)
            {
                if (varModify.var == this.target)
                {
                    this.title = varModify.var.Name;
                    this.editorData.Name = varModify.var.Name;
                }
            }
            return true;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            this.editorData.Pos = newPos.position;
        }
        private void m_initNodeView()
        {
            //移除右上角折叠按钮
            titleButtonContainer.RemoveFromHierarchy();
            topContainer.style.height = 24;
            this.AddToClassList("varNode");
            this.inputContainer.RemoveFromHierarchy();
            this.outputContainer.RemoveFromHierarchy();
            var titleLabel = this.titleContainer.Q("title-label");
            titleLabel.RemoveFromHierarchy();
            this.titleContainer.Add(this.inputContainer);
            this.titleContainer.Add(titleLabel);
            this.titleContainer.Add(this.outputContainer);
            Input = m_showPort(PortDirEnum.In);
            this.inputContainer.Add(Input);
            OutPut = m_showPort(PortDirEnum.Out);
            this.outputContainer.Add(OutPut);
            var contents = this.Q("contents");
            contents.RemoveFromHierarchy();
        }

        private NodePort m_showPort(PortDirEnum dir)
        {
            var port = NodePort.CreatePort(owner, this, dir);
            port.portName = "";
            return port;
        }

    }
}
