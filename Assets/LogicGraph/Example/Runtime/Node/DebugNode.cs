using Game.Logic.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Logic.Example
{
    public class DebugNode : BaseLogicNode
    {
        [VarInput("文本")]
        public string log;
    }

}