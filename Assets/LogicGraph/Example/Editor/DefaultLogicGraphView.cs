using Game.Logic.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Logic.Example.Editor
{
    [LogicGraph("默认逻辑图", typeof(DefaultLogicGraph))]
    public class DefaultLogicGraphView : BaseGraphView
    {
        public override List<Type> DefaultNodes => new List<Type>() { typeof(StartNode) };

    }
}