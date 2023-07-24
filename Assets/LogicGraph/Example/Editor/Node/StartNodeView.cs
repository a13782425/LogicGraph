using Game.Logic.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Logic.Example.Editor
{
    [LogicNode(typeof(StartNode), "系统/开始", PortType = PortDirEnum.Out)]
    public class StartNodeView : BaseNodeView
    {
        private int abc;
        public override void ShowUI()
        {
            this.AddUI(
                UIToolkit.Column(
                    UIToolkit.Label("dsadas"),
                    UIToolkit.Label("qqqqqq"),
                    UIToolkit.Field("zxc", this.abc),
                    UIToolkit.Field("colore", Color.red),
                    UIToolkit.Field("col3e", new GameObject())));
        }

        private void onUpdate()
        {
            GUILayout.Label("dasdasd");
        }
    }
}
