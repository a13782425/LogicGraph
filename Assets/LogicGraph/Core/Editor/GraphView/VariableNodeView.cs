using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Logic.Editor
{
    public sealed class VariableNodeView : BaseNodeView<VariableNode>
    {

        //protected override Node GetNode()
        //{
        //    var node = new VariableNodeVisualElement(this, this.owner);
        //    node.inputContainer.RemoveFromHierarchy();
        //    node.outputContainer.RemoveFromHierarchy();
        //    var titleLabel = node.titleContainer.Q("title-label");
        //    titleLabel.RemoveFromHierarchy();
        //    node.titleContainer.Add(node.inputContainer);
        //    node.titleContainer.Add(titleLabel);
        //    node.titleContainer.Add(node.outputContainer);
        //    Input = AddPort("", Direction.Input);
        //    node.inputContainer.Add(Input);
        //    OutPut = AddPort("", Direction.Output);
        //    node.outputContainer.Add(OutPut);
        //    var contents = node.Q("contents");
        //    contents.RemoveFromHierarchy();
        //    return node;
        //}
        public override void OnCreate()
        {
            Input.portColor = node.variable.GetColor();
            OutPut.portColor = node.variable.GetColor();

            if (this.owner != null)
            {
                this.owner.Event.AddEvent(LGEvent.VAR_MODDIFY, m_onModify);
            }
        }



        private void m_onModify(object param)
        {
            node.varName = node.variable.Name;
            this.title = node.variable.Name;
        }

        public override void OnDestroy()
        {
            if (this.owner!=null)
            {
                this.owner.Event.DelEvent(LGEvent.VAR_MODDIFY, m_onModify);
            }
        }

        //protected override void OnGenericMenu(ContextualMenuPopulateEvent evt)
        //{
        //    evt.menu.AppendAction("编辑描述", onOpenNodeDescribe);
        //    evt.menu.AppendSeparator();
        //    evt.menu.AppendAction("删除", (a) => owner.DeleteSelection());
        //}
        //public override void DrawLink() { }
        //public override bool CanLink(PortView ownerPort, PortView waitLinkPort)
        //{
        //    if (waitLinkPort.IsDefault)
        //    {
        //        return false;
        //    }
        //    if (waitLinkPort.Owner is VariableNodeView)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        protected override void initNodeView()
        {
            styleSheets.Add(LogicUtils.GetVariableNodeStyle());
            //移除右上角折叠按钮
            titleButtonContainer.RemoveFromHierarchy();
            topContainer.style.height = 24;
            this.SetPosition(new Rect(this.target.Pos, Vector2.zero));
            this.AddToClassList("varNode");
            this.inputContainer.RemoveFromHierarchy();
            this.outputContainer.RemoveFromHierarchy();
            var titleLabel = this.titleContainer.Q("title-label");
            titleLabel.RemoveFromHierarchy();
            this.titleContainer.Add(this.inputContainer);
            this.titleContainer.Add(titleLabel);
            this.titleContainer.Add(this.outputContainer);
            Input = ShowPort("", PortDirEnum.In);
            //Input = AddPort("", Direction.Input);
            this.inputContainer.Add(Input);
            //OutPut = AddPort("", Direction.Output);
            OutPut = ShowPort("", PortDirEnum.Out);
            this.outputContainer.Add(OutPut);
            var contents = this.Q("contents");
            contents.RemoveFromHierarchy();
        }

        //private class VariableNodeVisualElement : Node, INodeVisualElement
        //{

        //    public Color TitleBackgroundColor { get => titleContainer.style.backgroundColor.value; set => titleContainer.style.backgroundColor = value; }
        //    public Color ContentBackgroundColor { get => throw new Exception("变量节点不支持"); set => throw new Exception("变量节点不支持"); }
        //    public VisualElement ContentContainer { get; private set; }
        //    private BaseNodeView1 nodeView { get; set; }

        //    public event Action<ContextualMenuPopulateEvent> onGenericMenu;
        //    /// <summary>
        //    /// 逻辑图视图
        //    /// </summary>
        //    private BaseGraphView _graphView;


        //    public VariableNodeVisualElement(BaseNodeView1 nodeView, BaseGraphView graphView)
        //    {
        //        this.nodeView = nodeView;
        //        _graphView = graphView;
        //        userData = nodeView;
        //        styleSheets.Add(LogicUtils.GetVariableNodeStyle());
        //        //移除右上角折叠按钮
        //        titleButtonContainer.RemoveFromHierarchy();
        //        topContainer.style.height = 24;
        //        this.title = this.nodeView.Title;
        //        this.SetPosition(new Rect(this.nodeView.Target.Pos, Vector2.zero));
        //        this.AddToClassList("varNode");
        //    }

        //    public void AddUI(VisualElement ui)
        //    {
        //        throw new Exception("变量节点无法添加UI");
        //    }

        //    public override void SetPosition(Rect newPos)
        //    {
        //        base.SetPosition(newPos);
        //        nodeView.Target.Pos = newPos.position;
        //    }

        //    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        //    {
        //        if (!this.selected || this._graphView.selection.Count > 1)
        //        {
        //            return;
        //        }
        //        onGenericMenu?.Invoke(evt);
        //        evt.StopPropagation();
        //    }
        //}

    }

}