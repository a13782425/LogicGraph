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
        [Output("延时2")]
        public string delayTime2;
        [Input("测试")]
        [SerializeReference]
        public DebugNode node;
        [Output("测试2")]
        [SerializeReference]
        public DebugNode node2;
    }
}