using Game.Logic.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Logic.Example
{
    public class DebugNode : BaseLogicNode
    {
        [Input("文本")]
        public string log;

        [Output("输出")]
        public string log1;
    }

}