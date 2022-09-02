﻿using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public partial class LogicGraphView : GraphView
    {
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
    }

    /// <summary>
    /// 初始化
    /// </summary>
    partial class LogicGraphView
    {
        public LogicGraphView()
        {
            Input.imeCompositionMode = IMECompositionMode.On;
            m_addGridBackGround();
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
            //扩展大小与父对象相同
            this.StretchToParentSize();
        }
    }

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
            //扩展大小与父对象相同
            this.StretchToParentSize();
        }
    }
}