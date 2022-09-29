using Game.Logic.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Logic.Example
{
    public class DelayNode : BaseLogicNode
    {
        [Input("延时")]
        public float delayTime;

        [Input("测试")]
        public DebugNode node;
        [Output("测试2")]
        public DebugNode node2;
    }
}