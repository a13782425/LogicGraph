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
        private const string STYLE_PATH = "GraphView/VarNodeView.uss";

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
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
        }
        internal void Initialize(BaseGraphView owner, VarNodeEditorData editorData)
        {
            this.owner = owner;
            this.target = editorData.owner.target;
            this.editorData = editorData;
            this.owner.AddElement(this);
            m_initNodeView();
            Input.portColor = LogicUtils.GetColor(this.target.GetValueType());
            Input.onAddPort += Input_onAddPort;
            OutPut.portColor = LogicUtils.GetColor(this.target.GetValueType());
            OutPut.onAddPort += OutPut_onAddPort;
            this.title = this.editorData.owner.Name;
            this.SetPosition(new Rect(this.editorData.Pos, Vector2.zero));
            this.owner.owner.AddListener(LogicEventId.VAR_MODIFY, onVarModify);
        }

        private void OutPut_onAddPort(NodePort obj)
        {
            VarEdgeEditorData edgeEditorData = new VarEdgeEditorData();
            edgeEditorData.IsIn = false;
            edgeEditorData.NodeId = obj.nodeView.target.OnlyId;
            edgeEditorData.NodeFieldName = obj.fieldInfo.Name;
            this.editorData.Edges.Add(edgeEditorData);
        }

        private void Input_onAddPort(NodePort obj)
        {
            VarEdgeEditorData edgeEditorData = new VarEdgeEditorData();
            edgeEditorData.IsIn = true;
            edgeEditorData.NodeId = obj.nodeView.target.OnlyId;
            edgeEditorData.NodeFieldName = obj.fieldInfo.Name;
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
            var port = NodePort.CreatePort(dir, owner.ConnectorListener);
            port.Initialize(null, null, "");
            return port;
        }

    }
}
