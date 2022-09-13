using Game.Logic.Editor;
using Game.Logic.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Logic.Example.Editor
{

    public class JsonFormat
    {
        [GraphFormat("Json", typeof(DefaultLogicGraph))]
        public static bool MyFormat(BaseLogicGraph graph, string path)
        {
            return true;
        }
    }
}