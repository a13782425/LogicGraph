using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Logic.Runtime
{
    /// <summary>
    /// Mono的逻辑图运行时
    /// </summary>
    public class MonoLogicRuntime : MonoBehaviour, ILogicRuntime
    {
        [SerializeField]
        private BaseLogicGraph logicGraph;

        [SerializeReference]
        [SerializeField]
        public List<IVariable> vars = new List<IVariable>();
    }

}